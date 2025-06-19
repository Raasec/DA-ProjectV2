using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmGereTiposTarefas : Form
    {
        // isto tem de ser dinamico, n funciona no meu pc mas eu tirei do frmlogin
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\redom\Desktop\DA-MDS-PROJECT\IPL-DA-PROJECT\DatabaseUSER.mdf';Integrated Security=True";

        public frmGereTiposTarefas()
        {
            InitializeComponent();
        }

        private void frmGereTiposTarefas_Load(object sender, EventArgs e)
        {
            AtualizarLista();
        }

        // lê da BD e mete tudo na listbox
        private void AtualizarLista()
        {
            lstLista.Items.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM TipoTarefa ORDER BY TipoTarefaId";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // mostra "id - descrição"
                    string linha = $"{reader["TipoTarefaId"]} - {reader["TipoTarefaDescricao"]}";
                    lstLista.Items.Add(linha);
                }
            }
            // limpa os inputs depois de carregar para conveniencia
            txtId.Clear();
            txtDesc.Clear();
        }

        // quando se seleciona item na lista, preenche os campos 
        private void lstLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLista.SelectedItem == null) return;

            string linha = lstLista.SelectedItem.ToString();
            string[] partes = linha.Split('-');
            txtId.Text = partes[0].Trim();
            txtDesc.Text = partes[1].Trim();
        }

        // botão para guardar ou atualizar
        private void btnGravarDados_Click(object sender, EventArgs e)
        {
            string descricao = txtDesc.Text.Trim();

            if (string.IsNullOrWhiteSpace(descricao))
            {
                MessageBox.Show("Preenche a descrição.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    // novo tipo de tarefa
                    string insert = "INSERT INTO TipoTarefa (TipoTarefaDescricao) VALUES (@descricao)";
                    SqlCommand cmd = new SqlCommand(insert, conn);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Tipo de tarefa adicionado.");
                }
                else
                {
                    // atualizar tipo existente
                    int id = int.Parse(txtId.Text);
                    string update = "UPDATE TipoTarefa SET TipoTarefaDescricao = @descricao WHERE TipoTarefaId = @id";
                    SqlCommand cmd = new SqlCommand(update, conn);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Tipo de tarefa atualizado.");
                }
            }

            AtualizarLista();
        }
    }
}
