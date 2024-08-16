using System.Data;
using System.Data.SqlClient;

namespace Aula04.Models
{
    public class NoticiaCategoriaResponse
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public DateTime Data { get; set; }
        public int CodigoCategoria { get; set; }
        public string Categoria { get; set; }

        public NoticiaCategoriaResponse(SqlDataReader reader)
        {
            Codigo = reader.GetInt32("NotId");
            Titulo = reader.GetString("NotTitulo");
            Texto = reader.GetString("NotTexto");
            Data = reader.GetDateTime("NotData");
            CodigoCategoria = reader.GetInt32("CatId");
            Categoria = reader.GetString("CatNome");
        }
    }
}