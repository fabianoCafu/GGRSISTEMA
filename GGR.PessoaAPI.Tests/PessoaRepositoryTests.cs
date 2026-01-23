//using GR.PessoaAPI.Controllers;
//using GR.PessoaAPI.Service;
//using GR.Shared.Infra.DTO;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Shared.Result.ResultMessage;
//using Xunit;
//using GR.Shared.Infra.Repository;
//using GR.Shared.Infra.Model;
//using System.Data.Entity;
//using Microsoft.Extensions.Logging;

//namespace GGR.PessoaAPI.Tests
//{
//    public class PessoaRepositoryTests
//    {
//        private readonly Mock<IPessoaRespository> _mockPessoaRepository;
//        private readonly Mock<PessoaDtoRequest> _mockPessoaDtoRequest;
//        private readonly Mock<DbSet<Pessoa>> _mockPessoaDbSet; 
//        private readonly PessoaController _controller;

//        public PessoaRepositoryTests()
//        {
//            _mockPessoaRepository = new Mock<IPessoaRespository>();
//            _mockPessoaDtoRequest = new Mock<PessoaDtoRequest>();
//            _mockPessoaDbSet = new Mock<DbSet<Pessoa>>();
//        }

//        private static Mock<DbSet<Pessoa>> CriarMockDbSet()
//        {
//            var data = new List<Pessoa>().AsQueryable();

//            var mockSet = new Mock<DbSet<Pessoa>>();
//            mockSet.As<IQueryable<Pessoa>>().Setup(m => m.Provider).Returns(data.Provider);
//            mockSet.As<IQueryable<Pessoa>>().Setup(m => m.Expression).Returns(data.Expression);
//            mockSet.As<IQueryable<Pessoa>>().Setup(m => m.ElementType).Returns(data.ElementType);
//            mockSet.As<IQueryable<Pessoa>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

//            return mockSet;
//        }


//        #region EndPointt Create

//        [Fact]
//        public async Task CreateAsync_Deve_Retornar_Success_Quando_Pessoa_For_Criada()
//        {
//            // Arrange
//            var pessoa = MockPessoaPost(); 
//            var mockSet = CriarMockDbSet();

//            var mockContext = new Mock<AppDbContext>();
//            mockContext.Setup(c => c.Pessoas).Returns(mockSet.Object);
//            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
//                       .ReturnsAsync(1);

//            var mockLogger = new Mock<ILogger<PessoaRepository>>();

//            var repository = new PessoaRepository(mockContext.Object, mockLogger.Object);

//            // Act
//            var result = await repository.CreateAsync(pessoa);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.NotNull(result.Value);
//            Assert.Equal("Fabiano", result.Value.Nome);

//            mockSet.Verify(s => s.Add(It.IsAny<Pessoa>()), Times.Once);
//            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//        }

//        [Fact]
//        public async void CreateAsync_Deve_RetornarIsSuccess_QuandoPessoaForCriadaComSucesso()
//        {
//            // Arrange 
//            var pessoaPost = MockPessoaPost();
//            var mockSet = new Mock<DbSet<Pessoa>>();

//            _mockPessoaRepository.Setup(a => a.CreateAsync(It.IsAny<Pessoa>()))
//                                 .ReturnsAsync(Result<Pessoa>.Success(new Pessoa()));

//            // Act 
//            var result = await _mockPessoaRepository.Object.CreateAsync(pessoaPost);

//            // Assert
//            Assert.True(result.IsSuccess);
//        }

//        [Fact]
//        public async Task CreateAsync_Deve_LancarExcecao_QuandoForSalvarUmaPessoa()
//        {
//            // Act 
//            _mockPessoaRepository.Setup(r => r.CreateAsync(It.IsAny<Pessoa>()))
//                                 .ThrowsAsync(new Exception("Internal Server Error"));

//            // Arrange 
//            var exception = await Assert.ThrowsAsync<Exception>(() => _mockPessoaRepository.Object.CreateAsync(new Pessoa()));
            
//            // Assert
//            Assert.Equal("Internal Server Error", exception.Message);
//        }

//        #endregion

//        #region EndPoint Delete

//        [Fact]
//        public async Task CreateAsync_Deve_LancarExcecao_QuandoForDeletarUmaPessoa()
//        {
//            // Act 
//            _mockPessoaRepository.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
//                                 .ThrowsAsync(new Exception("Internal Server Error"));

