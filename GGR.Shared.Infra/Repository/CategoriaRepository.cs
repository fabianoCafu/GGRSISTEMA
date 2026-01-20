using GR.Shared.Infra.Data;
using GR.Shared.Infra.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Shared.Result.ResultMessage;

namespace GR.Shared.Infra.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly MySQLContext _context;
        private readonly ILogger<CategoriaRepository> _logger;

        public CategoriaRepository(
            MySQLContext context,
            ILogger<CategoriaRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Categoria>> CreateAsync(Categoria categoria)
        {
            try
            {
                _context.Categorias!.Add(categoria);
                await _context.SaveChangesAsync();

                return Result<Categoria>.Success(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar uma categoria no banco.");
                throw;
            }
        }

        public async Task<Result<List<Categoria>>> GetAllAsync()
        {
            try
            {
                var listaDeCategorias = await _context.Categorias!
                                                      .AsNoTracking()
                                                      .OrderByDescending(x => x.DataCriacaoRegistro)
                                                      .ToListAsync();

                return Result<List<Categoria>>.Success(listaDeCategorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar categorias no banco.");
                throw;
            }
        }

        public async Task<Result<List<Categoria>>> GetByDescription(string descricaoCategoria)
        {
            try
            {
                var listaDeCategorias = await _context.Categorias!
                                                      .AsNoTracking()
                                                      .Where(p => p.Descricao!.Contains(descricaoCategoria))
                                                      .OrderByDescending(p => p.DataCriacaoRegistro)
                                                      .ToListAsync();

                return Result<List<Categoria>>.Success(listaDeCategorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar categoria no banco.");
                throw;
            }
        }

        public async Task<Result<Categoria>> GetCategoriaByIdAsync(Guid categoriaId)
        {
            try
            {
                var categoria = await _context.Categorias!.FirstOrDefaultAsync(c => c.Id == categoriaId);
                return Result<Categoria>.Success(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar categoria no banco.");
                throw;
            }
        }
    }
}
