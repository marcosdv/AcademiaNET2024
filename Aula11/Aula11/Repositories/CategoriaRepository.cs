using Aula11.Models;
using Aula11.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Aula11.Repositories
{
    public class CategoriaRepository
    {
        private readonly DataContext context;

        public CategoriaRepository(IConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<DataContext>();
            options.UseSqlServer(configuration.GetConnectionString("MyConnection"));

            context = new DataContext(options.Options);
        }

        public Categoria? BuscarPorId(int id)
        {
            return context.Categorias.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}