//            // Arrange 
//            var exception = await Assert.ThrowsAsync<Exception>(() => _mockPessoaRepository.Object.DeleteAsync(new Guid()));

//            // Assert
//            Assert.Equal("Internal Server Error", exception.Message);
//        }

//        #endregion

//        #region EndPoint List

//        //[Fact]
//        //public async void List_Deve_RetornarOk200_AoListarPessoas()
//        //{
//        //    // Arrange  
//        //    _mockPessoaService.Setup(a => a.GetAllAsync())
//        //                      .ReturnsAsync(Result<List<PessoaDtoResponse>>.Success(new List<PessoaDtoResponse>()));

//        //    // Act
//        //    var result = await _controller.GetAll();

//        //    // Assert
//        //    var okResult = Assert.IsType<OkObjectResult>(result.Result);
//        //    Assert.Equal(200, okResult.StatusCode);
//        //}

//        //[Fact]
//        //public async void List_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaAoTentarListarPessoas()
//        //{
//        //    // Arrange 
//        //    _mockPessoaService.Setup(a => a.GetAllAsync())
//        //                      .ReturnsAsync(Result<List<PessoaDtoResponse>>.Failure(string.Empty));

//        //    // Act
//        //    var result = await _controller.GetAll();

//        //    // Assert
//        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
//        //    Assert.Equal(400, badRequestResult.StatusCode);
//        //}

//        //[Fact]
//        //public async void List_Deve_RetornarUmaException_AoTentarListarPessoas()
//        //{
//        //    // Arrange 
//        //    _mockPessoaService.Setup(a => a.GetAllAsync())
//        //                      .ThrowsAsync(new Exception("Internal Server Error"));

//        //    // Act
//        //    var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetAll());

//        //    // Assert
//        //    Assert.Equal("Error: Internal Server Error ao listar Pessoas!", exception.Message);
//        //}

//        #endregion

//        #region EndPoint GetByName

//        //[Fact]
//        //public async void GetByName_Deve_RetornarOk200_AoBuscarPessoaPeloNome()
//        //{
//        //    // Arrange  
//        //    _mockPessoaService.Setup(a => a.GetByName(It.IsAny<string>()))
//        //                      .ReturnsAsync(Result<List<PessoaDtoResponse>>.Success(new List<PessoaDtoResponse>()));

//        //    // Act
//        //    var result = await _controller.GetByName(string.Empty);

//        //    // Assert
//        //    var okResult = Assert.IsType<OkObjectResult>(result.Result);
//        //    Assert.Equal(200, okResult.StatusCode);
//        //}


//        //[Fact]
//        //public async void GetByName_Deve_RetornarUmBadRequest400_QuandoHouverUmaFalhaAoTentarBuscarPessoaPeloNome()
//        //{
//        //    // Arrange 
//        //    _mockPessoaService.Setup(a => a.GetByName(It.IsAny<string>()))
//        //                      .ReturnsAsync(Result<List<PessoaDtoResponse>>.Failure(string.Empty));

//        //    // Act
//        //    var result = await _controller.GetByName(string.Empty);

//        //    // Assert
//        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
//        //    Assert.Equal(400, badRequestResult.StatusCode);
//        //}

//        //[Fact]
//        //public async void GetByName_Deve_RetornarUmaException_AoTentarBuscarPessoaPeloNome()
//        //{
//        //    // Arrange 
//        //    _mockPessoaService.Setup(a => a.GetByName(It.IsAny<string>()))
//        //                      .ThrowsAsync(new Exception("Internal Server Error"));

//        //    // Act
//        //    var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetByName(string.Empty));

//        //    // Assert
//        //    Assert.Equal("Error: Internal Server Error ao buscar Pessoa pelo Nome!", exception.Message);
//        //}

//        #endregion


//        private static Pessoa MockPessoaPost()
//        {
//            return new Pessoa
//            {
//                Nome = "Camila Azevedo do Santos",
//                Idade = 35
//            };
//        }

//        private static Pessoa MockPessoaRequest()
//        {
//            return new Pessoa
//            {
//                Id = Guid.NewGuid(),
//                Nome = "Fernando Carlos da Rosa Pereira",
//                Idade = 35
//            };
//        }
//    }
//}
