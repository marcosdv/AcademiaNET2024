using System.Data.SqlClient;

namespace Aula01;

internal class BancoDeDados
{
    //Usuario e Senha -> Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;
    //Autenticacao Windows -> Server=myServerAddress;Database=myDataBase;Integrated Security=true;
    string connectionString = "Server=MDV-NOTE;Database=AcademiaNet;Integrated Security=true;";
    SqlConnection connection;

    public SqlConnection abrirConexao()
    {
        connection = new SqlConnection(connectionString);
        connection.Open();

        return connection;
    }

    public void fecharConexao()
    {
        connection.Close();
        connection.Dispose();
    }
}