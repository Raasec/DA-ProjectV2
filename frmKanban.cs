using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iTasks.Program;
using System.Data.Entity;

namespace iTasks
{
    public partial class frmKanban : Form
    {
        public frmKanban()
        {
            InitializeComponent();

            this.Load += frmKanban_Load;
        }

        private void LoadTarefas()
        {
            using (var db = new MyDbContext())
            {
                var tarefas = db.Tarefas.ToList();

                lstTodo.DataSource = tarefas.Where(t => t.EstadoAtual == (int)EstadoAtual.ToDo).ToList();
                lstTodo.DisplayMember = "Descricao";
                lstTodo.ValueMember = "Id";

                lstDoing.DataSource = tarefas.Where(t => t.EstadoAtual == (int)EstadoAtual.Doing).ToList();
                lstDoing.DisplayMember = "Descricao";
                lstDoing.ValueMember = "Id";

                lstDone.DataSource = tarefas.Where(t => t.EstadoAtual == (int)EstadoAtual.Done).ToList();
                lstDone.DisplayMember = "Descricao";
                lstDone.ValueMember = "Id";
            }
        }

        private void frmKanban_Load(object sender, EventArgs e)
        {
            LoadTarefas();
        }

        private void btNova_Click(object sender, EventArgs e)
        {
            using (var db = new MyDbContext())
            {
                var tarefa = new Tarefa
                {
                    Descricao = "Nova tarefa",
                    EstadoAtual = 0,
                    DataPrevistaInicio = DateTime.Now,
                    DataPrevistaFim = DateTime.Now.AddDays(7),
                    Ordem = 1,
                    ProgramadorId = 1,
                    GestorId = 1,
                    TipoTarefaId = 1
                };

                db.Tarefas.Add(tarefa);
                db.SaveChanges();

                MessageBox.Show("Tarefa adicionada com sucesso!");
            }
            LoadTarefas();
        }




        public enum EstadoAtual
        {
            ToDo = 0,
            Doing = 1,
            Done = 2
        }

        private void btSetDoing_Click(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa tarefa)
            {
                UpdateTarefaEstado(tarefa.Id, EstadoAtual.Doing);
            }

        }

        private void btSetTodo_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa tarefa)
            {
                UpdateTarefaEstado(tarefa.Id, EstadoAtual.ToDo);
            }
        }

        private void btSetDone_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa tarefa)
            {
                UpdateTarefaEstado(tarefa.Id, EstadoAtual.Done);
            }
        }


        private void UpdateTarefaEstado(int tarefaId, EstadoAtual novoEstado)
        {
            using (var db = new MyDbContext())
            {
                var tarefa = db.Tarefas.Find(tarefaId);
                if (tarefa != null)
                {
                    tarefa.EstadoAtual = (int)novoEstado;
                    db.SaveChanges();
                }
            }

            LoadTarefas();
        }









        private void lstTodo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}































/* 
 * 
 *         private void btNova_Click(object sender, EventArgs e)
        {
            string dbPath = AppDomain.CurrentDomain.BaseDirectory + @"\DatabaseUSER.mdf";
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Tarefa (Descricao, EstadoAtual, DataPrevistaInicio, DataPrevistaFim, Ordem, ProgramadorId, GestorId, TipoTarefaId) " +
                               "VALUES (@desc, @estado, @dataInicio, @dataFim, @ordem, @progId, @gestorId, @tipoId)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", "Nova tarefa");
                    cmd.Parameters.AddWithValue("@estado", (int)EstadoAtual.ToDo);
                    cmd.Parameters.AddWithValue("@dataInicio", DateTime.Now);
                    cmd.Parameters.AddWithValue("@dataFim", DateTime.Now.AddDays(7));
                    cmd.Parameters.AddWithValue("@ordem", 1);
                    cmd.Parameters.AddWithValue("@progId", 1);
                    cmd.Parameters.AddWithValue("@gestorId", 1);
                    cmd.Parameters.AddWithValue("@tipoId", 1);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Tarefa inserida com sucesso!");
            CarregarTarefas();
        }







    private void CarregarTarefas()
        {
            listTodo.Items.Clear();
            listDoing.Items.Clear();
            listDone.Items.Clear();

            string dbPath = AppDomain.CurrentDomain.BaseDirectory + @"\DatabaseUSER.mdf";
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Descricao, EstadoAtual FROM Tarefa";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string descricao = reader.GetString(0);
                        int estado = reader.GetInt32(1);

                        switch ((EstadoAtual)estado)
                        {
                            case EstadoAtual.ToDo:
                                listTodo.Items.Add(descricao);
                                break;
                            case EstadoAtual.Doing:
                                listDoing.Items.Add(descricao);
                                break;
                            case EstadoAtual.Done:
                                listDone.Items.Add(descricao);
                                break;
                        }
                    }
                }
            }

        }



        public enum EstadoAtual
        {
            ToDo = 0,
            Doing = 1,
            Done = 2
        }




*/