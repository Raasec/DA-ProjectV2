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
            using (var db = new Basededados())
            {
                var tarefas = db.Tarefas.ToList();

                lstTodo.DataSource = tarefas
                    .Where(t => t.EstadoAtual == Tarefa.estadoatual.todo)
                    .OrderBy(t => t.Ordem)
                    .ToList();
                lstTodo.DisplayMember = "Descricao";
                lstTodo.ValueMember = "Id";

                lstDoing.DataSource = tarefas
                    .Where(t => t.EstadoAtual == Tarefa.estadoatual.doing)
                    .OrderBy(t => t.Ordem)
                    .ToList();
                lstDoing.DisplayMember = "Descricao";
                lstDoing.ValueMember = "Id";

                lstDone.DataSource = tarefas
                    .Where(t => t.EstadoAtual == Tarefa.estadoatual.done)
                    .OrderBy(t => t.Ordem)
                    .ToList();
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
            using (var db = new Basededados())
            {
                var novaTarefa = new Tarefa
                {
                    Descricao = "Nova tarefa",
                    EstadoAtual = Tarefa.estadoatual.todo,
                    DataPrevistaInicio = DateTime.Now,
                    DataPrevistaFim = DateTime.Now.AddDays(7),
                    DataRealInicio = DateTime.Now,
                    DataRealFim = DateTime.Now,
                    DataCreation = DateTime.Now,
                    Ordem = 1,
                    ProgramadorId = 1,
                    GestorId = 1,
                    TipoTarefaId = 1
                };

                db.Tarefas.Add(novaTarefa);
                db.SaveChanges();

                MessageBox.Show("Tarefa adicionada com sucesso!");
            }

            LoadTarefas();
        }

        private void btSetDoing_Click(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa tarefa)
            {
                UpdateTarefaEstado(tarefa.Id, Tarefa.estadoatual.doing);
            }
        }

        private void btSetTodo_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa tarefa)
            {
                UpdateTarefaEstado(tarefa.Id, Tarefa.estadoatual.todo);
            }
        }

        private void btSetDone_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa tarefa)
            {
                UpdateTarefaEstado(tarefa.Id, Tarefa.estadoatual.done);
            }
        }


        private void UpdateTarefaEstado(int tarefaId, Tarefa.estadoatual novoEstado)
        {
            using (var db = new Basededados())
            {
                var tarefa = db.Tarefas.FirstOrDefault(t => t.Id == tarefaId);
                if (tarefa != null)
                {
                    tarefa.EstadoAtual = novoEstado;
                    db.SaveChanges();
                }
            }

            LoadTarefas();
        }

        private void lstTodo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmKanban_Load_1(object sender, EventArgs e)
        {

        }
    }
}