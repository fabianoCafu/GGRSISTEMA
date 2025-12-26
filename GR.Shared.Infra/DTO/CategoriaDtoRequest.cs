using System.ComponentModel.DataAnnotations;
using static Shared.Aplication.Enum.Enums;

namespace GR.Shared.Infra.DTO
{
    public class CategoriaDtoRequest
    {
        [Required]
        public string? Descricao { get; set; }

        [Required]

        public FinalidadeCategoria? Finalidade { get; set; } = null;
    }
}
