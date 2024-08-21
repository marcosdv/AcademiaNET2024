using System.Data;
using System.Data.SqlClient;

namespace Aula04.Models
{
    public class Categoria
    {
        public int CatId { get; set; }
        public string CatNome { get; set; }

        public Categoria() { }

        public Categoria(SqlDataReader reader)
        {
            CatId = reader.GetInt32("CatId");
            CatNome = reader.GetString("CatNome");
        }

        public Categoria(int codigo, CategoriaRequest request)
        {
            CatId = codigo;
            CatNome = request.Nome;
        }
    }
}