﻿using WebApiControllers.Controllers.V1;

namespace WebApiControllers.MsTest.Tests.Controllers.V1;

[TestClass]
public class RestuarantControllerTests
{
    private readonly Mock<ILogger<RestuarantController>> mockLogger;
    private readonly Mock<IRestuarantOrchestration> mockOrchestration;
    private readonly RestuarantController controller;

    public RestuarantControllerTests()
    {
        mockLogger = new Mock<ILogger<RestuarantController>>();
        mockOrchestration = new Mock<IRestuarantOrchestration>();
        controller = new(mockLogger.Object, mockOrchestration.Object);
    }

    [TestMethod]
    public async Task Get_DataExists_ReturnsRestuarantCollection()
    {
        // arrange
        List<Restuarant> mockResponse =
        [
            new(),
            new()
        ];
        mockOrchestration.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Get();

        // assert
        Assert.IsNotNull(testResult);
        Assert.IsTrue(testResult.Count > 0);
    }

    [TestMethod]
    public async Task Get_NoDataExists_ReturnsEmptyCollection()
    {
        // arrange
        List<Restuarant> mockResponse = [];
        mockOrchestration.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Get();

        // assert
        Assert.IsNotNull(testResult);
        Assert.IsFalse(testResult.Count > 0);
    }

    [TestMethod]
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
        mockOrchestration.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Find(name, cuisine);

        // assert
        Assert.IsNotNull(testResult);
        Assert.IsTrue(testResult.Count > 0);
    }

    [TestMethod]
    public async Task Find_NoDataExists_ReturnsEmptyCollection()
    {
        // arrange
        string name = "test";
        string cuisine = "test";
        List<Restuarant> mockResponse = [];
        mockOrchestration.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Find(name, cuisine);

        // assert
        Assert.IsNotNull(testResult);
        Assert.IsFalse(testResult.Count > 0);
    }

    [TestMethod]
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
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Restuarant(id);

        // assert
        Assert.IsNotNull(testResult);
        Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Id));
    }

    [TestMethod]
    public async Task Restuarant_NoDataExists_ReturnsEmptyCollection()
    {
        // arrange
        string id = "123456";
        Restuarant mockResponse = new();
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Restuarant(id);

        // assert
        Assert.IsNotNull(testResult);
        Assert.IsTrue(string.IsNullOrWhiteSpace(testResult.Id));
    }
}
