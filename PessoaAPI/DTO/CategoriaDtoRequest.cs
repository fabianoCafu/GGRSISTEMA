using static Shared.Aplication.Enum.Enums;

namespace GR.PessoaAPI.DTO
{
    public class CategoriaDtoRequest
    {
        public string? Descricao { get; set; }
        public FinalidadeCategoria Finalidade { get; set; }
    }
}
