using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace iTasks
{
    public enum NivelExperiencia
    {
        Junior,
        Senior
    }

    public enum Departamento
    {
        IT,
        Marketing,
        Administracao
    }

    public class Utilizador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Gestor : Utilizador
    {
        public Departamento Departamento { get; set; }

        public bool GereUtilizadores { get; set; }

        public override string ToString()
        {
            return $"{Nome} - {Departamento}";
        }
    }
    public class Programador : Utilizador
    {
        public NivelExperiencia NivelExperiencia { get; set; }
        public Gestor Gestor { get; set; }

        public override string ToString()
        {
            return $"{Nome}";
        }
    }
}
