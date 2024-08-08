using Aula04.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Aula04.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        string connectionString = "Server=MDV-NOTE;Database=AcademiaNet;Trusted_Connection=True;";

        [HttpGet]
        public List<Categoria> GetTodas()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM TbCategoria ORDER BY CatId";
            command.CommandType = CommandType.Text;

            SqlDataReader reader = command.ExecuteReader();

            List<Categoria> listaRetorno = new List<Categoria>();

            while (reader.Read())
            {
                /*
                if (reader["CatNome"] != null)
                {
                    listaRetorno.Add(reader["CatNome"].ToString());
                }
                else
                {
                    listaRetorno.Add("");
                }

                listaRetorno.Add(reader["CatNome"].ToString() ?? "");
                */

                Categoria categoria = new Categoria();
                categoria.CatId = reader.GetInt32("CatId");
                categoria.CatNome = reader.GetString("CatNome");

                listaRetorno.Add(categoria);
            }

            return listaRetorno;
        }

        [HttpGet("BuscarPorNome")]
        public List<Categoria> BuscarPorNome(string nome)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText =
                @"SELECT * FROM TbCategoria
                  WHERE UPPER(CatNome) LIKE UPPER('%' + @Nome + '%')
                  ORDER BY CatId";

            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@Nome", nome);
            
            SqlDataReader reader = command.ExecuteReader();

            List<Categoria> listaRetorno = new List<Categoria>();

            while (reader.Read())
            {
                /*
                Categoria categoria = new Categoria
                {
                    CatId = reader.GetInt32("CatId"),
                    CatNome = reader.GetString("CatNome")
                };                 
                */

                Categoria categoria = new Categoria();
                categoria.CatId = reader.GetInt32("CatId");
                categoria.CatNome = reader.GetString("CatNome");

                listaRetorno.Add(categoria);
            }

            return listaRetorno;
        }
    }
}