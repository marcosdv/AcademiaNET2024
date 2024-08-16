using Aula04.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aula04.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController : ControllerBase
    {
        [HttpGet]
        public IActionResult BuscarTodas([FromServices] IConfiguration configuration)
        {
            NoticiaRepository repository = new NoticiaRepository(configuration);
            var lista = repository.BuscarTodas();
            return Ok(lista);
        }

        [HttpGet("ComCategoria")]
        public IActionResult BuscarTodosComCategoria([FromServices] IConfiguration configuration)
        {
            NoticiaRepository repository = new NoticiaRepository(configuration);
            var lista = repository.BuscarTodosComCategoria();
            return Ok(lista);
        }
    }
}