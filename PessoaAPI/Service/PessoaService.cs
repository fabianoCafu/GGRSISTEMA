using AutoMapper;
using GR.Shared.Infra.DTO;
using GR.Shared.Infra.Model;
using GR.Shared.Infra.Repository;
using static Shared.Result.ResultMessage;

namespace GR.PessoaAPI.Service
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRespository _pessoaRepository;
        private readonly IMapper _mapper;

        public PessoaService(
            IPessoaRespository pessoaRepository,
            IMapper mapper)
        {
            _pessoaRepository = pessoaRepository ?? throw new ArgumentNullException(nameof(pessoaRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async  Task<Result<PessoaDtoResponse>> CreateAsync(PessoaDtoRequest pessoaDtoRequest)
        {
            try
            {
                if (pessoaDtoRequest is null)
                {
                    return Result<PessoaDtoResponse>.Failure("Falha a pessoaDtoRequest deve ser diferente de null!");
                }

                if (!naoEhNegativo(pessoaDtoRequest.Idade))
                {
                    return Result<PessoaDtoResponse>.Failure("A idade da Pessoa não pode ser negativa!");
                }

                var pessoa = _mapper.Map<Pessoa>(pessoaDtoRequest);
                var result = await _pessoaRepository.CreateAsync(pessoa);

                if (result.IsFailure)
                {
                    return Result<PessoaDtoResponse>.Failure("Falha ao cadastrar uma Pessoa!");
                }

                var pessoaDtoResponse = _mapper.Map<PessoaDtoResponse>(result.Objet);
                return Result<PessoaDtoResponse>.Success(pessoaDtoResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message} ao cadastra uma Pessoa!");
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid idPessoa)
        {
            try
            {
                if (idPessoa == Guid.Empty)
                {
                    return Result<bool>.Failure("Falha o idPessoa deve ser diferente de null!");
                }

                var result = await _pessoaRepository.DeleteAsync(idPessoa);

                if (result.IsFailure)
                {
                    return Result<bool>.Failure("Falha ao Remover Pessoa!");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message} ao deletar uma Pessoa!");
            }
        }

        public async Task<Result<List<PessoaDtoResponse>>> GetByName(string nomePessoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomePessoa))
                {
                    return Result<List<PessoaDtoResponse>>.Failure("Falha o nome da Pessoa deve ser informado!");
                }

                var result = await _pessoaRepository.GetByName(nomePessoa.Trim());
                var listaPessoaDtoResponse = _mapper.Map<List<PessoaDtoResponse>>(result.Objet);

                return Result<List<PessoaDtoResponse>>.Success(listaPessoaDtoResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message} ao buscar uma Pessoa!");
            }
        }

        public async Task<Result<List<PessoaDtoResponse>>> GetAllAsync()
        {
            try
            {
                var result = await _pessoaRepository.GetAllAsync();

                if (result.IsFailure)
                {
                    return Result<List<PessoaDtoResponse>>.Failure("Falha ao listar Pessoas!");
                }

                var listaPessoaDtoResponse = _mapper.Map<List<PessoaDtoResponse>>(result.Objet);
                return Result<List<PessoaDtoResponse>>.Success(listaPessoaDtoResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message} ao listar Pessoas!");
            }
        }

        //public async Task<Result<List<SaldoLiquidoDtoResponse>>> GetNetBalance(Guid pessoaId)
        //{
        //    try
        //    {
        //        var result = await _pessoaRepository.GetNetBalance(pessoaId);

        //        if (result is null)
        //        {
        //            return Result<List<SaldoLiquidoDtoResponse>>.Failure("Falha ao listar Saldo!");
        //        }

        //        return Result<List<SaldoLiquidoDtoResponse>>.Success(result.Objet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error {ex.Message} ao listar Saldo!");
        //    }
        //}

        Func<int, bool> naoEhNegativo = idade => idade >= 0;
    }
}
