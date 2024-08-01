using System.Data.SqlClient;

namespace Aula01;

internal class Noticias
{
    BancoDeDados bancoDeDados;

    public Noticias()
    {
        bancoDeDados = new BancoDeDados();
    }

    private void ExibirResultado(SqlCommand command)
    {
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine("{0} - {1} - {2}", reader["NotTitulo"], reader["NotTexto"], reader["NotData"]);
            //Console.WriteLine("{0} - {1} - {2}", reader[1], reader[2], reader[3]);
            //Console.WriteLine(reader["NotTitulo"] + " - " + reader["NotTexto"]);
        }
    }

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
}