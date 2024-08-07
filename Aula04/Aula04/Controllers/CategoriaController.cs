using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Aula04.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        string connectionString = "Server=MDV-NOTE;Database=AcademiaNet;Trusted_Connection=True;";

        [HttpGet]
        public List<string> GetTodas()
        {
            using SqlConnection connection = new SqlConnection(connectionString);

        }
    }
}