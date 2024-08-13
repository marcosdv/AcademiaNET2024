using Aula04.Models;
using System.Data;
using System.Data.SqlClient;

namespace Aula04.Repositories
{
    public class CategoriaRepository
    {
        private readonly SqlConnection connection;

        public CategoriaRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("conexao") ?? throw new Exception("ConnectionString sem valor válido");
            connection = new SqlConnection(connectionString);
        }

        public IList<Categoria> BuscarTodas()
        {
            try
            {
                connection.Open();

                using SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM TbCategoria ORDER BY CatId";
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                List<Categoria> listaRetorno = new List<Categoria>();

                while (reader.Read())
                {
                    Categoria categoria = new Categoria(reader);
                    listaRetorno.Add(categoria);
                }

                return listaRetorno;
            }
            finally
            {
                connection.Close();
            }
        }

        public IList<Categoria> BuscarPorNome(string nome)
        {
            try
            {
                connection.Open();

                using SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText =
                    @"SELECT * FROM TbCategoria
                  WHERE UPPER(CatNome) LIKE UPPER('%' + @Nome + '%')
                  ORDER BY CatId";

                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Nome", nome); //command.Parameters.Add("@Nome", SqlDbType.VarChar, 50).Value = nome;

                SqlDataReader reader = command.ExecuteReader();

                List<Categoria> listaRetorno = new List<Categoria>();

                while (reader.Read())
                {
                    Categoria categoria = new Categoria(reader);
                    listaRetorno.Add(categoria);
                }

                return listaRetorno;
            }
            finally
            {
                connection.Close();
            }
        }

        public Categoria? BuscarPorId(int id)
        {
            try
            {
                connection.Open();

                using SqlCommand command = new SqlCommand("SELECT * FROM TbCategoria WHERE CatId = @Codigo", connection);
                command.Parameters.AddWithValue("@Codigo", id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    return new Categoria(reader);
                }

                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
