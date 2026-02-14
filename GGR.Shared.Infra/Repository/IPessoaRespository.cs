using GGR.Shared.Infra.Model;
using static Shared.Result.ResultMessage;

namespace GGR.Shared.Infra.Repository
{
    public interface IPessoaRespository
    {
        Task<Result<Pessoa>> CreateAsync(Pessoa pessoa);
        Task<Result<bool>> DeleteAsync(Guid idPessoa);
        Task<Result<Pessoa>> GetPessoaByIdAsync(Guid pessoaId);
        Task<Result<List<Pessoa>>> GetByName(string nomePessoa); 
        Task<Result<List<Pessoa>>> GetAllAsync();
    }
}
