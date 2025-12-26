using GR.CategoriaAPI.Service;
using GR.Shared.Infra.DTO;
using Microsoft.AspNetCore.Mvc;
using static Shared.Result.ResultMessage;

namespace GR.CategoriaAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result<CategoriaDtoRequest>>> Create([FromBody] CategoriaDtoRequest categoriaDtoRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _categoriaService.CreateAsync(categoriaDtoRequest);

                if (result.IsSuccess)
                {
                    return Created($"api/v1/Categoria/create/{result.Objet!.Id}", result.Objet);
                }

                return BadRequest(new { error = result.Error });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao criar Categoria!");
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<CategoriaDtoResponse>>> GetAll()
        {
            try
            {
                var result = await _categoriaService.GetAllAsync();

                if (result.IsFailure)
                {
                    return BadRequest(new { error = result.Error });
                }

                return Ok(result.Objet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao listar Categorias!");
            }
        }

        [HttpGet("getbydescription")]
        public async Task<ActionResult<IEnumerable<PessoaDtoResponse>>> GetByDescription(string descricaCategoria)
        {
            try
            {
                var result = await _categoriaService.GetByDescription(descricaCategoria);

                if (result.IsFailure)
                {
                    return BadRequest(new { error = result.Error });
                }

                return Ok(result.Objet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao listar Categorias!");
            }
        }
    }
}
