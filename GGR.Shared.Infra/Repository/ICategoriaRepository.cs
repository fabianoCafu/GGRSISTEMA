using GGR.Shared.Infra.Model;
using static Shared.Result.ResultMessage;

namespace GGR.Shared.Infra.Repository
{
    public interface ICategoriaRepository
    {
        Task<Result<Categoria>> CreateAsync(Categoria categoria);

        Task<Result<Categoria>> GetCategoriaByIdAsync(Guid categoriaId);

        Task<Result<List<Categoria>>> GetByDescription(string descricaoCategoria);

        Task<Result<List<Categoria>>> GetAllAsync();
    }
}
