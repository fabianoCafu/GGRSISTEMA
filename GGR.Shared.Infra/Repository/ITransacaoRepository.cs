using GGR.Shared.Infra.DTO;
using GGR.Shared.Infra.Model;
using static Shared.Result.ResultMessage;

namespace GGR.Shared.Infra.Repository
{
    public interface ITransacaoRepository
    {
        Task<Result<Transacao>> CreateAsync(Transacao transacao);
        Task<Result<List<Transacao>>> GetAllAsync();
        Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalancePerson();
        Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalanceCategory();
    }
}
