using static Shared.Aplication.Enum.Enums;

namespace GR.PessoaAPI.DTO
{
    public class CategoriaDtoResponse
    {
        public Guid Id { get; init; }
        public string? Descricao { get; set; }
        public FinalidadeCategoria Finalidade { get; set; }
    }
}
