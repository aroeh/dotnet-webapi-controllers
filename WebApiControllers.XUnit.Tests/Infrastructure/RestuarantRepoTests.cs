namespace WebApiControllers.XUnit.Tests.Infrastructure;

public class RestuarantRepoTests
{
    private readonly Mock<ILogger<RestuarantRepo>> mockLogger;
    private readonly Mock<IDatabaseWrapper> mockMongoService;
    private readonly RestuarantRepo repo;

    public RestuarantRepoTests()
    {
        mockLogger = new Mock<ILogger<RestuarantRepo>>();
        mockMongoService = new Mock<IDatabaseWrapper>();

        repo = new RestuarantRepo(mockLogger.Object, mockMongoService.Object);
    }

    [Fact]
    public async Task GetAllRestuarants_NoData_ReturnsEmptyCollection()
    {
        // arrange
        List<Restuarant> mockResponse = [];
        mockMongoService.Setup(m => m.FindMany<Restuarant>(It.IsAny<string>(), It.IsAny<FilterDefinition<Restuarant>>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await repo.GetAllRestuarants();

        // assert
        Assert.NotNull(testResult);
        Assert.Empty(testResult);
    }

    [Fact]
    public async Task GetAllRestuarants_HasData_ReturnsRestuarants()
    {
        // arrange
        List<Restuarant> mockResponse =
        [
            new(),
                new()
        ];
        mockMongoService.Setup(m => m.FindMany<Restuarant>(It.IsAny<string>(), It.IsAny<FilterDefinition<Restuarant>>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await repo.GetAllRestuarants();

        // assert
        Assert.NotNull(testResult);
        Assert.NotEmpty(testResult);
    }

    [Fact]
    public async Task FindRestuarants_NoMatches_ReturnsEmptyCollection()
    {
        // arrange
        string name = "test";
        string cuisine = "test";
        List<Restuarant> mockResponse = [];
        mockMongoService.Setup(m => m.FindMany<Restuarant>(It.IsAny<string>(), It.IsAny<FilterDefinition<Restuarant>>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await repo.FindRestuarants(name, cuisine);

        // assert
        Assert.NotNull(testResult);
        Assert.Empty(testResult);
    }

    [Fact]
    public async Task FindRestuarants_MatchesFound_ReturnsRestuarants()
    {
        // arrange
        string name = "test";
        string cuisine = "test";
        List<Restuarant> mockResponse =
        [
            new(),
                new()
        ];
        mockMongoService.Setup(m => m.FindMany<Restuarant>(It.IsAny<string>(), It.IsAny<FilterDefinition<Restuarant>>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await repo.FindRestuarants(name, cuisine);

        // assert
        Assert.NotNull(testResult);
        Assert.NotEmpty(testResult);
    }

    [Fact]
    public async Task GetRestuarant_NotFound_ReturnsNewRestuarant()
    {
        // arrange
        string id = "123456";
        Restuarant mockResponse = new();
        mockMongoService.Setup(m => m.FindOne<Restuarant>(It.IsAny<string>(), It.IsAny<FilterDefinition<Restuarant>>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await repo.GetRestuarant(id);

        // assert
        Assert.NotNull(testResult);
        Assert.True(string.IsNullOrWhiteSpace(testResult.Id));
    }

    [Fact]
    public async Task GetRestuarant_RecordFound_ReturnsRestuarant()
    {
        // arrange
        string id = "123456";
        Restuarant mockResponse = new()
        {
            Id = id,
            Name = "Test",
            CuisineType = "Test"
        };
        mockMongoService.Setup(m => m.FindOne<Restuarant>(It.IsAny<string>(), It.IsAny<FilterDefinition<Restuarant>>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await repo.GetRestuarant(id);

        // assert
        Assert.NotNull(testResult);
        Assert.False(string.IsNullOrWhiteSpace(testResult.Id));
    }

    [Fact]
    public async Task InsertRestuarant_Success_ReturnsRestuarant()
    {
        // arrange
        Restuarant mockRequest = new()
        {
            Name = "Test",
            CuisineType = "Test"
        };
        Restuarant mockResponse = new()
        {
            Id = "123456",
            Name = "Test",
            CuisineType = "Test"
        };
        mockMongoService.Setup(m => m.InsertOne<Restuarant>(It.IsAny<string>(), It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await repo.InsertRestuarant(mockRequest);

        // assert
        Assert.NotNull(testResult);
        Assert.False(string.IsNullOrWhiteSpace(testResult.Id));
    }

    [Fact]
    public async Task UpdateRestuarant_Success_ReturnsRestuarant()
    {
        // arrange
        Restuarant mockRequest = new()
        {
            Id = "123456",
            Name = "Test",
            CuisineType = "Test"
        };
        MongoUpdateResult mockResponse = new()
        {
            IsAcknowledged = true,
            ModifiedCount = 1
        };
        mockMongoService.Setup(m => m.ReplaceOne<Restuarant>(It.IsAny<string>(), It.IsAny<FilterDefinition<Restuarant>>(), It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await repo.UpdateRestuarant(mockRequest);

        // assert
        Assert.NotNull(testResult);
        Assert.True(testResult.IsAcknowledged);
        Assert.True(testResult.ModifiedCount > 0);
    }
}
