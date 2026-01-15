using GR.Shared.Infra.DTO;
using static Shared.Result.ResultMessage;

namespace GR.TransacaoAPI.Service
{
    public interface ITransacaoService
    {
        Task<Result<TransacaoDtoResponse>> CreateAsync(TransacaoDtoRequest transacaoDtoRequest);
        Task<Result<List<TransacaoDtoResponse>>> GetAllAsync(); 
        Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalancePerson();
        Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalanceCategory();
    }
}
