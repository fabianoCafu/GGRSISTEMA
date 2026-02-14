using AutoMapper;
using GGR.CategoriaAPI.Service;
using GGR.Shared.Infra.DTO;
using GGR.Shared.Infra.Model;
using GGR.Shared.Infra.Repository;
using Moq;
using Xunit;
using static Shared.Aplication.Enum.Enums;
using static Shared.Result.ResultMessage;


namespace GGR.CategoriaAPI.Tests
{
    public class CategoriaServiceTest
    {
        private readonly Mock<ICategoriaRepository> _mockCategoriaRepository;
        private readonly Mock<IMapper> _mockMapper;

        public CategoriaServiceTest()
        {
            _mockCategoriaRepository = new Mock<ICategoriaRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        #region EndPoint Create

        [Fact]
        public async Task Create_Deve_RetornarUmFailure_QuandoCategoriaDtoRequest_ForIgualAhNull()
        {
            // Arrange
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.CreateAsync(It.IsAny<Categoria>()))
                                 .ReturnsAsync(Result<Categoria>.Success(MockCategoriaRequest()));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);

            // Act
            var result = await categoriaService.CreateAsync(null!);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Falha a categoriaDtoRequest deve ser diferente de null!", result.Error);
        }

        [Fact]
        public async Task Create_Deve_RetornarUmIsFailure_QuandoHouverUmaFalhaNaCriacaoDaCategoria()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.CreateAsync(It.IsAny<Categoria>()))
                                    .ReturnsAsync(Result<Categoria>.Failure(string.Empty));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);
            var categoriaRequest = MockCategoriaDtoRequest();

            // Act
            var result = await categoriaService.CreateAsync(categoriaRequest);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Falha ao cadastrar uma Categoria!", result.Error);
        }

        [Fact]
        public async Task Create_Deve_RetornarUmIsSuccess_QuandoCategoriaForCriadaComSucesso()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.CreateAsync(It.IsAny<Categoria>()))
                                    .ReturnsAsync(Result<Categoria>.Success(MockCategoriaRequest()));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);
            var categoriaRequest = MockCategoriaDtoRequest();

            // Act
            var result = await categoriaService.CreateAsync(categoriaRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public async Task Create_Deve_RetornarUmaExcption_QuandoHouverUmaFalhaNaCriacaoDaCategoria()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.CreateAsync(It.IsAny<Categoria>()))
                                 .ThrowsAsync(new Exception(string.Empty));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);
            var categoriaRequest = MockCategoriaDtoRequest();

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => categoriaService.CreateAsync(categoriaRequest));

            // Assert 
            Assert.Equal("Error ao cadastra uma Categoria!", exception.Message);
        }

        #endregion

        #region EndPoint List

        [Fact]
        public async Task List_Deve_RetornarUmIsSuccess_QuandoAhBuscaPorTodasAsCategoriasForRealizadaComSucesso()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.GetAllAsync())
                                    .ReturnsAsync(Result<List<Categoria>>.Success(new List<Categoria>()));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);

            // Act
            var result = await categoriaService.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public async Task List_Deve_RetornarUmIsFailure_QuandoBustarPorTodasAsCategorias()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.GetAllAsync())
                                    .ReturnsAsync(Result<List<Categoria>>.Failure(string.Empty));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);

            // Act
            var result = await categoriaService.GetAllAsync();

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal("Falha ao listar Categorias!", result.Error);
        }

        [Fact]
        public async Task List_Deve_RetornarUmaExcption_QuandoBustarPorTodasAsCategorias()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.GetAllAsync())
                                    .ThrowsAsync(new Exception(string.Empty));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => categoriaService.GetAllAsync());

            // Assert 
            Assert.Equal("Error ao listar Categorias!", exception.Message);
        }

        #endregion

        #region EndPoint GetByDescription

        [Fact]
        public async Task GetByDescription_Deve_RetornarUmIsSuccess_QuandoAhBuscaPorUmaCategoriaForRealizadaComSucesso()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.GetByDescription(It.IsAny<string>()))
                                    .ReturnsAsync(Result<List<Categoria>>.Success(new List<Categoria>()));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);
            var categoriaResponse = MockCategoriaDtoResponse();

            // Act
            var result = await categoriaService.GetByDescription(categoriaResponse.Descricao!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
        }

        [Fact]
        public async Task GetByDescription_Deve_RetornarUmIsFailure_QuandoCategoriaDtoRequestForIgualAhNull()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.GetByDescription(It.IsAny<string>()))
                                    .ReturnsAsync(Result<List<Categoria>>.Failure(string.Empty));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);
            
            // Act
            var result = await categoriaService.GetByDescription(null!);

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal("Falha a Descrição da Categoria deve ser informado!", result.Error);
        }

        [Fact]
        public async Task GetByDescription_Deve_RetornarUmaExcption_QuandoRealizadoUmaBuscaDaCategoriaPelaDescricao()
        {
            // Arrange 
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDtoRequest>()))
                       .Returns(MockCategoriaRequest());

            _mockCategoriaRepository.Setup(r => r.GetByDescription(It.IsAny<string>()))
                                    .ThrowsAsync(new Exception(string.Empty));

            _mockMapper.Setup(m => m.Map<CategoriaDtoResponse>(It.IsAny<Categoria>()))
                       .Returns(MockCategoriaDtoResponse());

            var categoriaService = new CategoriaService(_mockCategoriaRepository.Object, _mockMapper.Object);
            var categoriaResponse = MockCategoriaDtoResponse();

            // Act 
            var exception = await Assert.ThrowsAsync<Exception>(() => categoriaService.GetByDescription(categoriaResponse.Descricao!));

            // Assert 
            Assert.Equal("Error ao buscar uma Categoria pela Descrição!", exception.Message);
        }

        #endregion

        #region Metodos Private
        private static CategoriaDtoRequest MockCategoriaDtoRequest()
        {
            return new CategoriaDtoRequest
            {
                Descricao = "Prolabore",
                Finalidade = FinalidadeCategoria.Receita 
            };
        }

        private static Categoria MockCategoriaRequest()
        {
            return new Categoria
            { 
                Descricao = "Prolabore",
                Finalidade = FinalidadeCategoria.Receita,
                DataCriacaoRegistro = DateTime.Now
            };
        }

        private static CategoriaDtoResponse MockCategoriaDtoResponse()
        {
            return new CategoriaDtoResponse
            {
                Id  = Guid.NewGuid(),
                Descricao = "Prolabore",
                Finalidade = FinalidadeCategoria.Receita,
                DataCriacaoRegistro = DateTime.Now
            };
        }

        #endregion
    }
}
