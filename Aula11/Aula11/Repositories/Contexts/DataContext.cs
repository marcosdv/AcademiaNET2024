using Aula11.Models;
using Aula11.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Aula11.Repositories.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Noticia> Noticias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new NoticiaMap());
        }
    }
}