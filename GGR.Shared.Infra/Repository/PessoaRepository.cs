using GGR.Shared.Infra.Data;
using GGR.Shared.Infra.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Shared.Result.ResultMessage;

namespace GGR.Shared.Infra.Repository
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
                _logger.LogError($"Erro ao criar uma pessoa no banco. - {ex.Message}");
                throw new Exception("Error ao criar uma Pessoa no Banco de Dados!"); 
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid idPessoa)
        {
            try
            {
                var pessoa = await _context.Pessoas!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == idPessoa);
                
                if (pessoa is null)
                {
                    return Result<bool>.Failure("Falha pessoa não encontrada!");
                }

                var transacoes = _context.Transacoes!.AsNoTracking().Where(t => t.Pessoa!.Id == idPessoa);

                if (transacoes.Any())
                {
                    _context.Transacoes!.RemoveRange(transacoes);
                }

                _context.Pessoas!.Remove(pessoa);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar uma pessoa no banco. - {ex.Message}");
                throw new Exception("Error ao deletar uma Pessoa no Banco de Dados!"); 
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
                _logger.LogError($"Erro ao buscar pessoas no banco. - {ex.Message}");
                throw new Exception("Error ao buscar Pessoas no Banco de Dados!");
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
                _logger.LogError($"Erro ao buscar pessoa no banco. - {ex.Message}");
                throw new Exception("Error ao buscar Pessoa pelo Nome no Banco de Dados!");
            }
        }

        public async Task<Result<Pessoa>> GetPessoaByIdAsync(Guid pessoaId)
        {
            try
            {
                var pessoa  = await _context.Pessoas!.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pessoaId);

                if (pessoa is null)
                {
                    return Result<Pessoa>.Failure("Falha pessoa não encontrada!");
                }

                return Result<Pessoa>.Success(pessoa);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar pessoa pelo Id no banco. - {ex.Message}");
                throw new Exception("Error ao ao buscar Pessoa pelo Id no Banco de Dados!");
            }
        }
    }
}
