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
        private readonly string connectionString;

        //configuration -> Objeto que vai vim preenchido do DI - Dependency Injection (Injeção de Dependencias)
        public CategoriaController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("conexao") ?? throw new Exception("ConnectionString sem valor válido");
        }

        [HttpGet]
        public IActionResult GetTodas()
        {
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                using SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM TbCategoria ORDER BY CatId";
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                List<Categoria> listaRetorno = new List<Categoria>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Categoria categoria = new Categoria(reader);
                        listaRetorno.Add(categoria);
                    }

                    return Ok(listaRetorno);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("BuscarPorNome")]
        public IActionResult BuscarPorNome(string nome = "")
        {
            try
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
                //command.Parameters.Add("@Nome", SqlDbType.VarChar, 50).Value = nome;

                SqlDataReader reader = command.ExecuteReader();

                List<Categoria> listaRetorno = new List<Categoria>();

                while (reader.Read())
                {
                    Categoria categoria = new Categoria(reader);
                    listaRetorno.Add(categoria);
                }

                return listaRetorno.Count > 0 ? Ok(listaRetorno) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("BuscarPorId/{id}")]
        public IActionResult BuscarPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("O campo ID deve ser maior que zero"); //StatusCode(400, "O campo ID deve ser maior que zero");
            }

            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                using SqlCommand command = new SqlCommand("SELECT * FROM TbCategoria WHERE CatId = @Codigo", connection);
                command.Parameters.AddWithValue("@Codigo", id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Categoria categoria = new Categoria(reader);

                    return Ok(categoria); //StatusCode(200, categoria);
                }

                return NoContent(); //StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] CategoriaRequest categoria)
        {
            if (string.IsNullOrEmpty(categoria.Nome))
            {
                return BadRequest("Campo Nome deve ser informado!");
            }

            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string sql = "INSERT INTO TbCategoria (CatNome) VALUES (@CatNome)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CatNome", categoria.Nome);

                int numLinhas = command.ExecuteNonQuery();

                return Ok($"Número de linhas afetadas: {numLinhas}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, [FromBody] CategoriaRequest categoria)
        {
            if (string.IsNullOrEmpty(categoria.Nome))
            {
                return BadRequest("Campo Nome deve ser informado!");
            }

            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE TbCategoria SET CatNome = @CatNome WHERE CatId = @CatId";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CatNome", categoria.Nome);
                command.Parameters.AddWithValue("@CatId", codigo);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}