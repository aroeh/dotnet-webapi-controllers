using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using WebApiControllers.Controllers.V2;

namespace WebApiControllers.MsTest.Tests.Controllers.V2
{
    [TestClass]
    public class RestuarantControllerTests
    {
        private readonly Mock<ILogger<RestuarantController>> mockLogger;
        private readonly Mock<IRestuarantRepo> mockRepo;
        private readonly RestuarantController controller;

        public RestuarantControllerTests()
        {
            mockLogger = new Mock<ILogger<RestuarantController>>();
            mockRepo = new Mock<IRestuarantRepo>();
            controller = new(mockLogger.Object, mockRepo.Object);
        }


        [TestMethod]
        public async Task Get_NullRestuarants_ReturnsNotFound()
        {
            // arrange
            List<Restuarant> mockResponse = null;
            mockRepo.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Get();

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Get_EmptyRestuarants_ReturnsNotFound()
        {
            // arrange
            List<Restuarant> mockResponse = [];
            mockRepo.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Get();

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Get_HasRestuarants_ReturnsOk()
        {
            // arrange
            List<Restuarant> mockResponse =
            [
                new(),
                new()
            ];
            mockRepo.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Get();

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<List<Restuarant>>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<List<Restuarant>>)testResult).StatusCode);
        }

        // model state validation is not triggering and is causing incorrect results
        //[TestMethod]
        //public async Task Find_NullName_ReturnsBadRequest()
        //{
        //    // arrange
        //    SearchCriteria search = new();

        //    // act
        //    var testResult = await controller.Find(search);

        //    // assert
        //    Assert.IsNotNull(testResult);
        //    Assert.IsInstanceOfType<BadRequest>(testResult);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequest)testResult).StatusCode);
        //}

        [TestMethod]
        public async Task Find_NullRestuarants_ReturnsNotFound()
        {
            // arrange
            SearchCriteria search = new()
            {
                Name = "test",
                Cuisine = "test"
            };
            List<Restuarant> mockResponse = null;
            mockRepo.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Find(search);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Find_EmptyRestuarants_ReturnsNotFound()
        {
            // arrange
            SearchCriteria search = new()
            {
                Name = "test",
                Cuisine = "test"
            };
            List<Restuarant> mockResponse = [];
            mockRepo.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Find(search);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Find_HasRestuarants_ReturnsOk()
        {
            // arrange
            SearchCriteria search = new()
            {
                Name = "test",
                Cuisine = "test"
            };
            List<Restuarant> mockResponse =
            [
                new(),
                new()
            ];
            mockRepo.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Find(search);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<List<Restuarant>>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<List<Restuarant>>)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Restuarant_NullRestuarant_ReturnsNotFound()
        {
            // arrange
            string id = "123456";
            Restuarant mockResponse = null;
            mockRepo.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Restuarant(id);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Restuarant_RestuarantMissingId_ReturnsNotFound()
        {
            // arrange
            string id = "123456";
            Restuarant mockResponse = new();
            mockRepo.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Restuarant(id);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Restuarant_HasRestuarants_ReturnsOk()
        {
            // arrange
            string id = "123456";
            Restuarant mockResponse = new()
            {
                Id = id,
                Name = "test",
                CuisineType = "test"
            };
            mockRepo.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Restuarant(id);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<Restuarant>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<Restuarant>)testResult).StatusCode);
        }

        // TODO: See if we can figure out how to trigger model validation.  Then we can add tests for those scenarios on the Post method

        [TestMethod]
        public async Task Post_AddRestuarantFail_ReturnsOk()
        {
            // arrange
            Restuarant mockRequest = new()
            {
                Name = "test",
                CuisineType = "test"
            };
            bool mockResponse = false;
            mockRepo.Setup(r => r.InsertRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Post(mockRequest);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<bool>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.IsFalse(((Ok<bool>)testResult).Value);
        }

        [TestMethod]
        public async Task Post_AddRestuarantSuccess_ReturnsOk()
        {
            // arrange
            Restuarant mockRequest = new()
            {
                Name = "test",
                CuisineType = "test"
            };
            bool mockResponse = true;
            mockRepo.Setup(r => r.InsertRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Post(mockRequest);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<bool>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.IsTrue(((Ok<bool>)testResult).Value);
        }

        // TODO: See if we can figure out how to trigger model validation.  Then we can add tests for those scenarios on the Put method

        [TestMethod]
        public async Task Put_AddRestuarantFail_ReturnsOk()
        {
            // arrange
            Restuarant mockRequest = new()
            {
                Id = "123456",
                Name = "test",
                CuisineType = "test"
            };
            bool mockResponse = false;
            mockRepo.Setup(r => r.UpdateRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Put(mockRequest);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<bool>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.IsFalse(((Ok<bool>)testResult).Value);
        }

        [TestMethod]
        public async Task Put_AddRestuarantSuccess_ReturnsOk()
        {
            // arrange
            Restuarant mockRequest = new()
            {
                Id = "123456",
                Name = "test",
                CuisineType = "test"
            };
            bool mockResponse = true;
            mockRepo.Setup(r => r.UpdateRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Put(mockRequest);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<bool>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.IsTrue(((Ok<bool>)testResult).Value);
        }
    }
}
