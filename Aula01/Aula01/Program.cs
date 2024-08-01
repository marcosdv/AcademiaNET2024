using System.Data.SqlClient;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        Console.WriteLine("Exemplo do SqlClient");
        ExemploSqlClient();
    }

    private static void ExemploSqlClient()
    {
        //Usuario e Senha -> Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;
        //Autenticacao Windows -> Server=myServerAddress;Database=myDataBase;Integrated Security=true;
        string connectionString = "Server=MDV-NOTE;Database=AcademiaNet;Integrated Security=true;";

        string sql = "SELECT NotId, NotTitulo, NotTexto, NotData, CatId FROM TbNoticia";

        using SqlConnection connection = new SqlConnection(connectionString);

        SqlCommand command = new SqlCommand(sql, connection);

        connection.Open();

        SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine("{0} - {1} - {2}", reader["NotTitulo"], reader["NotTexto"], reader["NotData"]);
            //Console.WriteLine("{0} - {1} - {2}", reader[1], reader[2], reader[3]);
            //Console.WriteLine(reader["NotTitulo"] + " - " + reader["NotTexto"]);
        }

        //Dispose -> Exclui o objeto da memoria
        //Utilizando o "using" no objeto, ele faz o Dispose automaticamente, ao encerrar a execução do metodo
        //connection.Dispose();

        Console.ReadLine();
    }
}