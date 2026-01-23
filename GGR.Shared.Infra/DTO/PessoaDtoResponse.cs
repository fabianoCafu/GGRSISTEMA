using System.ComponentModel;

namespace GR.Shared.Infra.DTO
{
    public class PessoaDtoResponse
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid Id { get; init; }
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public DateTime DataCriacaoRegistro { get; init; }

        public PessoaDtoResponse()
        {
            DataCriacaoRegistro = DateTime.Now;
        }
    }
}
