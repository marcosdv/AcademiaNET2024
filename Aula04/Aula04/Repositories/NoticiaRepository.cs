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

        #region | Metodos de Busca |

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

        #endregion

        #region | Manipulacao Dados |

        public int Adicionar(NoticiaRequest request)
        {
            string sql = @"
                INSERT INTO TbNoticia (NotTitulo, NotTexto, NotData, CatId) 
                               VALUES (@NotTitulo, @NotTexto, @NotData, @CatId)";

            var parametros = new
            {
                NotTitulo = request.Titulo,
                NotTexto = request.Texto,
                NotData = DateTime.Now,
                CatId = request.CodCategoria
            };

            return connection.Execute(sql, parametros);
        }

        public int Alterar(Noticia noticia)
        {
            string sql = @"
                UPDATE TbNoticia SET
                    NotTitulo = @NotTitulo,
                    NotTexto = @NotTexto,
                    NotData = @NotData,
                    CatId = @CatId
                WHERE NotId = @NotId";

            var parametros = new
            {
                NotTitulo = noticia.NotTitulo,
                NotTexto = noticia.NotTexto,
                NotData = DateTime.Now,
                CatId = noticia.Categoria.CatId,
                NotId = noticia.NotId
            };

            return connection.Execute(sql, parametros);
        }

        public int Excluir(int codigo)
        {
            /*
            string sql = "DELETE FROM TbNoticia WHERE NotId = @NotId";
            var parametros = new { NotId = codigo };
            return connection.Execute(sql, parametros);
            */
            return connection.Execute("DELETE FROM TbNoticia WHERE NotId = @NotId", new { NotId = codigo });
        }

        #endregion
    }
}