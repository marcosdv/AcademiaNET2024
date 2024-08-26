using Aula11.Models;
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

        [HttpGet("Categoria/{id}")]
        public IActionResult BuscarPorCategoria(int id)
        {
            try
            {
                var lista = repository.BuscarPorCategoria(id);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Adicionar(NoticiaRequest request)
        {
            try
            {
                Noticia noticia = new Noticia(request);
                int numLinhas = repository.Adicionar(noticia);
                return Ok(numLinhas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Alterar(int id, NoticiaRequest request)
        {
            try
            {
                Noticia noticia = new Noticia(request);
                noticia.Id = id;

                int numLinhas = repository.Alterar(noticia);
                return Ok(numLinhas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Apagar(int id)
        {
            try
            {
                int numLinhas = repository.Excluir(id);
                return Ok(numLinhas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}