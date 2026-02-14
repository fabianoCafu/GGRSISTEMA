using System.ComponentModel.DataAnnotations;

namespace GGR.Shared.Infra.DTO
{
    public class PessoaDtoRequest
    {
        [Required]
        public string? Nome { get; set; }
        [Required]
        public int Idade { get; set; }
    }
}
