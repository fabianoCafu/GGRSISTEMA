using System.ComponentModel;
using static Shared.Aplication.Enum.Enums;

namespace GGR.Shared.Infra.DTO
{
    public class CategoriaDtoResponse
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Id { get; init; }
        public string? Descricao { get; set; }
        public FinalidadeCategoria Finalidade { get; set; }
        public DateTime DataCriacaoRegistro { get; init; }
    }
}
