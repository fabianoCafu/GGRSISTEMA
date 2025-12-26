using static Shared.Aplication.Enum.Enums;

namespace GR.Shared.Infra.Model
{
    public class Categoria
    {
        public Guid Id { get; init; }

        public string? Descricao { get; set; }

        public FinalidadeCategoria Finalidade { get; set; }

        public DateTime DataCriacaoRegistro { get; init; }

        public Categoria() 
        {
            Id = Guid.NewGuid();
            DataCriacaoRegistro = DateTime.Now;
        }

        public Categoria(
            string descricao,
            FinalidadeCategoria finalidade)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            Finalidade = finalidade;
            DataCriacaoRegistro = DateTime.Now;
        }
    }
}
