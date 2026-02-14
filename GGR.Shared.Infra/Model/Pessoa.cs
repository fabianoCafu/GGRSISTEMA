namespace GGR.Shared.Infra.Model
{
    public class Pessoa
    {
        public Guid Id { get; init; }
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public DateTime DataCriacaoRegistro { get; init; }
        public ICollection<Transacao> Transacoes { get; set; }
        public Pessoa() 
        {
            Id = Guid.NewGuid();
            DataCriacaoRegistro = DateTime.Now;
        }

        public Pessoa(
            string nome,
            int idade)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Idade = idade;
            DataCriacaoRegistro = DateTime.Now;
        }
    }
}
