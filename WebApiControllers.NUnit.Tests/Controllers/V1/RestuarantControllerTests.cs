﻿using WebApiControllers.Controllers.V1;

namespace WebApiControllers.NUnit.Tests.Controllers.V1
{
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


        [Test]
        public async Task Get_DataExists_ReturnsRestuarantCollection()
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
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.Not.Empty);
        }

        [Test]
        public async Task Get_NoDataExists_ReturnsEmptyCollection()
        {
            // arrange
            List<Restuarant> mockResponse = [];
            mockRepo.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Get();

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.Empty);
        }

        [Test]
        public async Task Find_DataExists_ReturnsRestuarantCollection()
        {
            // arrange
            string name = "test";
            string cuisine = "test";
            List<Restuarant> mockResponse =
            [
                new(),
                new()
            ];
            mockRepo.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Find(name, cuisine);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.Not.Empty);
        }

        [Test]
        public async Task Find_NoDataExists_ReturnsEmptyCollection()
        {
            // arrange
            string name = "test";
            string cuisine = "test";
            List<Restuarant> mockResponse = [];
            mockRepo.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Find(name, cuisine);

            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(testResult, Is.Empty);
        }

        [Test]
        public async Task Restuarant_DataExists_ReturnsRestuarantCollection()
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

            Assert.Multiple(() =>
            {
                // assert
                Assert.That(testResult, Is.Not.Null);
                Assert.That(string.IsNullOrWhiteSpace(testResult.Id), Is.False);
            });
        }

        [Test]
        public async Task Restuarant_NoDataExists_ReturnsEmptyCollection()
        {
            // arrange
            string id = "123456";
            Restuarant mockResponse = new();
            mockRepo.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await controller.Restuarant(id);

            Assert.Multiple(() =>
            {
                // assert
                Assert.That(testResult, Is.Not.Null);
                Assert.That(string.IsNullOrWhiteSpace(testResult.Id), Is.True);
            });
        }
    }
}
