namespace GR.CategoriaAPI.DTO.PessoaDto
{
    public class PessoaDtoResponse
    {
        public Guid Id { get; init; }

        public string? Nome { get; set; }

        public int Idade { get; set; }
    }
}
