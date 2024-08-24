using WebApiControllers.DataAccess;

namespace WebApiControllers.XUnit.Tests.Repos
{
    public class RestuarantRepoTests
    {
        private readonly Mock<ILogger<RestuarantRepo>> mockLogger;
        private readonly Mock<IRestuarantData> mockData;
        private readonly RestuarantRepo repo;

        public RestuarantRepoTests()
        {
            mockLogger = new Mock<ILogger<RestuarantRepo>>();
            mockData = new Mock<IRestuarantData>();
            repo = new(mockLogger.Object, mockData.Object);
        }

        [Fact]
        public async Task GetAllRestuarants_NoneFound_ReturnsEmptyCollection()
        {
            // arrange
            List<Restuarant> mockDataResponse = null;
            mockData.Setup(d => d.GetAllRestuarants()).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.GetAllRestuarants();

            // assert
            Assert.NotNull(testResult);
            Assert.Empty(testResult);
        }

        [Fact]
        public async Task GetAllRestuarants_EmptyQueryResults_ReturnsEmptyCollection()
        {
            // arrange
            List<Restuarant> mockDataResponse = [];
            mockData.Setup(d => d.GetAllRestuarants()).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.GetAllRestuarants();

            // assert
            Assert.NotNull(testResult);
            Assert.Empty(testResult);
        }

        [Fact]
        public async Task GetAllRestuarants_HasData_ReturnsRestuarantCollection()
        {
            // arrange
            List<Restuarant> mockDataResponse =
            [
                new(),
                new()
            ];
            mockData.Setup(d => d.GetAllRestuarants()).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.GetAllRestuarants();

            // assert
            Assert.NotNull(testResult);
            Assert.NotEmpty(testResult);
        }

        [Fact]
        public async Task FindRestuarants_NoneFound_ReturnsEmptyCollection()
        {
            // arrange
            string name = "test";
            string cuisine = "test";
            List<Restuarant> mockDataResponse = null;
            mockData.Setup(d => d.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.FindRestuarants(name, cuisine);

            // assert
            Assert.NotNull(testResult);
            Assert.Empty(testResult);
        }

        [Fact]
        public async Task FindRestuarants_EmptyQueryResults_ReturnsEmptyCollection()
        {
            // arrange
            string name = "test";
            string cuisine = "test";
            List<Restuarant> mockDataResponse = [];
            mockData.Setup(d => d.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.FindRestuarants(name, cuisine);

            // assert
            Assert.NotNull(testResult);
            Assert.Empty(testResult);
        }

        [Fact]
        public async Task FindRestuarants_HasData_ReturnsRestuarantCollection()
        {
            // arrange
            string name = "test";
            string cuisine = "test";
            List<Restuarant> mockDataResponse =
            [
                new(),
                new()
            ];
            mockData.Setup(d => d.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockDataResponse);

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
            Restuarant mockDataResponse = null;
            mockData.Setup(d => d.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.GetRestuarant(id);

            // assert
            Assert.NotNull(testResult);
            Assert.True(string.IsNullOrWhiteSpace(testResult.Id));
        }

        [Fact]
        public async Task GetRestuarant_Exists_ReturnsRestuarant()
        {
            // arrange
            string id = "123456";
            Restuarant mockDataResponse = new()
            {
                Id = id,
                Name = "test",
                CuisineType = "test"
            };
            mockData.Setup(d => d.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.GetRestuarant(id);

            // assert
            Assert.NotNull(testResult);
            Assert.False(string.IsNullOrWhiteSpace(testResult.Id));
        }

        [Fact]
        public async Task InsertRestuarant_FailsWithNull_ReturnsFalse()
        {
            // arrange

            Restuarant mockDataRequest = new()
            {
                Name = "test",
                CuisineType = "test"
            };
            Restuarant mockDataResponse =null;

            mockData.Setup(d => d.InsertRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.InsertRestuarant(mockDataRequest);

            // assert
            Assert.False(testResult);
        }

        [Fact]
        public async Task InsertRestuarant_FailsWithNoId_ReturnsFalse()
        {
            // arrange
            Restuarant mockDataRequest = new()
            {
                Name = "test",
                CuisineType = "test"
            };
            Restuarant mockDataResponse = new();

            mockData.Setup(d => d.InsertRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.InsertRestuarant(mockDataRequest);

            // assert
            Assert.False(testResult);
        }

        [Fact]
        public async Task InsertRestuarant_Success_ReturnsTrue()
        {
            // arrange
            Restuarant mockDataRequest = new()
            {
                Id = "123456",
                Name = "test",
                CuisineType = "test"
            };

            mockData.Setup(d => d.InsertRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockDataRequest);

            // act
            var testResult = await repo.InsertRestuarant(mockDataRequest);

            // assert
            Assert.True(testResult);
        }

        [Fact]
        public async Task UpdateRestuarant_FailsNotAcknowledged_ReturnsFalse()
        {
            // arrange
            Restuarant mockDataRequest = new()
            {
                Name = "test",
                CuisineType = "test"
            };
            MongoUpdateResult mockDataResponse = new()
            {
                IsAcknowledged = false,
                ModifiedCount = 0
            };

            mockData.Setup(d => d.UpdateRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.UpdateRestuarant(mockDataRequest);

            // assert
            Assert.False(testResult);
        }

        [Fact]
        public async Task UpdateRestuarant_FailsNoRecordsModified_ReturnsFalse()
        {
            // arrange
            Restuarant mockDataRequest = new()
            {
                Name = "test",
                CuisineType = "test"
            };
            MongoUpdateResult mockDataResponse = new()
            {
                IsAcknowledged = true,
                ModifiedCount = 0
            };
            mockData.Setup(d => d.UpdateRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.UpdateRestuarant(mockDataRequest);

            // assert
            Assert.False(testResult);
        }

        [Fact]
        public async Task UpdateRestuarant_Success_ReturnsTrue()
        {
            // arrange
            Restuarant mockDataRequest = new()
            {
                Id = "123456",
                Name = "test",
                CuisineType = "test"
            };
            MongoUpdateResult mockDataResponse = new()
            {
                IsAcknowledged = true,
                ModifiedCount = 1
            };
            mockData.Setup(d => d.UpdateRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockDataResponse);

            // act
            var testResult = await repo.UpdateRestuarant(mockDataRequest);

            // assert
            Assert.True(testResult);
        }
    }
}