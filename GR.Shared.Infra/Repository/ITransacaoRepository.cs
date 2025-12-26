using GR.Shared.Infra.Model;
using static Shared.Result.ResultMessage;

namespace GR.Shared.Infra.Repository
{
    public interface ITransacaoRepository
    {
        Task<Result<Transacao>> CreateAsync(Transacao transacao);

        Task<Result<List<Transacao>>> GetAllAsync();
    }
}
