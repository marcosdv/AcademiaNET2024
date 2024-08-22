using Aula04.Models;
using Aula04.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Aula04.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaRepository repository;

        //configuration -> Objeto que vai vim preenchido do DI - Dependency Injection (Injeção de Dependencias)
        public CategoriaController(IConfiguration configuration)
        {
            repository = new CategoriaRepository(configuration);
        }

        [HttpGet]
        public IActionResult GetTodas()
        {
            try
            {
                IList<Categoria> listaRetorno = repository.BuscarTodas();

                if (listaRetorno.Any()) //if (listaRetorno.Count > 0)
                {
                    return Ok(listaRetorno);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("BuscarPorNome")]
        public IActionResult BuscarPorNome(string nome = "")
        {
            try
            {
                var listaRetorno = repository.BuscarPorNome(nome);
                return listaRetorno.Any() ? Ok(listaRetorno) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                var categoria = repository.BuscarPorId(id);
                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("NoticiaComCategoria")]
        public IActionResult BuscarNoticiaComCategoria(string filtroCategoria = "")
        {
            if (filtroCategoria.Length < 3)
            {
                return BadRequest("O filtro da categoria deve conter pelo menos 3 letras!");
            }

            try
            {
                var lista = repository.BuscarNoticiaComCategoria(filtroCategoria);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                int numLinhas = repository.Adicionar(categoria);
                return Ok($"Número de linhas adicionadas: {numLinhas}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                var cat = new Categoria(codigo, categoria);
                int numLinhas = repository.Alterar(cat);
                return Ok($"Número de linhas alteradas: {numLinhas}");
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
                int numLinhas = repository.Excluir(codigo);
                return Ok($"Número de linhas excluídas: {numLinhas}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}