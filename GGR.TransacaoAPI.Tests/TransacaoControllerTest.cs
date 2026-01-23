using GR.TransacaoAPI.Controllers;
using GR.TransacaoAPI.Service;
using GR.Shared.Infra.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Shared.Result.ResultMessage;

namespace GGR.TransacaoAPI.Tests
{
    public class TransacaoControllerTest
    {
        private readonly Mock<ITransacaoService> _mockTransacaoService;
        private readonly Mock<TransacaoDtoRequest> _mockTransacaoDtoRequest;
        private readonly TransacaoController _controller;

        public TransacaoControllerTest()
        {
            _mockTransacaoService = new Mock<ITransacaoService>();
            _mockTransacaoDtoRequest = new Mock<TransacaoDtoRequest>();
            _controller = new TransacaoController(_mockTransacaoService.Object);
        }

        #region EndPointt Create

        [Fact]
        public void Create_Deve_RetornarUmBadRequest400_QuandoModelStateForInvalido()
        {
            // Arrange
            _controller.ModelState.AddModelError("Valor", "O atributo Valor é Obrigatório");
            _controller.ModelState.AddModelError("Tipo", "O atributo Tipo é Obrigatório");
            _controller.ModelState.AddModelError("PessoaId", "O atributo PessoaId é Obrigatório");
            _controller.ModelState.AddModelError("CategoriaId", "O atributo CategoriaId é Obrigatório");

            // Act
            var result = _controller.Create(_mockTransacaoDtoRequest.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.True(_controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public async void Create_Deve_RetornarUmCreatedResult201_QuandoCategoriaForTransacaoComSucesso()
        {
            // Arrange  
            _mockTransacaoService.Setup(a => a.CreateAsync(It.IsAny<TransacaoDtoRequest>()))
                                 .ReturnsAsync(Result<TransacaoDtoResponse>.Success(new TransacaoDtoResponse()));

            // Act 
            var result = await _controller.Create(new TransacaoDtoRequest());

            // Assert
            var createResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal(201, createResult.StatusCode);
        }

        [Fact]
        public void Create_Deve_RetornarUmBadRequest400_AoTentarCriarUmaTransacao()
        {
            // Arrange
            _mockTransacaoService.Setup(a => a.CreateAsync(It.IsAny<TransacaoDtoRequest>()))
                                 .ReturnsAsync(Result<TransacaoDtoResponse>.Failure(string.Empty));

            // Act
            var result = _controller.Create(_mockTransacaoDtoRequest.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);

        }

        [Fact]
        public async void Create_Deve_RetornarUmaException_AoTentarCriarUmaTransacao()
        {
            // Arrange  
            _mockTransacaoService.Setup(r => r.CreateAsync(It.IsAny<TransacaoDtoRequest>()))
                                 .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.Create(new TransacaoDtoRequest()));

            // Assert
            Assert.Equal("Error: Internal Server Error ao criar Transacao!", exception.Message);
        }

        #endregion

        #region EndPoint List

        [Fact]
        public async void List_Deve_RetornarOk200_AoListarTransacoes()
        {
            // Arrange  
            _mockTransacaoService.Setup(a => a.GetAllAsync())
                                 .ReturnsAsync(Result<List<TransacaoDtoResponse>>.Success(new List<TransacaoDtoResponse>()));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaAoTentarListarTransacoes()
        {
            // Arrange 
            _mockTransacaoService.Setup(a => a.GetAllAsync())
                                 .ReturnsAsync(Result<List<TransacaoDtoResponse>>.Failure(string.Empty));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmaException_AoTentarListarTransacoes()
        {
            // Arrange 
            _mockTransacaoService.Setup(a => a.GetAllAsync())
                                 .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetAll());

            // Assert
            Assert.Equal("Error: Internal Server Error ao listar Transações!", exception.Message);
        }

        #endregion

        #region EndPoint GetNetBalancePerson

        [Fact]
        public async void List_Deve_RetornarOk200_AoListarSaldoPorPessoa()
        {
            // Arrange  
            _mockTransacaoService.Setup(a => a.GetNetBalancePerson())
                                 .ReturnsAsync(Result<List<SaldoLiquidoDtoResponse>>.Success(new List<SaldoLiquidoDtoResponse>()));

            // Act
            var result = await _controller.GetNetBalancePerson();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaAoTentarSaldoPorPessoa()
        {
            // Arrange 
            _mockTransacaoService.Setup(a => a.GetNetBalancePerson())
                                 .ReturnsAsync(Result<List<SaldoLiquidoDtoResponse>>.Failure(string.Empty));

            // Act
            var result = await _controller.GetNetBalancePerson();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmaException_AoTentarListarSaldoPorPessoa()
        {
            // Arrange 
            _mockTransacaoService.Setup(a => a.GetNetBalancePerson())
                              .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetNetBalancePerson());

            // Assert
            Assert.Equal("Error: Internal Server Error ao listar Saldo!", exception.Message);
        }

        #endregion

        #region EndPoint GetNetBalanceCategory

        [Fact]
        public async void List_Deve_RetornarOk200_AoListarSaldoPorCategoria()
        {
            // Arrange  
            _mockTransacaoService.Setup(a => a.GetNetBalanceCategory())
                                 .ReturnsAsync(Result<List<SaldoLiquidoDtoResponse>>.Success(new List<SaldoLiquidoDtoResponse>()));

            // Act
            var result = await _controller.GetNetBalanceCategory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaAoTentarSaldoPorCategoria()
        {
            // Arrange 
            _mockTransacaoService.Setup(a => a.GetNetBalanceCategory())
                                 .ReturnsAsync(Result<List<SaldoLiquidoDtoResponse>>.Failure(string.Empty));

            // Act
            var result = await _controller.GetNetBalanceCategory();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmaException_AoTentarListarSaldoPorCategoria()
        {
            // Arrange 
            _mockTransacaoService.Setup(a => a.GetNetBalanceCategory())
                              .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetNetBalanceCategory());

            // Assert
            Assert.Equal("Error: Internal Server Error ao listar Saldo!", exception.Message);
        }

        #endregion
    }
}
