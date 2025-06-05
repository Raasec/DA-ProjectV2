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






























