using GGR.Shared.Infra.Data;
using GGR.Shared.Infra.DTO;
using GGR.Shared.Infra.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Shared.Result.ResultMessage;

namespace GGR.Shared.Infra.Repository
{
    public class TransicaoRepository : ITransacaoRepository
    {
        private readonly MySQLContext _context;
        private readonly ILogger<PessoaRepository> _logger;

        public TransicaoRepository(
            MySQLContext context,
            ILogger<PessoaRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Transacao>> CreateAsync(Transacao transacao)
        {
            try
            {
                _context.Transacoes!.Add(transacao);
                await _context.SaveChangesAsync();

                return Result<Transacao>.Success(transacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar uma transação no banco.");
                throw;
            }
        }

        public async Task<Result<List<Transacao>>> GetAllAsync()
        {
            try
            {
                var listaDeTransacoes = await _context.Transacoes!
                                                      .Include(p => p.Pessoa)!
                                                      .Include(c => c.Categoria)
                                                      .AsNoTracking()
                                                      .OrderByDescending(x => x.DataCriacaoRegistro)
                                                      .ToListAsync();

                return Result<List<Transacao>>.Success(listaDeTransacoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar transações no banco.");
                throw;
            }
        }

        public async Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalancePerson()
        {
            try
            {
                var listaSaldoLiquidoPessoa = await BalancePerPersonQuery();
                var totalReceitas = listaSaldoLiquidoPessoa.Sum(x => x.Receitas);
                var totalDespesas = listaSaldoLiquidoPessoa.Sum(x => x.Despesas);
                var totalSaldo = totalReceitas - totalDespesas;

                listaSaldoLiquidoPessoa.ForEach(x =>
                {
                    x.TotalReceitas = totalReceitas;
                    x.TotalDespesas = totalDespesas;
                    x.TotalSaldo = totalSaldo;
                });

                if (!listaSaldoLiquidoPessoa.Any())
                {
                    return Result<List<SaldoLiquidoDtoResponse>>.Failure("Falha saldo liquido não encontrado!");
                }

                return Result<List<SaldoLiquidoDtoResponse>>.Success(listaSaldoLiquidoPessoa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pessoa no banco.");
                throw;
            }
        }

        public async Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalanceCategory()
        {
            try
            {
                var listaSaldoLiquidoCategoria = await BalanceByCategoryQuery(); 
                var totalReceitas = listaSaldoLiquidoCategoria.Sum(x => x.Receitas);
                var totalDespesas = listaSaldoLiquidoCategoria.Sum(x => x.Despesas);
                var totalSaldo = totalReceitas - totalDespesas;

                listaSaldoLiquidoCategoria.ForEach(x =>
                {
                    x.TotalReceitas = totalReceitas;
                    x.TotalDespesas = totalDespesas;
                    x.TotalSaldo = totalSaldo;
                });

                if (!listaSaldoLiquidoCategoria.Any())
                {
                    return Result<List<SaldoLiquidoDtoResponse>>.Failure("Falha saldo liquido não encontrado!");
                }

                return Result<List<SaldoLiquidoDtoResponse>>.Success(listaSaldoLiquidoCategoria);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pessoa no banco.");
                throw;
            }
        }

        private async Task<List<SaldoLiquidoDtoResponse>> BalancePerPersonQuery()
        {
            return await _context.Transacoes!
                                 .GroupBy(t => new
                                 {
                                     t.PessoaId,
                                     t.Pessoa!.Nome
                                 })
                                 .Select(g => new SaldoLiquidoDtoResponse
                                 {
                                     PessoaId = g.Key.PessoaId,
                                     Nome = g.Key.Nome,
                                     Receitas = g.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0,
                                     Despesas = g.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0,
                                     Saldo = (g.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0)
                                     - (g.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0)
                                 }).ToListAsync();
        }

        private async Task<List<SaldoLiquidoDtoResponse>> BalanceByCategoryQuery()
        {
            return await _context.Transacoes!
                                 .GroupBy(t => new
                                 {
                                     t.CategoriaId,
                                     t.Categoria!.Descricao
                                 })
                                 .Select(g => new SaldoLiquidoDtoResponse
                                 {
                                     CategoriaId = g.Key.CategoriaId,
                                     Nome = g.Key.Descricao,
                                     Receitas = g.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0,
                                     Despesas = g.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0,
                                     Saldo = (g.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0)
                                     - (g.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0)
                                 })
                                 .ToListAsync();
        }
    }
}
