using System.ComponentModel;
using static Shared.Aplication.Enum.Enums;

namespace GR.Shared.Infra.DTO
{
    public class TransacaoDtoResponse
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Id { get; init; }
        public string? Descricao { get; set; }

        public decimal Valor { get; set; }

        public TipoTransacao Tipo { get; set; }

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid PessoaId { get; init; }

        public PessoaDtoResponse? Pessoa { get; set; }

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid CategoriaId { get; init; }

        public CategoriaDtoResponse? Categoria { get; set; }

        public DateTime DataCriacaoRegistro { get; init; }
    }
}
