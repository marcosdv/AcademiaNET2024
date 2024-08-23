using Aula11.Models;
using Aula11.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Aula11.Repositories
{
    public class NoticiaRepository
    {
        private readonly DataContext context;

        public NoticiaRepository(IConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<DataContext>();
            options.UseSqlServer(configuration.GetConnectionString("MyConnection"));

            context = new DataContext(options.Options);
        }

        public IEnumerable<Noticia> BuscarTodas()
        {
            return context.Noticias;
        }

        public Noticia? BuscarPorId(int id)
        {
            /*
            return (
                from noticia in context.Noticias
                where noticia.Id == id
                select noticia
            ).FirstOrDefault();
            */

            return context.Noticias.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}