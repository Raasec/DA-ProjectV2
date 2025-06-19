using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks
{
    class Basededados : DbContext
    {
        public DbSet<Utilizador> Utilizadores;
        public DbSet<Gestor> Gestors;
        public DbSet<Programador> Programadors;
        public DbSet<Tarefa> Tarefas;
        public DbSet<TipoTarefa> tipoTarefas;
    }
}
