using Demo.Restuarants.API.Controllers.V1;
using Demo.Restuarants.Core.Interfaces;
using Demo.Restuarants.Shared.Models;

namespace Demo.Resuarants.API.NUnit.Tests.Controllers.V1;

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


    [Test]
    public async Task Get_DataExists_ReturnsRestuarantCollection()
    {
        // arrange
        List<RestuarantBO> mockPaginatedData = [
            new("", "", "", null, "", new LocationBO("", "", "", "", "")),
            new("", "", "", null, "", new LocationBO("", "", "", "", ""))
        ];
        PaginationMetaData mockPaginatedMetaData = new(1, mockPaginatedData.Count, 1, mockPaginatedData.Count);
        PaginationResponse<RestuarantBO> mockResponse = new(mockPaginatedData, mockPaginatedMetaData);
        mockOrchestration.Setup(r => r.ListRestuarants(It.IsAny<FilterQueryParametersBO>())).ReturnsAsync(mockResponse);

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
        List<RestuarantBO> mockPaginatedData = [];
        PaginationMetaData mockPaginatedMetaData = new(1, mockPaginatedData.Count, 1, mockPaginatedData.Count);
        PaginationResponse<RestuarantBO> mockResponse = new(mockPaginatedData, mockPaginatedMetaData);
        mockOrchestration.Setup(r => r.ListRestuarants(It.IsAny<FilterQueryParametersBO>())).ReturnsAsync(mockResponse);

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
        List<RestuarantBO> mockPaginatedData = [
            new("", "", "", null, "", new LocationBO("", "", "", "", "")),
            new("", "", "", null, "", new LocationBO("", "", "", "", ""))
        ];
        PaginationMetaData mockPaginatedMetaData = new(1, mockPaginatedData.Count, 1, mockPaginatedData.Count);
        PaginationResponse<RestuarantBO> mockResponse = new(mockPaginatedData, mockPaginatedMetaData);
        mockOrchestration.Setup(r => r.ListRestuarants(It.IsAny<FilterQueryParametersBO>())).ReturnsAsync(mockResponse);

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
        List<RestuarantBO> mockPaginatedData = [];
        PaginationMetaData mockPaginatedMetaData = new(1, mockPaginatedData.Count, 1, mockPaginatedData.Count);
        PaginationResponse<RestuarantBO> mockResponse = new(mockPaginatedData, mockPaginatedMetaData);
        mockOrchestration.Setup(r => r.ListRestuarants(It.IsAny<FilterQueryParametersBO>())).ReturnsAsync(mockResponse);

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
        RestuarantBO? mockResponse = new(id, "", "", null, "", new LocationBO("", "", "", "", ""));
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Restuarant(id);

        Assert.Multiple(() =>
        {
            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(string.IsNullOrWhiteSpace(testResult?.Id), Is.False);
        });
    }

    [Test]
    public async Task Restuarant_NoDataExists_ReturnsEmptyCollection()
    {
        // arrange
        string id = "123456";
        RestuarantBO? mockResponse = new("", "", "", null, "", new LocationBO("", "", "", "", ""));
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Restuarant(id);

        Assert.Multiple(() =>
        {
            // assert
            Assert.That(testResult, Is.Not.Null);
            Assert.That(string.IsNullOrWhiteSpace(testResult?.Id), Is.True);
        });
    }
}
