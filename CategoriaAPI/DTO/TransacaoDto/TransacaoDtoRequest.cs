using static Shared.Aplication.Enum.Enums;

namespace GR.CategoriaAPI.DTO.TransacaoDto
{
    public class TransacaoDtoRequest
    {
        public string? Descricao { get; set; }

        public decimal Valor { get; set; }

        public TipoTransacao Tipo { get; set; }

    }
}
