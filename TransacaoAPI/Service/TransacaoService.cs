using AutoMapper;
using GR.Shared.Infra.DTO;
using GR.Shared.Infra.Model;
using GR.Shared.Infra.Repository;
using static Shared.Aplication.Enum.Enums;
using static Shared.Result.ResultMessage;

namespace GR.TransacaoAPI.Service
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IPessoaRespository _pessaoRepository;
        private readonly ICategoriaRepository _categoriaRespository;
        private readonly IMapper _mapper;
        public const int MAIOR_IDADE = 18;
        public const int TIPO_AMBOS = 0;

        public TransacaoService(
            ITransacaoRepository transacaoRepository,
            IPessoaRespository pessaoRepository,
            ICategoriaRepository categoriaRespository,
            IMapper mapper)
        {
            _transacaoRepository = transacaoRepository ?? throw new ArgumentNullException(nameof(transacaoRepository));
            _pessaoRepository = pessaoRepository ?? throw new ArgumentNullException(nameof(pessaoRepository));
            _categoriaRespository = categoriaRespository ?? throw new ArgumentNullException(nameof(categoriaRespository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<TransacaoDtoResponse>> CreateAsync(TransacaoDtoRequest transacaoDtoRequest)
        {
            try
            {
                if (transacaoDtoRequest is null)
                {
                    return Result<TransacaoDtoResponse>.Failure("Falha a transacaoDtoRequest deve ser diferente de null!");
                }

                var pessoa = await _pessaoRepository.GetPessoaByIdAsync(transacaoDtoRequest.PessoaId);
                var categoria = await _categoriaRespository.GetCategoriaByIdAsync(transacaoDtoRequest.CategoriaId);
                var resultadoValidacao = ValidaTransacao(transacaoDtoRequest, pessoa, categoria);

                if (!resultadoValidacao.IsFailure)
                {
                    var transacao = _mapper.Map<Transacao>(transacaoDtoRequest);
                    var result = await _transacaoRepository.CreateAsync(transacao);

                    if (result.IsFailure)
                    {
                        return Result<TransacaoDtoResponse>.Failure("Falha ao cadastrar uma Transação!");
                    }

                    var transacaoDtoResponse = _mapper.Map<TransacaoDtoResponse>(result.Objet);
                    return Result<TransacaoDtoResponse>.Success(transacaoDtoResponse);
                }

                return resultadoValidacao;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message} ao cadastra uma Transação!");
            }
        }

        public async Task<Result<List<TransacaoDtoResponse>>> GetAllAsync()
        {
            try
            {
                var result = await _transacaoRepository.GetAllAsync();

                if (result.IsFailure)
                {
                    return Result<List<TransacaoDtoResponse>>.Failure("Falha ao listar Transações!");
                }

                var pessoaDtoResponse = _mapper.Map<List<TransacaoDtoResponse>>(result.Objet);
                return Result<List<TransacaoDtoResponse>>.Success(pessoaDtoResponse);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message} ao listar Transações!");
            }
        }

        readonly Func<TransacaoDtoRequest, Result<Pessoa>, Result<Categoria>, Result<TransacaoDtoResponse>>
            ValidaTransacao = (transacaoDtoRequest, pessoa, categoria) =>
            {
                if (pessoa.IsFailure)
                {
                    return Result<TransacaoDtoResponse>.Failure("Falha pessoa não encontrada!");
                }

                if (categoria.IsFailure)
                {
                    return Result<TransacaoDtoResponse>.Failure("Falha categoria não encontrada!");
                }

                if (pessoa.Objet!.Idade < MAIOR_IDADE && transacaoDtoRequest.Tipo != TipoTransacao.Despesa)
                {
                   return Result<TransacaoDtoResponse>.Failure("Pessao MENOR DE IDADE apenas Despesas deverão ser aceitas!");
                }

                if (((int)categoria.Objet!.Finalidade != (int)transacaoDtoRequest.Tipo) && (transacaoDtoRequest.Tipo != TIPO_AMBOS))
                {
                    return Result<TransacaoDtoResponse>.Failure("O Tipo da Transação deve ser compatível com a Finalidade da Categoria!");
                }

            return Result<TransacaoDtoResponse>.Success(new TransacaoDtoResponse());
        };
    }
}
