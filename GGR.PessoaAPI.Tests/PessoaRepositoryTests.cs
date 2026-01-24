//using GR.PessoaAPI.Controllers;
//using GR.PessoaAPI.Service;
//using GR.Shared.Infra.Data;
//using GR.Shared.Infra.DTO;
//using GR.Shared.Infra.Model;
//using GR.Shared.Infra.Repository;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System;
////using System.Data.Entity;
//using Xunit;
//using static Shared.Result.ResultMessage;

//namespace GGR.PessoaAPI.Tests
//{
//    public class PessoaRepositoryTests
//    {
//        private readonly Mock<DbSet<Pessoa>> _mockPessoaRepository;
//        private readonly Mock<MySQLContext>  _mockPessoaContext;
        
//        public PessoaRepositoryTests()
//        {
//            _mockPessoaContext = new Mock<MySQLContext>();
//            _mockPessoaRepository = new Mock<DbSet<Pessoa>>();
//        }

//        //private static PessoaRepository CreatePessoaRepository()
//        //{
//        //    var options = new DbContextOptionsBuilder<MySQLContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid()
//        //                                                             .ToString()).Options;

//        //    var context = new MySQLContext(options);
//        //    var logger = new Mock<ILogger<PessoaRepository>>();

//        //    return new PessoaRepository(context, logger.Object);
//        //}

//        #region EndPointt Create

//        [Fact]
//        public async Task CreateAsync_Deve_LancarExcecao_QuandoSaveChangesFalhar()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<MySQLContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;

//            var context = new MySQLContext(options);

//            var loggerMock = new Mock<ILogger<PessoaRepository>>();
//            var repository = new PessoaRepository(context, loggerMock.Object);

//            var pessoa = MockPessoaRequest();

//            // Força erro simulando SaveChanges
//            context.Database.EnsureDeleted();

//            // Act & Assert
//            var exception = await Assert.ThrowsAsync<Exception>(() => repository.CreateAsync(pessoa));

//            Assert.Contains("Erro ao criar", exception.Message);
//        }



//        [Fact]
//        //public async Task CreateAsync_Deve_RetornarIsSuccessQuandoPessoaForCriadaComSucess()
//        public async Task CreateAsync_Deve_LancarExcecao_QuandoSaveChangesFalhar_()
//        {
//            // Arrange   
//            _mockPessoaContext.Setup(p => p.Pessoas)
//                              .Returns(_mockPessoaRepository.Object);

//            _mockPessoaContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
//                              .ThrowsAsync(new Exception("Error ao criar uma Pessoa no banco de Dados!"));

//            var loggerMock = new Mock<ILogger<PessoaRepository>>();
//            var pessoaRepository = new PessoaRepository(_mockPessoaContext.Object, loggerMock.Object);
//            var pessoa = MockPessoaRequest();

//            // Assert
//            var exception = await Assert.ThrowsAsync<Exception>(() => pessoaRepository.CreateAsync(pessoa));

//            Assert.Equal("Erro ao criar uma Pessoa no banco de Dados!", exception.Message );
//        }

//        //[Fact]
//        //public async Task CreateAsync_Deve_LancarExcecao_QuandoSaveChangesFalhar()
//        //{
//        //    // Arrange
//        //    var pessoa = MockPessoaRequest();

//        //    var contextMock = new Mock<MySQLContext>();
//        //    var pessoasMock = new Mock<DbSet<Pessoa>>();

//        //    contextMock
//        //        .Setup(c => c.Pessoas)
//        //        .Returns(pessoasMock.Object);

//        //    contextMock
//        //        .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
//        //        .ThrowsAsync(new Exception("Erro no banco"));

//        //    var loggerMock = new Mock<ILogger<PessoaRepository>>();

//        //    var repository = new PessoaRepository(
//        //        contextMock.Object,
//        //        loggerMock.Object
//        //    );

//        //    // Act & Assert
//        //    var exception = await Assert.ThrowsAsync<Exception>(
//        //        () => repository.CreateAsync(pessoa)
//        //    );

//        //    Assert.Equal(
//        //        "Erro ao criar uma Pessoa no banco de Dados!",
//        //        exception.Message
//        //    );
//        //}



//        //[Fact]
//        //public async Task GetAllAsync_Deve_RetornarUmaExcption_QuandoBustarPorTodasAsPessoas()
//        //{
//        //    // Arrange 
//        //    _mockMapper.Setup(m => m.Map<Pessoa>(It.IsAny<PessoaDtoRequest>()))
//        //               .Returns(MockPessoaRequest());

//        //    _mockPessoaRepository.Setup(r => r.GetAllAsync())
//        //                         .ThrowsAsync(new Exception("Falha ao listar Pessoas!"));

//        //    _mockMapper.Setup(m => m.Map<PessoaDtoResponse>(It.IsAny<Pessoa>()))
//        //               .Returns(MockPessoaDtoResponse());

//        //    var pessoaService = new PessoaService(_mockPessoaRepository.Object, _mockMapper.Object);

//        //    // Act
//        //    var exception = await Assert.ThrowsAsync<Exception>(() => pessoaService.GetAllAsync());

//        //    // Assert 
//        //    Assert.Equal("Falha ao listar Pessoas!", exception.Message);
//        //}


//        [Fact]
//        public async void CreateAsync_Deve_RetornarUmaExcption_QuandoPessoaForCriarUmaPessoa()
//        {
//            //$"Error ao criar uma Pessoa no banco de Dados!"

//            // Arrange 

//            // Act 

//            // Assert

//        }

//        [Fact]
//        public async Task CreateAsync_Deve_LancarExcecao_QuandoForSalvarUmaPessoa()
//        {
//            // Act 
           
//            // Arrange 

//            // Assert
          
//        }

//        #endregion

//        #region EndPoint Delete

//        [Fact]
//        public async Task CreateAsync_Deve_LancarExcecao_QuandoForDeletarUmaPessoa()
//        {
//            // Act 
           
//            // Arrange 
          

//            // Assert
          
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


//        private static PessoaDtoRequest MockPessoaDtoRequest()
//        {
//            return new PessoaDtoRequest
//            {
//                Nome = "João Alfredo Cunha",
//                Idade = 35
//            };
//        }

//        private static Pessoa MockPessoaRequest()
//        {
//            return new Pessoa
//            {
//                Nome = "João Alfredo Cunha",
//                Idade = 35
//            };
//        }

//        private static PessoaDtoResponse MockPessoaDtoResponse()
//        {
//            return new PessoaDtoResponse
//            {
//                Id = Guid.NewGuid(),
//                Nome = "João Alfredo Cunha",
//                Idade = 35
//            };
//        }
//    }
//}
