using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmDetalhesTarefa : Form
    {
        private int _tarefaId;
        public frmDetalhesTarefa(int tarefaId)
        {
            InitializeComponent();
            _tarefaId = tarefaId;
        }

        private void frmDetalhesTarefas_Load(object sender, EventArgs e)
        {
            using (var db = new Basededados())
            {
                var tarefa = db.Tarefas.Find(_tarefaId);
                if (tarefa == null)
                {
                    MessageBox.Show("Tarefa não encontrada.");
                    this.Close();
                    return;
                }

                // Textboxes
                txtId.Text = tarefa.Id.ToString();
                txtDataRealini.Text = tarefa.DataRealInicio.ToString("yyyy-MM-dd");
                txtdataRealFim.Text = tarefa.DataRealFim.ToString("yyyy-MM-dd");
                txtEstado.Text = tarefa.EstadoAtual.ToString();
                txtDataCriacao.Text = tarefa.DataCreation.ToString("yyyy-MM-dd");
                txtDesc.Text = tarefa.Descricao;
                txtOrdem.Text = tarefa.Ordem.ToString();
                txtStoryPoints.Text = tarefa.StoryPoints.ToString();

                // Calendars
                dtInicio.Value = tarefa.DataPrevistaInicio;
                dtFim.Value = tarefa.DataPrevistaFim;

                // Populate dropdowns
                cbTipoTarefa.DataSource = db.Set<TipoTarefa>().ToList();
                cbTipoTarefa.DisplayMember = "TipoTarefaName";
                cbTipoTarefa.ValueMember = "TipoTarefaId";
                cbTipoTarefa.SelectedValue = tarefa.TipoTarefaId;

                // Example: Load static programador list
                cbProgramador.DataSource = db.Programadors
                    .Select(p => new { p.Id, p.Nome })
                    .ToList();
                cbProgramador.DisplayMember = "Nome";
                cbProgramador.ValueMember = "Id";
                cbProgramador.SelectedValue = tarefa.ProgramadorId;
            }
        }



        private void btGravar_Click(object sender, EventArgs e)
        {
            using (var db = new Basededados())
            {
                var tarefa = db.Tarefas.Find(_tarefaId);
                if (tarefa == null)
                {
                    MessageBox.Show("Tarefa não encontrada.");
                    return;
                }

                // Update fields from UI
                tarefa.DataRealInicio = DateTime.Parse(txtDataRealini.Text);
                tarefa.DataRealFim = DateTime.Parse(txtdataRealFim.Text);
                tarefa.Descricao = txtDesc.Text;
                tarefa.EstadoAtual = (Tarefa.estadoatual)int.Parse(txtEstado.Text); // mudei o parse aqui 
                tarefa.DataPrevistaInicio = dtInicio.Value;
                tarefa.DataPrevistaFim = dtFim.Value;
                tarefa.Ordem = int.Parse(txtOrdem.Text);
                tarefa.StoryPoints = int.Parse(txtStoryPoints.Text);
                tarefa.TipoTarefaId = (int)cbTipoTarefa.SelectedValue;
                tarefa.ProgramadorId = (int)cbProgramador.SelectedValue;

                db.SaveChanges();
                MessageBox.Show("Alterações salvas com sucesso!");
                this.Close();
            }
        }


        private void btFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
