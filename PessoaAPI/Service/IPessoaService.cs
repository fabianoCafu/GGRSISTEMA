using GGR.Shared.Infra.DTO;
using static Shared.Result.ResultMessage;

namespace GGR.PessoaAPI.Service
{
    public interface IPessoaService
    {
        Task<Result<PessoaDtoResponse>> CreateAsync(PessoaDtoRequest pessoaDtoRequest);
        Task<Result<bool>> DeleteAsync(Guid idPessoa);
        Task<Result<List<PessoaDtoResponse>>> GetByName(string nomePessoa); 
        Task<Result<List<PessoaDtoResponse>>> GetAllAsync();
    }
}
