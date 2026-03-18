using Demo.Restuarants.API.Controllers.V2;
using Demo.Restuarants.API.Models.V2;
using Demo.Restuarants.API.Shared.Models.V2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Demo.Resuarants.API.MsTests.Tests.Controllers.V2;

[TestClass]
public class RestuarantControllerTests
{
    private readonly Mock<ILoggerFactory> mockLoggerFactory;
    private readonly Mock<IRestuarantOrchestration> mockOrchestration;
    private readonly RestuarantController controller;

    public RestuarantControllerTests()
    {
        mockLoggerFactory = new Mock<ILoggerFactory>();
        mockOrchestration = new Mock<IRestuarantOrchestration>();
        controller = new(mockLoggerFactory.Object, mockOrchestration.Object)
        {
            // There may be instances where http context might need to be mocked or setup
            // This covers most of the basics that might be needed
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.HttpContext.Request.Headers["device-id"] = "20317";
        controller.HttpContext.Request.Scheme = "http";
        controller.HttpContext.Request.Host = new HostString("localhost");
    }


    [TestMethod]
    public async Task Get_EmptyRestuarants_ReturnsNotFound()
    {
        // arrange
        List<RestuarantBO> mockPaginatedData = [];
        PaginationMetaData mockPaginatedMetaData = new(1, mockPaginatedData.Count, 1, mockPaginatedData.Count);
        PaginationResponse<RestuarantBO> mockResponse = new(mockPaginatedData, mockPaginatedMetaData);
        mockOrchestration.Setup(r => r.ListRestuarants(It.IsAny<FilterQueryParametersBO>())).ReturnsAsync(mockResponse);

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
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType<Ok<IEnumerable<RestuarantBO>>>(testResult);
        Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<IEnumerable<RestuarantBO>>)testResult).StatusCode);
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
        List<RestuarantBO> mockPaginatedData = [];
        PaginationMetaData mockPaginatedMetaData = new(1, mockPaginatedData.Count, 1, mockPaginatedData.Count);
        PaginationResponse<RestuarantBO> mockResponse = new(mockPaginatedData, mockPaginatedMetaData);
        mockOrchestration.Setup(r => r.ListRestuarants(It.IsAny<FilterQueryParametersBO>())).ReturnsAsync(mockResponse);

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
        List<RestuarantBO> mockPaginatedData = [
            new("", "", "", null, "", new LocationBO("", "", "", "", "")),
            new("", "", "", null, "", new LocationBO("", "", "", "", ""))
        ];
        PaginationMetaData mockPaginatedMetaData = new(1, mockPaginatedData.Count, 1, mockPaginatedData.Count);
        PaginationResponse<RestuarantBO> mockResponse = new(mockPaginatedData, mockPaginatedMetaData);
        mockOrchestration.Setup(r => r.ListRestuarants(It.IsAny<FilterQueryParametersBO>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Find(search);

        // assert
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType<Ok<IEnumerable<RestuarantBO>>>(testResult);
        Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<IEnumerable<RestuarantBO>>)testResult).StatusCode);
    }

    [TestMethod]
    public async Task Restuarant_NullRestuarant_ReturnsNotFound()
    {
        // arrange
        string id = "123456";
        RestuarantBO? mockResponse = null;
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

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
        RestuarantBO mockResponse = new("", "test", "test", null, "", new LocationBO("", "", "", "", ""));
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

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
        RestuarantBO mockResponse = new(id, "test", "test", null, "", new LocationBO("", "", "", "", ""));
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Restuarant(id);

        // assert
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType<Ok<RestuarantBO>>(testResult);
        Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<RestuarantBO>)testResult).StatusCode);
    }

    // TODO: See if we can figure out how to trigger model validation.  Then we can add tests for those scenarios on the Post method

    [TestMethod]
    public async Task Post_AddRestuarantSuccess_ReturnsOk()
    {
        // arrange
        Restuarant mockRequest = new()
        {
            Name = "test",
            CuisineType = "test"
        };
        RestuarantBO mockResponse = new("", "", "", null, "", new LocationBO("", "", "", "", ""));
        mockOrchestration.Setup(r => r.CreateRestuarant(It.IsAny<CreateRestuarantRequestBO>())).ReturnsAsync(mockResponse);

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
        mockOrchestration.Setup(r => r.UpdateRestuarant(It.IsAny<string>(), It.IsAny<UpdateRestuarantRequestBO>())).ReturnsAsync(mockResponse);

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
        mockOrchestration.Setup(r => r.UpdateRestuarant(It.IsAny<string>(), It.IsAny<UpdateRestuarantRequestBO>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Put(mockRequest);

        // assert
        Assert.IsNotNull(testResult);
        Assert.IsInstanceOfType<Ok<bool>>(testResult);
        Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
        Assert.IsTrue(((Ok<bool>)testResult).Value);
    }
}
