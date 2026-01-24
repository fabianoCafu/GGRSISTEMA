using AutoMapper;
using GR.Shared.Infra.DTO;
using GR.Shared.Infra.Model;
using GR.Shared.Infra.Repository;
using static Shared.Result.ResultMessage;

namespace GR.CategoriaAPI.Service
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(
            ICategoriaRepository categoriaRepository,
            IMapper mapper)
        {
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<CategoriaDtoResponse>> CreateAsync(CategoriaDtoRequest categoriaDtoRequest)
        {
            try
            {
                if (categoriaDtoRequest is null)
                {
                    return Result<CategoriaDtoResponse>.Failure("Falha a categoriaDtoRequest deve ser diferente de null!");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDtoRequest);
                var result = await _categoriaRepository.CreateAsync(categoria);

                if (result.IsFailure)
                {
                    return Result<CategoriaDtoResponse>.Failure("Falha ao cadastrar uma Categoria!");
                }

                var categoriaDtoResponse = _mapper.Map<CategoriaDtoResponse>(result.Objet);
                return Result<CategoriaDtoResponse>.Success(categoriaDtoResponse);
            }
            catch
            {
                throw new Exception("Error ao cadastra uma Categoria!");
            }
        }

        public async Task<Result<List<CategoriaDtoResponse>>> GetByDescription(string descricaoCategoria)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descricaoCategoria))
                {
                    return Result<List<CategoriaDtoResponse>>.Failure("Falha a Descrição da Categoria deve ser informado!");
                }

                var result = await _categoriaRepository.GetByDescription(descricaoCategoria.Trim());
                var listaCategoriaDtoResponse = _mapper.Map<List<CategoriaDtoResponse>>(result.Objet);

                return Result<List<CategoriaDtoResponse>>.Success(listaCategoriaDtoResponse);
            }
            catch 
            {
                throw new Exception("Error ao buscar uma Categoria pela Descrição!");
            }
        }

        public async Task<Result<List<CategoriaDtoResponse>>> GetAllAsync()
        {
            try
            {
                var result = await _categoriaRepository.GetAllAsync();

                if (result.IsFailure)
                {
                    return Result<List<CategoriaDtoResponse>>.Failure("Falha ao listar Categorias!");
                }

                var pessoaDtoResponse = _mapper.Map<List<CategoriaDtoResponse>>(result.Objet);
                return Result<List<CategoriaDtoResponse>>.Success(pessoaDtoResponse);

            }
            catch 
            {
                throw new Exception("Error ao listar Categorias!");
            }
        }
    }
}
