using AutoMapper;
using GR.PessoaAPI.Service;
using GR.Shared.Infra.DTO;
using GR.Shared.Infra.Model;
using GR.Shared.Infra.Repository;
using Moq;
using Xunit;
using static Shared.Result.ResultMessage;

namespace GGR.PessoaAPI.Tests
{
    public class PessoaSeviceTests
    {
        private readonly Mock<IPessoaRespository> _mockPessoaRepository;
        private readonly Mock<IMapper> _mockMapper;
        
        public PessoaSeviceTests()
        {
            _mockPessoaRepository = new Mock<IPessoaRespository>();
            _mockMapper = new Mock<IMapper>();
        }

        #region EndPointt Create

        [Fact]
        public async Task Create_Deve_RetornarUmIsSuccess_QuandoPessoaForCriadaComSucesso()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());
            
            _mockPessoaRepository.Setup(r => r.CreateAsync(It.IsAny<Pessoa>()))
                                 .ReturnsAsync(Result<Pessoa>.Success(MockPessoaRequest()));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaRequest = MockPessoaDtoRequest();

            // Act
            var result = await pessoaService.CreateAsync(pessoaRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public async Task Create_Deve_RetornarUmFailure_QuandoPessoaDtoRequest_ForIgualAhNull()
        {
            // Arrange
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.CreateAsync(It.IsAny<Pessoa>()))
                                 .ReturnsAsync(Result<Pessoa>.Success(MockPessoaRequest()));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            
            // Act
            var result = await pessoaService.CreateAsync(null!);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Falha a pessoaDtoRequest deve ser diferente de null!", result.Error);
        }

