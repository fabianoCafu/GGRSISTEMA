using GR.Shared.Infra.Data;
using GR.Shared.Infra.DTO;
using GR.Shared.Infra.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Shared.Result.ResultMessage;

namespace GR.Shared.Infra.Repository
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

        public async Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalance()
        {
            try
            {
                var listaSaldoLiquido = await _context.Transacoes!
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
                                                          Saldo = (g.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0) - (g.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0)
                                                      }).ToListAsync();


                var totalReceitas = listaSaldoLiquido.Sum(x => x.Receitas);
                var totalDespesas = listaSaldoLiquido.Sum(x => x.Despesas);
                var totalSaldo = totalReceitas - totalDespesas;

                listaSaldoLiquido.ForEach(x =>
                {
                    x.TotalReceitas = totalReceitas;
                    x.TotalDespesas = totalDespesas;
                    x.TotalSaldo = totalSaldo;
                });

                if (!listaSaldoLiquido.Any())
                {
                    return Result<List<SaldoLiquidoDtoResponse>>.Failure("Falha saldo liquido não encontrado!");
                }

                return Result<List<SaldoLiquidoDtoResponse>>.Success(listaSaldoLiquido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pessoa no banco.");
                throw;
            }
        }

        //public async Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalance(Guid pessoaId)
        //{
        //    try
        //    {
        //        var listaSaldoLiquidoPessoa = await _context.Transacoes!
        //                                                    .Where(t => t.PessoaId == pessoaId)
        //                                                    .GroupBy(t => new
        //                                                    {
        //                                                        t.PessoaId,
        //                                                        t.Pessoa!.Nome
        //                                                    })
        //                                                    .Select(g => new SaldoLiquidoDtoResponse
        //                                                    {
        //                                                        PessoaId = g.Key.PessoaId,
        //                                                        Nome = g.Key.Nome,

        //                                                        TotalReceitas = g.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0,
        //                                                        TotalDespesas = g.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0,
        //                                                        Saldo = (g.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0) - (g.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0)
        //                                                    })
        //                                                    .ToListAsync();

        //        if (!listaSaldoLiquidoPessoa.Any())
        //        {
        //            return Result<List<SaldoLiquidoDtoResponse>>.Failure("Falha saldo liquido não encontrado!");
        //        }

        //        return Result<List<SaldoLiquidoDtoResponse>>.Success(listaSaldoLiquidoPessoa);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erro ao buscar pessoa no banco.");
        //        throw;
        //    }
        //}
    }
}
