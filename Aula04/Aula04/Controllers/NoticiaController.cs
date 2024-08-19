using Aula04.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Aula04.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController : ControllerBase
    {
        private readonly NoticiaRepository repository;

        public NoticiaController(IConfiguration configuration)
        {
            repository = new NoticiaRepository(configuration);
        }

        [HttpGet]
        //public IActionResult BuscarTodas([FromServices] IConfiguration configuration)
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

        [HttpGet("ComCategoria")]
        public IActionResult BuscarTodosComCategoria()
        {
            try
            {
                var lista = repository.BuscarTodosComCategoria();
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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Categoria/{codigo}")]
        public IActionResult BuscarPorCategoria(int codigo)
        {
            try
            {
                var lista = repository.BuscarPorCategoria(codigo);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}