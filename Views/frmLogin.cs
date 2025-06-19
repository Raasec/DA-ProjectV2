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
            try
            {
                using (var db = new Basededados())
                {
                    // Procurar utilizador com username e password exatos
                    var user = db.Utilizadores
                        .SingleOrDefault(u => u.Username == username && u.Password == password);

                    if (user == null)
                    {
                        MessageBox.Show("Utilizador ou password inválidos.");
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao tentar fazer login: " + e.Message);
                return false;
            }
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (VerifyLogin(username, password))
            {
                MessageBox.Show("Login efetuado com sucesso");

                frmKanban kanban = new frmKanban();
                kanban.Show();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Autenticação falhou.");
            }
        }
    


    private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
