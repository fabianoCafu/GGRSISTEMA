using GR.Shared.Infra.Data;
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
    }
}
