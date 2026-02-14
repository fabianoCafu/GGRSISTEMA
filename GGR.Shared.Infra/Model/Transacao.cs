using static Shared.Aplication.Enum.Enums;

namespace GGR.Shared.Infra.Model
{
    public class Transacao
    {
        public Guid Id { get; init; }
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; } 
        public Guid CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public Guid PessoaId { get; set; }
        public Pessoa? Pessoa { get; set; }
        public DateTime DataCriacaoRegistro { get; init; }

        public Transacao() 
        {
            Id = Guid.NewGuid();
            DataCriacaoRegistro = DateTime.Now;
        }

        public Transacao(
            decimal valor,
            TipoTransacao tipo,
            Guid categoriaId,
            Guid pessoaId)
        {
            Id = Guid.NewGuid();
            Valor = valor;
            Tipo = tipo;
            CategoriaId = categoriaId;
            PessoaId = pessoaId;
            DataCriacaoRegistro = DateTime.Now;
        }
    }
}
