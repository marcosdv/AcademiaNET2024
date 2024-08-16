using Aula04.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Aula04.Repositories
{
    public class NoticiaRepository
    {
        private readonly SqlConnection connection;

        public NoticiaRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("conexao") ?? throw new Exception("ConnectionString sem valor válido");
            connection = new SqlConnection(connectionString);
        }

        public IList<Noticia> BuscarTodas()
        {
            
        }
    }
}