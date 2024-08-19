using Aula04.Models;
using Dapper;
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

        public Noticia? BuscarPorId(int codigo)
        {
            string sql = @"
                SELECT N.*, C.CatNome
                FROM TbNoticia N
                INNER JOIN TbCategoria C ON C.CatId = N.CatId
                WHERE N.NotId = @NotId
            ";

            var parametros = new { NotId = codigo };

            var lista = connection.Query<Noticia, Categoria, Noticia>(sql,
                (noticia, categoria) => {
                    noticia.Categoria = categoria;
                    return noticia;
                },
                splitOn: "CatId",
                param: parametros
            );

            return lista.FirstOrDefault();
        }

        public IEnumerable<Noticia> BuscarPorCategoria(int codigo)
        {
            string sql = @"
                SELECT N.*, C.CatNome
                FROM TbNoticia N
                INNER JOIN TbCategoria C ON C.CatId = N.CatId
                WHERE C.CatId = @CatId
            ";

            return connection.Query<Noticia, Categoria, Noticia>(sql,
                (noticia, categoria) => {
                    noticia.Categoria = categoria;
                    return noticia;
                },
                splitOn: "CatId",
                param: new { CatId = codigo }
            );
        }
    }
}