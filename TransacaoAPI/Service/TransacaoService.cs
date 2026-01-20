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
        private readonly IPessoaRespository _pessoaRepository;
        private readonly ICategoriaRepository _categoriaRespository;
        private readonly IMapper _mapper;
        public const int MAIOR_IDADE = 18;
        public const int TIPO_AMBOS = 0;

        public TransacaoService(
            ITransacaoRepository transacaoRepository,
            IPessoaRespository pessoaRepository,
            ICategoriaRepository categoriaRespository,
            IMapper mapper)
        {
            _transacaoRepository = transacaoRepository ?? throw new ArgumentNullException(nameof(transacaoRepository));
            _pessoaRepository = pessoaRepository ?? throw new ArgumentNullException(nameof(pessoaRepository));
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

                var pessoa = await _pessoaRepository.GetPessoaByIdAsync(transacaoDtoRequest.PessoaId);
                var categoria = await _categoriaRespository.GetCategoriaByIdAsync(transacaoDtoRequest.CategoriaId);
                var resultadoValidaTransacao = ValidaTransacao(transacaoDtoRequest, pessoa, categoria);

                if (resultadoValidaTransacao.IsFailure)
                {
                    return resultadoValidaTransacao;
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

        public async Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalancePerson()
        {
            try
            {
                var result = await _transacaoRepository.GetNetBalancePerson();

                if (result is null)
                {
                    return Result<List<SaldoLiquidoDtoResponse>>.Failure("Falha ao listar Saldo!");
                }

                return Result<List<SaldoLiquidoDtoResponse>>.Success(result.Objet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message} ao listar Saldo!");
            }
        }

        public async Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalanceCategory()
        {
            try
            {
                var result = await _transacaoRepository.GetNetBalanceCategory();

                if (result is null)
                {
                    return Result<List<SaldoLiquidoDtoResponse>>.Failure("Falha ao listar Saldo!");
                }

                return Result<List<SaldoLiquidoDtoResponse>>.Success(result.Objet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message} ao listar Saldo!");
            }
        }

        readonly Func<TransacaoDtoRequest, Result<Pessoa>, Result<Categoria>, Result<TransacaoDtoResponse>>
            ValidaTransacao = (transacaoDtoRequest, pessoa, categoria) =>
        {
            if (pessoa.IsFailure)
            {
                return Fail("Falha pessoa não encontrada!");
            }

            if (categoria.IsFailure)
            {
                return Fail("Falha categoria não encontrada!");
            }

            if (pessoa.Objet!.Idade < MAIOR_IDADE && transacaoDtoRequest.Tipo != TipoTransacao.Despesa)
            {
                return Fail("Pessoa MENOR DE IDADE apenas Despesas deverão ser aceitas!");
            }

            if (((int)categoria.Objet!.Finalidade != (int)transacaoDtoRequest.Tipo) && (transacaoDtoRequest.Tipo != TIPO_AMBOS))
            {
                return Fail("O Tipo da Transação deve ser compatível com a Finalidade da Categoria!");
            }

            return Result<TransacaoDtoResponse>.Success(new TransacaoDtoResponse());
        };

        private static Result<TransacaoDtoResponse> Fail(string mensagem) => Result<TransacaoDtoResponse>.Failure(mensagem); 
    }
}
