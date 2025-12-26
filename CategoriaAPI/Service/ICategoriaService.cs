using GR.Shared.Infra.DTO;
using static Shared.Result.ResultMessage;

namespace GR.CategoriaAPI.Service
{
    public interface ICategoriaService
    {
        Task<Result<CategoriaDtoResponse>> CreateAsync(CategoriaDtoRequest userDTO);
        Task<Result<List<CategoriaDtoResponse>>> GetByDescription(string descricaoCategoria);
        Task<Result<List<CategoriaDtoResponse>>> GetAllAsync(); 
    }
}
