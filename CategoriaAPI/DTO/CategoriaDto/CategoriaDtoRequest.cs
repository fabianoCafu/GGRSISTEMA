using static Shared.Aplication.Enum.Enums;

namespace GR.CategoriaAPI.DTO.Categoria
{
    public class CategoriaDtoRequest
    {
        public string? Descricao { get; set; }

        public FinalidadeCategoria Finalidade { get; set; }
    }
}
