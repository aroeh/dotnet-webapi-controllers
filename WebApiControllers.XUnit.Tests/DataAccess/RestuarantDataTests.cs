using MongoDB.Driver;
using WebApiControllers.DataAccess;

namespace WebApiControllers.XUnit.Tests.DataAccess
{
    public class RestuarantDataTests
    {
        private readonly Mock<ILogger<RestuarantData>> mockLogger;
        private readonly Mock<IDatabaseWrapper> mockMongoService;
        private readonly RestuarantData data;

        public RestuarantDataTests()
        {
            mockLogger = new Mock<ILogger<RestuarantData>>();
            mockMongoService = new Mock<IDatabaseWrapper>();

            data = new RestuarantData(mockLogger.Object, mockMongoService.Object);
        }

        [Fact]
        public async Task GetAllRestuarants_NoData_ReturnsEmptyCollection()
        {
            // arrange
            List<Restuarant> mockResponse = [];
            mockMongoService.Setup(m => m.FindMany<Restuarant>(It.IsAny<string>(), It.IsAny<FilterDefinition<Restuarant>>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await data.GetAllRestuarants();

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
            var testResult = await data.GetAllRestuarants();

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
            var testResult = await data.FindRestuarants(name, cuisine);

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
            var testResult = await data.FindRestuarants(name, cuisine);

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
            var testResult = await data.GetRestuarant(id);

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
            var testResult = await data.GetRestuarant(id);

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
            var testResult = await data.InsertRestuarant(mockRequest);

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
            var testResult = await data.UpdateRestuarant(mockRequest);

            // assert
            Assert.NotNull(testResult);
            Assert.True(testResult.IsAcknowledged);
            Assert.True(testResult.ModifiedCount > 0);
        }
    }
}