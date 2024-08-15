using Aula04.Models;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

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

        public IList<NoticiaCategoriaResponse> BuscarNoticiaComCategoria()
        {
            try
            {
                connection.Open();

                using SqlCommand command = new SqlCommand("ProcBuscarNoticias", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                List<NoticiaCategoriaResponse> lista = new List<NoticiaCategoriaResponse>();

                while(reader.Read())
                {
                    NoticiaCategoriaResponse response = new NoticiaCategoriaResponse(reader);
                    lista.Add(response);
                }

                return lista;
            }
            finally
            {
                connection.Close();
            }
        }

        public int Adicionar(CategoriaRequest categoria)
        {
            try
            {
                connection.Open();

                string sql = "INSERT INTO TbCategoria (CatNome) VALUES (@CatNome)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CatNome", categoria.Nome);

                return command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        /* Alterar com BeginTransaction
        public int Alterar(Categoria categoria)
        {
            var catOriginal = BuscarPorId(categoria.CatId);

            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            
            try
            {
                string sql = "UPDATE TbCategoria SET CatNome = @CatNome WHERE CatId = @CatId";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Transaction = transaction;
                command.Parameters.AddWithValue("@CatNome", categoria.CatNome);
                command.Parameters.AddWithValue("@CatId", categoria.CatId);
                int numLinhas = command.ExecuteNonQuery();

                string sqlLog = @"INSERT INTO TbLogCategoria (LogNomeOriginal, LogNomeNovo, CatId) VALUES (@LogNomeOriginal, @LogNomeNovo, @CatId)";
                using SqlCommand commandLog = new SqlCommand(sqlLog, connection);
                commandLog.Transaction = transaction;
                commandLog.Parameters.AddWithValue("@LogNomeOriginal", catOriginal?.CatNome ?? "");
                commandLog.Parameters.AddWithValue("@LogNomeNovo", categoria.CatNome);
                commandLog.Parameters.AddWithValue("@CatId", categoria.CatId);
                commandLog.ExecuteNonQuery();

                transaction.Commit();

                return numLinhas;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        */

        public int Alterar(Categoria categoria)
        {
            try
            {
                var catOriginal = BuscarPorId(categoria.CatId);

                using TransactionScope transaction = new TransactionScope();

                connection.Open();

                string sql = "UPDATE TbCategoria SET CatNome = @CatNome WHERE CatId = @CatId";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CatNome", categoria.CatNome);
                command.Parameters.AddWithValue("@CatId", categoria.CatId);
                int numLinhas = command.ExecuteNonQuery();

                string sqlLog = @"INSERT INTO TbLogCategoria (LogNomeOriginal, LogNomeNovo, CatId)
                                                    VALUES (@LogNomeOriginal, @LogNomeNovo, @CatId)";
                using SqlCommand commandLog = new SqlCommand(sqlLog, connection);
                commandLog.Parameters.AddWithValue("@LogNomeOriginal", catOriginal?.CatNome ?? "");
                commandLog.Parameters.AddWithValue("@LogNomeNovo", categoria.CatNome);
                commandLog.Parameters.AddWithValue("@CatId", categoria.CatId);
                commandLog.ExecuteNonQuery();

                transaction.Complete();

                return numLinhas;
            }
            finally
            {
                connection.Close();
            }
        }

        public int Excluir(int catId)
        {
            try
            {
                connection.Open();

                string sql = "DELETE FROM TbCategoria WHERE CatId = @CatId";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CatId", catId);

                return command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
