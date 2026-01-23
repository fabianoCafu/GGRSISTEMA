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

        #endregion

        #region EndPoint List
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
