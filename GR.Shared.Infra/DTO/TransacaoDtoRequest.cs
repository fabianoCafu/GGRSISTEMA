using System.ComponentModel;
using static Shared.Aplication.Enum.Enums;

namespace GR.Shared.Infra.DTO
{
    public class TransacaoDtoRequest
    {
        public decimal Valor { get; set; }

        [DefaultValue(0)]
        public TipoTransacao Tipo { get; set; }

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid PessoaId { get; init; } 

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid CategoriaId { get; init; }
    }
}
