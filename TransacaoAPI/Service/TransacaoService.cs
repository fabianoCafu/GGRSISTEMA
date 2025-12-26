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

                if (pessoa.IsFailure)
                {
                    return Result<TransacaoDtoResponse>.Failure("Falha pessoa não encontrada!");
                }

                if (ValidaAhFinalidadeDaTransacao(pessoa.Objet!.Idade, transacaoDtoRequest.Tipo))
                {
                    return Result<TransacaoDtoResponse>.Failure("Pessao MENOR DE IDADE apenas Despesas deverão ser aceitas!");
                }

                var categoria = await _categoriaRespository.GetCategoriaByIdAsync(transacaoDtoRequest.CategoriaId);

                if (!ValidaAhCategotiaDaTransacao(categoria.Objet!.Finalidade, transacaoDtoRequest.Tipo))
                {
                    return Result<TransacaoDtoResponse>.Failure("O Tipo da Transação deve ser compatível com a Finalidade da Categoria!");
                }

                var transacao = _mapper.Map<Transacao>(transacaoDtoRequest);
                var result = await _transacaoRepository.CreateAsync(transacao);

                if (result.IsFailure)
                {
                    return Result<TransacaoDtoResponse>.Failure("Falha ao cadastrar uma Transação!");
                }

                var transacaoDtoResponse = _mapper.Map<TransacaoDtoResponse>(result.Objet);
                return Result<TransacaoDtoResponse>.Success(transacaoDtoResponse);
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

        Func<int, TipoTransacao, bool> ValidaAhFinalidadeDaTransacao = (idadePessoa, tipoTransacao) =>
        {
            return (idadePessoa < MAIOR_IDADE && tipoTransacao != TipoTransacao.Despesa);
        };

        Func<FinalidadeCategoria, TipoTransacao, bool> ValidaAhCategotiaDaTransacao = (finalidadeCategoria, tipoTransacao) =>
        {
            return (((int)finalidadeCategoria == (int)tipoTransacao) || ((int)tipoTransacao == TIPO_AMBOS));
        };
    }
}
