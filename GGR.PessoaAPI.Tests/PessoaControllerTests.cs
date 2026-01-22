using GR.PessoaAPI.Controllers;
using GR.PessoaAPI.Service;
using GR.Shared.Infra.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Shared.Result.ResultMessage;

namespace GGR.PessoaAPI.Tests
{
    public class PessoaControllerTests
    {
        private readonly Mock<IPessoaService> _mockPessoaService;
        private readonly Mock<PessoaDtoRequest> _mockPessoaDtoRequest;
        private readonly PessoaController _controller;

        public PessoaControllerTests()
        {
            _mockPessoaService = new Mock<IPessoaService>();
            _mockPessoaDtoRequest = new Mock<PessoaDtoRequest>();
            _controller = new PessoaController(_mockPessoaService.Object);
        }

        #region EndPointt Create

        [Fact]
        public void Create_Deve_RetornarUmBadRequest400_QuandoModelStateForInvalido()
        {
            // Arrange
            _controller.ModelState.AddModelError("Nome", "O atributo Nome é Obrigatório");
            _controller.ModelState.AddModelError("Idade", "O atributo Idade é Obrigatório");

            // Act
            var result = _controller.Create(_mockPessoaDtoRequest.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.True(_controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public async void Create_Deve_RetornarUmCreatedResult201_QuandoPessoaForCriadaComSucesso()
        {
            // Arrange 
            var pessoaDto = new PessoaDtoRequest { Nome = "Camila do Santos Martins", Idade = 34 };

            _mockPessoaService.Setup(a => a.CreateAsync(It.IsAny<PessoaDtoRequest>()))
                              .ReturnsAsync(Result<PessoaDtoResponse>.Success(new PessoaDtoResponse()));

            // Act 
            var result = await _controller.Create(pessoaDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public void Create_Deve_RetornarUmBadRequest400_AoTentarCriarUmaPessoa()
        {
            // Arrange
            _mockPessoaService.Setup(a => a.CreateAsync(It.IsAny<PessoaDtoRequest>()))
                              .ReturnsAsync(Result<PessoaDtoResponse>.Failure("Falha ao cadastrar uma Pessoa!"));

            // Act
            var result = _controller.Create(_mockPessoaDtoRequest.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);

        }

        [Fact]
        public async void Create_Deve_RetornarUmaException_AoTentarCriarUmaPessoa()
        {
            // Arrange 
            var pessoaDto = new PessoaDtoRequest { Nome = "Camila do Santos Martins", Idade = 34 };

            _mockPessoaService.Setup(r => r.CreateAsync(It.IsAny<PessoaDtoRequest>()))
                              .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.Create(pessoaDto));

            // Assert
            Assert.Equal("Error: Internal Server Error ao criar Pessoa!", exception.Message);
        }

        #endregion

        #region EndPoint Delete

        [Fact]
        public async void Delete_Deve_RetornarOk200_QuandoUmaPessoaForRemovidaComSucesso()
        {
            // Arrange 
            _mockPessoaService.Setup(a => a.DeleteAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void Delete_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaNaRemocaoDaPessoa()
        {
            // Arrange 
            _mockPessoaService.Setup(a => a.DeleteAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(Result<bool>.Failure(string.Empty));

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void Delete_Deve_RetornarUmaException_AoTentarRemoverUmaPessoa()
        {
            // Arrange 
            _mockPessoaService.Setup(a => a.DeleteAsync(It.IsAny<Guid>()))
                              .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.Delete(Guid.NewGuid()));
            
            // Assert
            Assert.Equal("Error: Internal Server Error ao deletar Pessoa!", exception.Message);
        }

        #endregion

        #region EndPoint List

        [Fact]
        public async void List_Deve_RetornarOk200_AoListarPessoas()
        {
            // Arrange 
            //var listaPessoas = new Result<List<PessoaDtoResponse>>();

            _mockPessoaService.Setup(a => a.GetAllAsync())
                              .ReturnsAsync(Result<List<PessoaDtoResponse>>.Success(new List<PessoaDtoResponse>()));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaAoTentarListarPessoas()
        {
            // Arrange 
            _mockPessoaService.Setup(a => a.GetAllAsync())
                              .ReturnsAsync(Result<List<PessoaDtoResponse>>.Failure(string.Empty));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmaException_AoTentarListarPessoas()
        {
            // Arrange 
            _mockPessoaService.Setup(a => a.GetAllAsync())
                              .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetAll());

            // Assert
            Assert.Equal("Error: Internal Server Error ao listar Pessoas!", exception.Message);
        }

        #endregion

        #region EndPoint GetByName

        [Fact]
        public async void GetByName_Deve_RetornarOk200_AoBuscarPessoaPeloNome()
        {
            // Arrange  
            _mockPessoaService.Setup(a => a.GetByName(It.IsAny<string>()))
                              .ReturnsAsync(Result<List<PessoaDtoResponse>>.Success(new List<PessoaDtoResponse>()));

            // Act
            var result = await _controller.GetByName(string.Empty);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }


        [Fact]
        public async void GetByName_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaAoTentarBuscarPessoaPeloNome()
        {
            // Arrange 
            _mockPessoaService.Setup(a => a.GetByName(It.IsAny<string>()))
                              .ReturnsAsync(Result<List<PessoaDtoResponse>>.Failure(string.Empty));

            // Act
            var result = await _controller.GetByName(string.Empty);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void GetByName_Deve_RetornarUmaException_AoTentarBuscarPessoaPeloNome()
        {
            // Arrange 
            _mockPessoaService.Setup(a => a.GetByName(It.IsAny<string>()))
                              .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetByName(string.Empty));

            // Assert
            Assert.Equal("Error: Internal Server Error ao buscar Pessoa pelo Nome!", exception.Message);
        }
        #endregion
    }
}