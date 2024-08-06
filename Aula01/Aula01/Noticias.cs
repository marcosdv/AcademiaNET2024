using System.Data;
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

        try
        {
            using SqlCommand command = new SqlCommand(sql, bancoDeDados.abrirConexao());
            command.Parameters.AddWithValue("@FiltroData", filtroData);

            ExibirResultado(command);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao buscar por data: " + ex.Message);
        }
        finally
        {
            bancoDeDados.fecharConexao();
        }
    }

    public void BuscarPorTitulo(string titulo)
    {
        string sql =
            @"SELECT NotId, NotTitulo, NotTexto, NotData, CatId
              FROM TbNoticia
              WHERE UPPER(NotTitulo) LIKE UPPER('%' + @Titulo + '%')
              ORDER BY NotData DESC";

        try
        {
            using SqlCommand command = new SqlCommand(sql, bancoDeDados.abrirConexao());
            command.Parameters.AddWithValue("@Titulo", titulo);

            ExibirResultado(command);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao buscar por título: " + ex.Message);
        }
        finally
        {
            bancoDeDados.fecharConexao();
        }
    }

    public void BuscarDadosViewPorCategoria(string categoria)
    {
        string sql = 
            @"SELECT * FROM VwNoticia
              WHERE UPPER(CatNome) LIKE ('%' + @Categoria + '%')";

        try
        {
            using SqlCommand command = new SqlCommand(sql, bancoDeDados.abrirConexao());
            command.Parameters.AddWithValue("@Categoria", categoria);

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("{0}\t{1}\t{2}\n{3}\n",
                    reader["CatNome"], reader["NotTitulo"], reader["NotData"], reader["NotTexto"]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao buscar na View: " + ex.Message);
        }
        finally
        {
            bancoDeDados.fecharConexao();
        }
    }

    public void CriarDataSet()
    {
        DataSet dataSet = new DataSet("Noticias");

        string sqlNoticia = "SELECT * FROM TbNoticia";
        using SqlCommand commandNoticia = new SqlCommand(sqlNoticia, bancoDeDados.abrirConexao());

        using SqlDataAdapter adapterNoticia = new SqlDataAdapter();
        adapterNoticia.TableMappings.Add("TbNoticia", "TbNoticia");
        adapterNoticia.SelectCommand = commandNoticia;
        adapterNoticia.Fill(dataSet, "TbNoticia");

        bancoDeDados.fecharConexao();

        foreach (DataRow linha in dataSet.Tables["TbNoticia"].Rows)
        {
            foreach (DataColumn coluna in dataSet.Tables["TbNoticia"].Columns) {
                Console.Write("{0}\t", linha[coluna]);
            }
            Console.WriteLine("\n");
        }
    }

    public void CriarDataSetComCategoria()
    {
        SqlConnection connection = bancoDeDados.abrirConexao();

        DataSet dataSet = new DataSet("Noticias");

        string sqlNoticia = "SELECT * FROM TbNoticia";
        using SqlCommand commandNoticia = new SqlCommand(sqlNoticia, connection);

        using SqlDataAdapter adapterNoticia = new SqlDataAdapter();
        adapterNoticia.TableMappings.Add("TbNoticia", "TbNoticia");
        adapterNoticia.SelectCommand = commandNoticia;
        adapterNoticia.Fill(dataSet, "TbNoticia");

        string sqlCategoria = "SELECT * FROM TbCategoria";
        using SqlCommand commandCategoria = new SqlCommand(sqlCategoria, connection);

        using SqlDataAdapter adapterCategoria = new SqlDataAdapter();
        adapterCategoria.TableMappings.Add("TbCategoria", "TbCategoria");
        adapterCategoria.SelectCommand = commandCategoria;
        adapterCategoria.Fill(dataSet, "TbCategoria");

        bancoDeDados.fecharConexao();

        DataColumn colunaPai = dataSet.Tables["TbCategoria"].Columns["CatId"];
        DataColumn colunaFilho = dataSet.Tables["TbNoticia"].Columns["CatId"];

        DataRelation relacionamento = new DataRelation("Categoria_Notica", colunaPai, colunaFilho);

        dataSet.Relations.Add(relacionamento);

        foreach (DataRow linhaCategoria in dataSet.Tables["TbCategoria"].Rows)
        {
            Console.WriteLine(linhaCategoria["CatNome"]);

            foreach (DataRow linhaNoticia in linhaCategoria.GetChildRows(relacionamento))
            {
                Console.WriteLine("\t{0}", linhaNoticia["NotTitulo"]);
            }
        }
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
            bancoDeDados.fecharConexao();
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
        finally
        {
            bancoDeDados.fecharConexao();
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
        finally
        {
            bancoDeDados.fecharConexao();
        }
    }

    #endregion
}