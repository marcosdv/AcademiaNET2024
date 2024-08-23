using Aula11.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Aula11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController : ControllerBase
    {
        private readonly NoticiaRepository repository;

        public NoticiaController(IConfiguration config)
        {
            repository = new NoticiaRepository(config);
        }

        [HttpGet]
        public IActionResult BuscarTodas()
        {
            try
            {
                var lista = repository.BuscarTodas();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{codigo}")]
        public IActionResult BuscarPorId(int codigo)
        {
            try
            {
                var noticia = repository.BuscarPorId(codigo);
                return Ok(noticia);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}