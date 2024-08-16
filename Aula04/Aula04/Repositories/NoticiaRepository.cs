using Aula04.Models;
using Dapper;
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

        public IEnumerable<Noticia> BuscarTodas()
        {
            return connection.Query<Noticia>("SELECT * FROM TbNoticia");
        }

        public IEnumerable<Noticia> BuscarTodosComCategoria()
        {
            string sql = "SELECT N.*, C.CatNome FROM TbNoticia N INNER JOIN TbCategoria C ON C.CatId = N.CatId";

            return connection.Query<Noticia, Categoria, Noticia>(sql,
                (noticia, categoria) => {
                    noticia.Categoria = categoria;
                    return noticia;
                },
                splitOn: "CatId"
            );
        }
    }
}