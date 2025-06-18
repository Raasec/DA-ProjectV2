using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace iTasks
{
    public class Tarefa
    {

        public enum estadoatual
        {
            todo,
            doing,
            done
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public estadoatual EstadoAtual { get; set; }
        public DateTime DataPrevistaInicio { get; set; }
        public DateTime DataPrevistaFim { get; set; }
        public int Ordem { get; set; }
        public int ProgramadorId { get; set; }
        public int GestorId { get; set; }
        public int TipoTarefaId { get; set; }
        public int StoryPoints { get; set; }


        public DateTime DataRealInicio { get; set; }
        public DateTime DataRealFim { get; set; }
        public DateTime DataCreation { get; set; }

        public Gestor Gestor { get; set; }

        public Programador Programador { get; set; }

        public override string ToString()
        {
            return $"{Descricao} - {Programador.Nome} - Ordem: {Ordem}";

        }
    }

}
