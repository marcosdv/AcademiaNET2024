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
            //AsNoTracking -> Desativa/Não Faz o reastreamento do EntityFramework
            return context.Noticias.Include(x => x.Categoria).AsNoTracking();
        }

        public IEnumerable<Noticia> BuscarPorCategoria(int codigo)
        {
            /*
            var lista = (
                from noticia in context.Noticias
                join categoria in context.Categorias on noticia.Categoria.Id equals categoria.Id
                where noticia.Categoria.Id == codigo
                select new { noticia, categoria }
            ).AsNoTracking();
            
            return (
                from noticia in context.Noticias
                where noticia.Categoria.Id == codigo
                select noticia
            ).Include(x => x.Categoria).AsNoTracking();
            */

            return context.Noticias.Include(x => x.Categoria).Where(x => x.Categoria.Id == codigo).AsNoTracking();
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

            return context.Noticias.Include(x => x.Categoria).Where(x => x.Id == id).FirstOrDefault();
        }

        public int Adicionar(Noticia noticia)
        {
            try
            {
                context.Noticias.Add(noticia);
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Alterar(Noticia noticia)
        {
            context.Entry(noticia).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public int Excluir(int id)
        {
            Noticia? noticia = BuscarPorId(id);
            if (noticia != null)
            {
                context.Remove(noticia);
                return context.SaveChanges();
            }
            return 0;
        }
    }
}