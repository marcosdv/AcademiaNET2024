using Aula04.Models;
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

        [HttpGet("ComCategoria/Filtro")]
        public IActionResult BuscarTodosComCategoriaFiltro(string? titulo, int? categoria)
        {
            try
            {
                var lista = repository.BuscarTodosComCategoria();

                var listaLinq =
                    from noticia in lista
                    //where noticia.NotTitulo.Contains(titulo, StringComparison.OrdinalIgnoreCase)
                    where noticia.Categoria.CatId == categoria
                    orderby noticia.NotTitulo descending, noticia.Categoria.CatId
                    select noticia;

                var listaDados = lista
                    .Where(x => x.Categoria.CatId == categoria)
                    .OrderByDescending(x => x.NotData)
                    .OrderBy(x => x.Categoria.CatId);

                return Ok(listaLinq);
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

        [HttpPost]
        public IActionResult Adicionar(NoticiaRequest noticia)
        {
            try
            {
                var numLinhas = repository.Adicionar(noticia);
                return Ok(numLinhas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, NoticiaRequest request)
        {
            try
            {
                //var categoria = new Categoria { CatId = request.CodCategoria };
                var categoria = new Categoria();
                categoria.CatId = request.CodCategoria;

                var noticia = new Noticia();
                noticia.NotId = codigo;
                noticia.NotTitulo = request.Titulo;
                noticia.NotTexto = request.Texto;
                noticia.Categoria = categoria;

                var numLinhas = repository.Alterar(noticia);
                return Ok(numLinhas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{codigo}")]
        public IActionResult Excluir(int codigo)
        {
            try
            {
                var numLinhas = repository.Excluir(codigo);
                return Ok(numLinhas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}