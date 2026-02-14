using GGR.CategoriaAPI.Controllers;
using GGR.CategoriaAPI.Service;
using GGR.Shared.Infra.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Shared.Result.ResultMessage;

namespace GGR.CategoriaAPI.Tests
{
    public class CategoriaControllerTest
    {
        private readonly Mock<ICategoriaService> _mockCategoriaService;
        private readonly Mock<CategoriaDtoRequest> _mockCategoriaDtoRequest;
        private readonly CategoriaController _controller;

        public CategoriaControllerTest()
        {
            _mockCategoriaService = new Mock<ICategoriaService>();
            _mockCategoriaDtoRequest = new Mock<CategoriaDtoRequest>();
            _controller = new CategoriaController(_mockCategoriaService.Object);
        }

        #region EndPointt Create

        [Fact]
        public void Create_Deve_RetornarUmBadRequest400_QuandoModelStateForInvalido()
        {
            // Arrange
            _controller.ModelState.AddModelError("Descricao", "O atributo Descricao é Obrigatório");
            _controller.ModelState.AddModelError("FinalidadeCategoria", "O atributo FinalidadeCategoria é Obrigatório");

            // Act
            var result = _controller.Create(_mockCategoriaDtoRequest.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.True(_controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public async void Create_Deve_RetornarUmCreatedResult201_QuandoCategoriaForCriadaComSucesso()
        {
            // Arrange  
            _mockCategoriaService.Setup(a => a.CreateAsync(It.IsAny<CategoriaDtoRequest>()))
                                 .ReturnsAsync(Result<CategoriaDtoResponse>.Success(new CategoriaDtoResponse()));

            // Act 
            var result = await _controller.Create(new CategoriaDtoRequest());

            // Assert
            var createResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal(201, createResult.StatusCode);
        }

        [Fact]
        public void Create_Deve_RetornarUmBadRequest400_AoTentarCriarUmaCategoria()
        {
            // Arrange
            _mockCategoriaService.Setup(a => a.CreateAsync(It.IsAny<CategoriaDtoRequest>()))
                                 .ReturnsAsync(Result<CategoriaDtoResponse>.Failure(string.Empty));

            // Act
            var result = _controller.Create(_mockCategoriaDtoRequest.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);

        }

        [Fact]
        public async void Create_Deve_RetornarUmaException_AoTentarCriarUmaCategoria()
        {
            // Arrange  
            _mockCategoriaService.Setup(r => r.CreateAsync(It.IsAny<CategoriaDtoRequest>()))
                                 .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.Create(new CategoriaDtoRequest()));

            // Assert
            Assert.Equal("Error: Internal Server Error ao criar Categoria!", exception.Message);
        }

        #endregion

        #region EndPoint List

        [Fact]
        public async void List_Deve_RetornarOk200_AoListarCategorias()
        {
            // Arrange  
            _mockCategoriaService.Setup(a => a.GetAllAsync())
                                 .ReturnsAsync(Result<List<CategoriaDtoResponse>>.Success(new List<CategoriaDtoResponse>()));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaAoTentarListarCategorias()
        {
            // Arrange 
            _mockCategoriaService.Setup(a => a.GetAllAsync())
                                 .ReturnsAsync(Result<List<CategoriaDtoResponse>>.Failure(string.Empty));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void List_Deve_RetornarUmaException_AoTentarListarCategorias()
        {
            // Arrange 
            _mockCategoriaService.Setup(a => a.GetAllAsync())
                                 .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetAll());

            // Assert
            Assert.Equal("Error: Internal Server Error ao listar Categorias!", exception.Message);
        }

        #endregion

        #region EndPoint GetByDescription

        [Fact]
        public async void GetByDescription_Deve_RetornarOk200_AoBuscarCategoriaPelaDescricao()
        {
            // Arrange  
            _mockCategoriaService.Setup(a => a.GetByDescription(It.IsAny<string>()))
                                 .ReturnsAsync(Result<List<CategoriaDtoResponse>>.Success(new List<CategoriaDtoResponse>()));

            // Act
            var result = await _controller.GetByDescription(string.Empty);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }


        [Fact]
        public async void GetByDescription_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaAoTentarBuscarCategoriaPelaDescricao()
        {
            // Arrange 
            _mockCategoriaService.Setup(a => a.GetByDescription(It.IsAny<string>()))
                                 .ReturnsAsync(Result<List<CategoriaDtoResponse>>.Failure(string.Empty));

            // Act
            var result = await _controller.GetByDescription(string.Empty);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void GetByDescription_Deve_RetornarUmaException_AoTentarBuscarCategoriaPelaDescricao()
        {
            // Arrange 
            _mockCategoriaService.Setup(a => a.GetByDescription(It.IsAny<string>()))
                                 .ThrowsAsync(new Exception("Internal Server Error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetByDescription(string.Empty));

            // Assert
            Assert.Equal("Error: Internal Server Error ao listar Categorias!", exception.Message);
        }

        #endregion
    }
}
