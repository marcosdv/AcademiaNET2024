using System.Data.SqlClient;

namespace Aula01;

internal class BancoDeDados
{
    //Usuario e Senha -> Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;
    //Autenticacao Windows -> Server=myServerAddress;Database=myDataBase;Integrated Security=true;
    string connectionString = "Server=MDV-NOTE;Database=AcademiaNet;Integrated Security=true;";
    SqlConnection connection;

    public BancoDeDados()
    {
        connection = new SqlConnection(connectionString);
    }

    public SqlConnection abrirConexao()
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        return connection;
    }
}