        [Fact]
        public async Task Create_Deve_RetornarUmIsFailure_QuandoPessoaDtoRequest_IdadeForUmNumeroNegativo()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.CreateAsync(It.IsAny<Pessoa>()))
                                 .ReturnsAsync(Result<Pessoa>.Success(MockPessoaRequest()));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaRequest = MockPessoaDtoRequest();
            pessoaRequest.Idade = -7;

            // Act
            var result = await pessoaService.CreateAsync(pessoaRequest);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("A idade da Pessoa deve de ser MAIOR que 0.", result.Error); 
        }

        [Fact]
        public async Task Create_Deve_RetornarUmIsFailure_QuandoHouverUmaFalhaNaCriacaoDaPessoa()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.CreateAsync(It.IsAny<Pessoa>()))
                                 .ReturnsAsync(Result<Pessoa>.Failure("Falha ao cadastrar uma Pessoa!"));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaRequest = MockPessoaDtoRequest();

            // Act
            var result = await pessoaService.CreateAsync(pessoaRequest);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Falha ao cadastrar uma Pessoa!", result.Error); 
        }

        [Fact]
        public async Task Create_Deve_RetornarUmaExcption_QuandoHouverUmaFalhaNaCriacaoDaPessoa()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.CreateAsync(It.IsAny<Pessoa>())) 
                                 .ThrowsAsync(new Exception("Error ao cadastra uma Pessoa!"));
            
            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaRequest = MockPessoaDtoRequest();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => pessoaService.CreateAsync(pessoaRequest));

            // Assert 
            Assert.Equal("Error ao cadastra uma Pessoa!", exception.Message);
        }

        #endregion

        #region EndPoint Delete

        [Fact]
        public async Task Delete_Deve_RetornarUmIsSuccess_QuandoPessoaForDeletadaComSucesso()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
                                 .ReturnsAsync(Result<bool>.Success(true));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaResponse = MockPessoaDtoResponse();

            // Act
            var result = await pessoaService.DeleteAsync(pessoaResponse.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public async Task Delete_Deve_RetornarUmIsFailure_QuandoPessoaDtoRequest_GuidForEmpty()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.CreateAsync(It.IsAny<Pessoa>()))
                                 .ReturnsAsync(Result<Pessoa>.Success(MockPessoaRequest()));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            
            // Act
            var result = await pessoaService.DeleteAsync(Guid.Empty);
            
            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal("Falha o idPessoa deve ser diferente de null!", result.Error);
        }

        [Fact]
        public async Task Delete_Deve_RetornarUmIsFailure_QuandoHouverUmaFalhaAoDeletarUmaPessoa()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
                                 .ReturnsAsync(Result<bool>.Failure("Falha ao Remover Pessoa!"));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaResponse = MockPessoaDtoResponse();

            // Act
            var result = await pessoaService.DeleteAsync(pessoaResponse.Id);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Falha ao Remover Pessoa!", result.Error);
        }

        [Fact]
        public async Task Delete_Deve_RetornarUmaExcption_QuandoForDeletarUmaPessoa()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
                                 .ThrowsAsync(new Exception("Error ao cadastra uma Pessoa!"));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaResponse = MockPessoaDtoResponse();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => pessoaService.DeleteAsync(pessoaResponse.Id));

            // Assert 
            Assert.Equal("Error ao cadastra uma Pessoa!", exception.Message);
        }


        #endregion

        #region EndPoit GetByName

        [Fact]
        public async Task GetByName_Deve_RetornarUmIsSuccess_QuandoAhBuscaForRealizadaComSucesso()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.GetByName(It.IsAny<string>()))
                                 .ReturnsAsync(Result<List<Pessoa>>.Success(new List<Pessoa>()));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaResponse = MockPessoaDtoResponse();

            // Act
            var result = await pessoaService.GetByName(pessoaResponse.Nome!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public async Task GetByName_Deve_RetornarUmaExcption_QuandoRealizadoUmaBuscaPeloNomeDaPessoa()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.GetByName(It.IsAny<string>()))
                                 .ThrowsAsync(new Exception("Error ao buscar uma Pessoa!"));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaResponse = MockPessoaDtoResponse();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => pessoaService.GetByName(pessoaResponse.Nome!));

            // Assert 
            Assert.Equal("Error ao buscar uma Pessoa!", exception.Message);
        }

        [Fact]
        public async Task GetByName_Deve_RetornarUmIsFailure_QuandoPessoaDtoRequest_NomeForEmpty()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.GetByName(It.IsAny<string>()))
                                 .ReturnsAsync(Result<List<Pessoa>>.Failure("Falha o nome da Pessoa deve ser informado!"));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            var pessoaResponse = MockPessoaDtoResponse();
            pessoaResponse.Nome = string.Empty;

            // Act
            var result = await pessoaService.GetByName(pessoaResponse.Nome);

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal("Falha o nome da Pessoa deve ser informado!", result.Error);
        }

        #endregion

        #region EndPoint GetAllAsync
        [Fact]
        public async Task GetAllAsync_Deve_RetornarUmIsSuccess_QuandoAhBuscaPorTodasAsPessoaForRealizadaComSucesso()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.GetAllAsync())
                                 .ReturnsAsync(Result<List<Pessoa>>.Success(new List<Pessoa>()));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            
            // Act
            var result = await pessoaService.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public async Task GetAllAsync_Deve_RetornarUmIsFailure_QuandoBustarPorTodasAsPessoas()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.GetAllAsync())
                                 .ReturnsAsync(Result<List<Pessoa>>.Failure("Falha ao listar Pessoas!"));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            
            // Act
            var result = await pessoaService.GetAllAsync();

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal("Falha ao listar Pessoas!", result.Error);
        }

        [Fact]
        public async Task GetAllAsync_Deve_RetornarUmaExcption_QuandoBustarPorTodasAsPessoas()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
                       .Returns(MockPessoaRequest());

            _mockPessoaRepository.Setup(r => r.GetAllAsync())
                                 .ThrowsAsync(new Exception("Falha ao listar Pessoas!"));

            _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
                       .Returns(MockPessoaDtoResponse());

            var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);
            
            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => pessoaService.GetAllAsync());

            // Assert 
            Assert.Equal("Falha ao listar Pessoas!", exception.Message);
        }

        #endregion

        private static PessoaDtoRequest MockPessoaDtoRequest()
        {
            return new PessoaDtoRequest
            {
                Nome = "João Alfredo Cunha",
                Idade = 35
            };
        }

        private static Pessoa MockPessoaRequest()
        {
            return new Pessoa
            {
                Nome = "João Alfredo Cunha",
                Idade = 35
            };
        }

        private static PessoaDtoResponse MockPessoaDtoResponse()
        {
            return new PessoaDtoResponse
            {
                Id = Guid.NewGuid(),
                Nome = "João Alfredo Cunha",
                Idade = 35
            };
        }
    }
}
