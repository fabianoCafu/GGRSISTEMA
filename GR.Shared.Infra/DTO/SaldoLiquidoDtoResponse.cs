namespace GR.Shared.Infra.DTO
{
    public class SaldoLiquidoDtoResponse
    {
        public Guid PessoaId { get; set; }
        public string ?Nome { get; set; }
        public decimal Receitas { get; set; }
        public decimal Despesas { get; set; }
        public decimal Saldo { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal TotalSaldo { get; set; }
    }
}
