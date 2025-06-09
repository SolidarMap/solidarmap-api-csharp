using Microsoft.EntityFrameworkCore;
using SolidarMap.Models;

namespace SolidarMap.Connection
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ajuda> Ajudas { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<TipoRecurso> TiposRecurso { get; set; }
        public DbSet<TipoZona> TiposZona { get; set; }
        public DbSet<Localizacao> Localizacoes { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }


    }
}
