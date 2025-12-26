namespace GR.PessoaAPI.DTO
{
    public class PessoaDtoResponse
    {
        public Guid Id { get; init; }
        public string? Nome { get; set; }
        public int Idade { get; set; }
    }
}
