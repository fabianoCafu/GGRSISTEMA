using GR.Shared.Infra.DTO;
using GR.TransacaoAPI.Service;
using Microsoft.AspNetCore.Mvc;
using static Shared.Result.ResultMessage;

namespace GR.TransacaoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransacaoController : Controller
    {
        private readonly ITransacaoService _transacaoService;

        public TransacaoController(ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService ?? throw new ArgumentNullException(nameof(transacaoService));
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result<TransacaoDtoRequest>>> Create([FromBody] TransacaoDtoRequest transacaoDtoRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _transacaoService.CreateAsync(transacaoDtoRequest);

                if (result.IsSuccess)
                {
                    return Created($"api/v1/Transacao/create/{result.Objet!.Id}", result.Objet);
                }

                return BadRequest(new { error = result.Error });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao criar Transacao!");
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<TransacaoDtoResponse>>> GetAll()
        {
            try
            {
                var result = await _transacaoService.GetAllAsync();

                if (result.IsFailure)
                {
                    return BadRequest(new { error = result.Error });
                }

                return Ok(result.Objet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao listar Transações!");
            }
        }

        [HttpGet("getnetbalance")]
        public async Task<IActionResult> GetNetBalance(Guid idPessoa)
        {
            try
            {
                var result = await _transacaoService.GetNetBalance(idPessoa);

                if (result.IsFailure)
                {
                    return BadRequest(new { error = result.Error });
                }

                return Ok(result.Objet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message} ao listar Saldo!");
            }
        }
    }
}
