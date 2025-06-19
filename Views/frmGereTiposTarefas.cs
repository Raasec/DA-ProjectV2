using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmGereTiposTarefas : Form
    {
        public frmGereTiposTarefas()
        {
            InitializeComponent();
        }

        private void frmGereTiposTarefas_Load(object sender, EventArgs e)
        {
            AtualizarLista();
        }

        private void AtualizarLista()
        {
            lstLista.Items.Clear();

            using (var db = new Basededados())
            {
                var lista = db.tipoTarefas
                    .OrderBy(tipotarefa => tipotarefa.TipoTarefaId)
                    .ToList();

                foreach (var tipoTarefa in lista)
                {
                    // mostra "id - descricao"
                    string linha = $"{tipoTarefa.TipoTarefaId} - {tipoTarefa.TipoTarefaDesc}";
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

        private void btGravar_Click(object sender, EventArgs e)
        {
            string descricao = txtDesc.Text.Trim();

            if (string.IsNullOrWhiteSpace(descricao))
            {
                MessageBox.Show("Preenche a descricao.");
                return;
            }

            using (var db = new Basededados())
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    // novo tipo de tarefa
                    var novoTipo = new TipoTarefa
                    {
                        TipoTarefaDesc = descricao
                    };

                    db.tipoTarefas.Add(novoTipo);
                    db.SaveChanges();

                    MessageBox.Show("Tipo de tarefa adicionado.");
                }
                else
                {
                    // atualizar tipo existente
                    int id = int.Parse(txtId.Text);
                    var tipoExistente = db.tipoTarefas.Find(id);

                    if (tipoExistente != null)
                    {
                        tipoExistente.TipoTarefaDesc = descricao;
                        db.SaveChanges();

                        MessageBox.Show("Tipo de tarefa atualizado.");
                    }
                    else
                    {
                        MessageBox.Show("Tipo de tarefa não encontrado.");
                    }
                }
            }

            AtualizarLista();
        }
    }
}
