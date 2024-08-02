using System.Data.SqlClient;

namespace Aula01;

internal class Noticias
{
    BancoDeDados bancoDeDados;

    #region | Construtores |

    public Noticias()
    {
        bancoDeDados = new BancoDeDados();
    }

    #endregion

    #region | Metodos Basicos |

    private void ExibirResultado(SqlCommand command)
    {
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine("Id: {0}\t{1}\t{2}\tCategoria:{3}\n{4}",
                reader["NotId"], reader["NotTitulo"], reader["NotData"], reader["CatId"], reader["NotTexto"]);
            
            Console.WriteLine("");
            
            //Console.WriteLine("{0} - {1} - {2}", reader[1], reader[2], reader[3]);
            //Console.WriteLine(reader["NotTitulo"] + " - " + reader["NotTexto"]);
        }
    }

    #endregion

    #region | Metodos de Busca |

    public void BuscarPorData(DateTime filtroData)
    {
        string sql =
            @"SELECT NotId, NotTitulo, NotTexto, NotData, CatId
              FROM TbNoticia
              WHERE NotData >= @FiltroData
              ORDER BY NotData DESC";

        using SqlCommand command = new SqlCommand(sql, bancoDeDados.abrirConexao());
        command.Parameters.AddWithValue("@FiltroData", filtroData);

        ExibirResultado(command);
    }

    public void BuscarPorTitulo(string titulo)
    {
        string sql =
            @"SELECT NotId, NotTitulo, NotTexto, NotData, CatId
              FROM TbNoticia
              WHERE UPPER(NotTitulo) LIKE UPPER('%' + @Titulo + '%')
              ORDER BY NotData DESC";

        using SqlCommand command = new SqlCommand(sql, bancoDeDados.abrirConexao());
        command.Parameters.AddWithValue("@Titulo", titulo);

        ExibirResultado(command);
    }

    #endregion

    #region | Manipulacao Dados |

    public void Inserir(string titulo, string texto, int categoria)
    {
        string sql =
            @"INSERT INTO TbNoticia (NotTitulo, NotTexto, NotData, CatId)
                             VALUES (@NotTitulo, @NotTexto, @NotData, @CatId)";

        SqlCommand command = new SqlCommand(sql, bancoDeDados.abrirConexao());

        try
        {
            command.Parameters.AddWithValue("@NotTitulo", titulo);
            command.Parameters.AddWithValue("@NotTexto", texto);
            command.Parameters.AddWithValue("@NotData", DateTime.Now);
            command.Parameters.AddWithValue("@CatId", categoria);

            int numLinhas = command.ExecuteNonQuery();
            Console.WriteLine("Número de linhas afetadas: " + numLinhas);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Erro ao inserir!\n" + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao inserir!\n" + ex.Message);
        }
        finally
        {
            command.Dispose();
        }
    }

    public void Alterar(int codigo, string titulo, string texto, int categoria)
    {
        string sql =
            @"UPDATE TbNoticia SET
                NotTitulo = @NotTitulo,
                NotTexto = @NotTexto,
                CatId = @CatId
              WHERE NotId = @NotId";

        try
        {
            using SqlCommand command = new SqlCommand(sql, bancoDeDados.abrirConexao());
            command.Parameters.AddWithValue("@NotTitulo", titulo);
            command.Parameters.AddWithValue("@NotTexto", texto);
            command.Parameters.AddWithValue("@CatId", categoria);
            command.Parameters.AddWithValue("@NotId", codigo);

            int numLinhas = command.ExecuteNonQuery();
            Console.WriteLine("Número de linhas afetadas: " + numLinhas);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Erro ao alterar!\n" + ex.Message);
        }
    }

    public void Excluir(int codigo)
    {
        string sql = "DELETE FROM TbNoticia WHERE NotId = @NotId";

        try
        {
            using SqlCommand command = new SqlCommand(sql, bancoDeDados.abrirConexao());
            command.Parameters.AddWithValue("NotId", codigo);

            int numLinhas = command.ExecuteNonQuery();
            Console.WriteLine("Número de linhas afetadas: " + numLinhas);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Erro ao excluir!\n" + ex.Message);
        }
    }

    #endregion
}