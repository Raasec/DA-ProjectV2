using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace iTasks
{
    public partial class frmLogin : Form
    {

        private const int SALTSIZE = 8;
        private const int NUMBER_OF_ITERATIONS = 1000;

        public frmLogin()
        {
            InitializeComponent();
        }

        private bool VerifyLogin(string username, string password)
        {
            SqlConnection conn = null;
            try
            {
                // Configurar ligação à Base de Dados
                //VERIFICAR O LINK para a DatabaseUSER.mdf
                //Está com link absoluto e precisa ser dinâmico

                conn = new SqlConnection();
                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\redom\Desktop\DA-MDS-PROJECT\IPL-DA-PROJECT\DatabaseUSER.mdf';Integrated Security=True");

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração do comando SQL
                String sql = "SELECT * FROM Users WHERE Username = @username";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;

                // Declaração dos parâmetros do comando SQL
                SqlParameter param = new SqlParameter("@username", username);

                // Introduzir valor ao parâmentro registado no comando SQL
                cmd.Parameters.Add(param);

                // Associar ligação à Base de Dados ao comando a ser executado
                cmd.Connection = conn;

                // Executar comando SQL
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("Error while trying to access an user");
                }

                // Ler resultado da pesquisa
                reader.Read();

                // Obter Hash (password + salt)
                byte[] saltedPasswordHashStored = (byte[])reader["SaltedPasswordHash"];

                // Obter salt
                byte[] saltStored = (byte[])reader["Salt"];

                conn.Close();

                //TODO: verificar se a password na base de dados 
                byte[] hash = GenerateSaltedHash(password, saltStored);

                return saltedPasswordHashStored.SequenceEqual(hash);


                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message);
                return false;
            }
        }

        private void Register(string username, byte[] saltedPasswordHash, byte[] salt)
        {
            SqlConnection conn = null;
            try
            {
                //ATENÇAO
                // Configurar ligação à Base de Dados
                //VERIFICAR O LINK para a DatabaseUSER.mdf
                //Está com link absoluto e precisa ser dinâmico

                conn = new SqlConnection();
                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\redom\Desktop\DA-MDS-PROJECT\IPL-DA-PROJECT\DatabaseUSER.mdf';Integrated Security=True");

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração dos parâmetros do comando SQL
                SqlParameter paramUsername = new SqlParameter("@username", username);
                SqlParameter paramPassHash = new SqlParameter("@saltedPasswordHash", saltedPasswordHash);
                SqlParameter paramSalt = new SqlParameter("@salt", salt);

                // Declaração do comando SQL
                String sql = "INSERT INTO Users (Username, SaltedPasswordHash, Salt) VALUES (@username,@saltedPasswordHash,@salt)";

                // Prepara comando SQL para ser executado na Base de Dados
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Introduzir valores aos parâmentros registados no comando SQL
                cmd.Parameters.Add(paramUsername);
                cmd.Parameters.Add(paramPassHash);
                cmd.Parameters.Add(paramSalt);

                // Executar comando SQL
                int lines = cmd.ExecuteNonQuery();

                // Fechar ligação
                conn.Close();
                if (lines == 0)
                {
                    // Se forem devolvidas 0 linhas alteradas então o não foi executado com sucesso
                    throw new Exception("Error while inserting an user");
                }
                MessageBox.Show("Utilizador Registado com suscesso!!!");
            }
            catch (Exception e)
            {
                throw new Exception("Error while inserting an user:" + e.Message);
            }
        }

        private static byte[] GenerateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return buff;
        }

        private static byte[] GenerateSaltedHash(string plainText, byte[] salt)
        {
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(plainText, salt, NUMBER_OF_ITERATIONS);
            return rfc2898.GetBytes(32);
        }



        private void btLogin_Click(object sender, EventArgs e)
        {

            String password = txtPassword.Text;
            String username = txtUsername.Text;

            if (VerifyLogin(username, password))
            {
                MessageBox.Show("O utilizador é váildo!!");

            }
            else
            {
                MessageBox.Show("Autenticação errada!!");
            }

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            String pass = txtPassword.Text;
            String username = txtUsername.Text;

            byte[] salt = GenerateSalt(SALTSIZE);
            byte[] hash = GenerateSaltedHash(pass, salt);

            Register(username, hash, salt);

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }


        //Transformar de butao para gerar automáticamente ao Registar
        //Criar um messageBox.Show para confirmar o registo 

        /*private void buttonGenerateSaltedHash_Click(object sender, EventArgs e)
        {
            String password = textBoxPassword.Text;

            byte[] salt = GenerateSalt(SALTSIZE);
            byte[] hash = GenerateSaltedHash(password, salt);

            textBoxSaltedHash.Text = Convert.ToBase64String(hash);

            textBoxSalt.Text = Convert.ToBase64String(salt);

            textBoxSizePass.Text = (hash.Length * 8).ToString();
            textBoxSizeSalt.Text = (salt.Length * 8).ToString();

        }
        */

    }
}
