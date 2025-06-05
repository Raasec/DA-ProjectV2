using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace iTasks
{
    internal class Utilizador
    {
        public int Id;
        public string Nome;
        public string Username;
        public string Password;   // ter cuidado com isto
        public string Tipo;   // Programador ou Gestor -> Fazer herança
    }
}
