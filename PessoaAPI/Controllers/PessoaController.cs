using GR.PessoaAPI.Service;
using GR.Shared.Infra.DTO;
using Microsoft.AspNetCore.Mvc;
using static Shared.Result.ResultMessage;

namespace GR.PessoaAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PessoaController : Controller
    {
        private readonly IPessoaService _pessoaService;

        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService ?? throw new ArgumentNullException(nameof(pessoaService));
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result<PessoaDtoRequest>>> Create([FromBody] PessoaDtoRequest pessoaDtoRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _pessoaService.CreateAsync(pessoaDtoRequest);

                if (result.IsSuccess)
                {
                    return Created($"api/v1/Pessoa/create/{result.Objet!.Id}", result.Objet);
                }

                return BadRequest(new { error = result.Error });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao criar Pessoa!");
            }
        }

        [HttpDelete("delete/{idPessoa}")]
        public async Task<ActionResult<PessoaDtoRequest>> Delete(Guid idPessoa)
        {
            try
            {
                var result = await _pessoaService.DeleteAsync(idPessoa);

                if (result.IsFailure)
                {
                    return BadRequest(new { error = result.Error });
                }

                return Ok(new { Message = "Pessoa removida com sucesso!" });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao deletar Pessoa!");
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<PessoaDtoResponse>>> GetAll()
        {
            try
            {
                var result = await _pessoaService.GetAllAsync();

                if (result.IsFailure)
                {
                    return BadRequest(new { error = result.Error });
                }

                return Ok(result.Objet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao listar Pessoas!");
            }
        }

        [HttpGet("getbyname")]
        public async Task<ActionResult<IEnumerable<PessoaDtoResponse>>> GetByName(string nomePessoa)
        {
            try
            {
                var result = await _pessoaService.GetByName(nomePessoa);

                if (result.IsFailure)
                {
                    return BadRequest(new { error = result.Error });
                }

                return Ok(result.Objet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao buscar Pessoa pelo Nome!");
            }
        }
    }
}
