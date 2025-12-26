using GR.Shared.Infra.Data;
using GR.Shared.Infra.DTO;
using GR.Shared.Infra.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Shared.Result.ResultMessage;

namespace GR.Shared.Infra.Repository
{
    public class PessoaRepository : IPessoaRespository
    {
        private readonly MySQLContext _context;
        private readonly ILogger<PessoaRepository> _logger;

        public PessoaRepository(
            MySQLContext context,
            ILogger<PessoaRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Pessoa>> CreateAsync(Pessoa pessoa)
        {
            try
            {
                _context.Pessoas!.Add(pessoa);
                await _context.SaveChangesAsync();

                return Result<Pessoa>.Success(pessoa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar uma pessoa no banco.");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid idPessoa)
        {
            try
            {
                var pessoaEntity = await _context.Pessoas!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == idPessoa);

                if (pessoaEntity is null)
                {
                    return Result<bool>.Failure("Falha pessoa não encontrada!");
                }

                _context.Pessoas!.Remove(pessoaEntity);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar uma pessoa no banco.");
                throw;
            }
        }

        public async Task<Result<List<Pessoa>>> GetAllAsync()
        {
            try
            {
                var listaDePessoas = await _context.Pessoas!
                                                   .AsNoTracking()
                                                   .OrderByDescending(x => x.DataCriacaoRegistro)
                                                   .ToListAsync();

                return Result<List<Pessoa>>.Success(listaDePessoas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pessoas no banco.");
                throw;
            }
        }

        public async Task<Result<List<Pessoa>>> GetByName(string nomePessoa)
        {
            try
            {
                var listaDePessoas = await _context.Pessoas!
                                                   .AsNoTracking()
                                                   .Where(p => p.Nome!.Contains(nomePessoa))
                                                   .OrderByDescending(p => p.DataCriacaoRegistro)
                                                   .ToListAsync();

                return Result<List<Pessoa>>.Success(listaDePessoas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pessoa no banco.");
                throw;
            }
        }

        public async Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalance(Guid pessoaId)
        {
            try
            {
                var listaSaldoLiquidoPessoa = await _context.Transacoes!
                    .Where(t => t.PessoaId == pessoaId)
                    .GroupBy(t => new
                    {
                        t.PessoaId,
                        t.Pessoa!.Nome
                    })
                    .Select(g => new SaldoLiquidoDtoResponse
                    {
                        PessoaId = g.Key.PessoaId,
                        Nome = g.Key.Nome,

                        TotalReceitas = g.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0,
                        TotalDespesas = g.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0,
                        Saldo = (g.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0) - (g.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0)
                    })
                    .ToListAsync();


                //var listaSaldoLiquidoPessoa = await _context.Transacoes!
                //    .Include(s => s.Pessoa)
                //    .Include(c => c.Categoria)
                //    .Distinct()
                //    .Where(p => p.Pessoa!.Id == pessoaId) 
                //    .Select(s => new SaldoLiquidoDtoResponse
                //    {
                //        PessoaId = s.Pessoa!.Id,
                //        Nome = s.Pessoa.Nome,
                //        TotalReceitas = s.Pessoa.Transacoes.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0,
                //        TotalDespesas = s.Pessoa.Transacoes.Where(t => Convert.ToInt32(t.Tipo) == 2).Sum(t => (decimal?)t.Valor) ?? 0,
                //        Saldo = (s.Pessoa.Transacoes.Where(t => Convert.ToInt32(t.Tipo) == 1).Sum(t => (decimal?)t.Valor) ?? 0) - (s.Pessoa.Transacoes.Where(t => Convert.ToInt32(t.Tipo) ==  2 ).Sum(t => (decimal?)t.Valor) ?? 0)
                //    })
                //    .ToListAsync();

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

        public async Task<Result<Pessoa>> GetPessoaByIdAsync(Guid pessoaId)
        {
            try
            {
                var pessoa = await _context.Pessoas!.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pessoaId);
                return Result<Pessoa>.Success(pessoa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pessoa no banco.");
                throw;
            }
        }
    }
}
