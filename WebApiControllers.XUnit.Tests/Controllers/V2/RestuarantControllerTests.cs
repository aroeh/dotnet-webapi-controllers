using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApiControllers.Controllers.V2;

namespace WebApiControllers.XUnit.Tests.Controllers.V2;

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


    [Fact]
    public async Task Get_NullRestuarants_ReturnsNotFound()
    {
        // arrange
        List<Restuarant> mockResponse = null;
        mockOrchestration.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Get();

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<NotFound>(testResult);
        Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
    }

    [Fact]
    public async Task Get_EmptyRestuarants_ReturnsNotFound()
    {
        // arrange
        List<Restuarant> mockResponse = [];
        mockOrchestration.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Get();

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<NotFound>(testResult);
        Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
    }

    [Fact]
    public async Task Get_HasRestuarants_ReturnsOk()
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
        Assert.NotNull(testResult);
        Assert.IsType<Ok<List<Restuarant>>>(testResult);
        Assert.Equal((int)HttpStatusCode.OK, ((Ok<List<Restuarant>>)testResult).StatusCode);
    }

    // model state validation is not triggering and is causing incorrect results
    //[Fact]
    //public async Task Find_NullName_ReturnsBadRequest()
    //{
    //    // arrange
    //    SearchCriteria search = new();

    //    // act
    //    var testResult = await controller.Find(search);

    //    // assert
    //    Assert.NotNull(testResult);
    //    Assert.IsType<BadRequest>(testResult);
    //    Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequest)testResult).StatusCode);
    //}

    [Fact]
    public async Task Find_NullRestuarants_ReturnsNotFound()
    {
        // arrange
        SearchCriteria search = new()
        {
            Name = "test",
            Cuisine = "test"
        };
        List<Restuarant> mockResponse = null;
        mockOrchestration.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Find(search);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<NotFound>(testResult);
        Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
    }

    [Fact]
    public async Task Find_EmptyRestuarants_ReturnsNotFound()
    {
        // arrange
        SearchCriteria search = new()
        {
            Name = "test",
            Cuisine = "test"
        };
        List<Restuarant> mockResponse = [];
        mockOrchestration.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Find(search);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<NotFound>(testResult);
        Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
    }

    [Fact]
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
        mockOrchestration.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Find(search);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<Ok<List<Restuarant>>>(testResult);
        Assert.Equal((int)HttpStatusCode.OK, ((Ok<List<Restuarant>>)testResult).StatusCode);
    }

    [Fact]
    public async Task Restuarant_NullRestuarant_ReturnsNotFound()
    {
        // arrange
        string id = "123456";
        Restuarant mockResponse = null;
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Restuarant(id);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<NotFound>(testResult);
        Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
    }

    [Fact]
    public async Task Restuarant_RestuarantMissingId_ReturnsNotFound()
    {
        // arrange
        string id = "123456";
        Restuarant mockResponse = new();
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Restuarant(id);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<NotFound>(testResult);
        Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
    }

    [Fact]
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
        mockOrchestration.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Restuarant(id);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<Ok<Restuarant>>(testResult);
        Assert.Equal((int)HttpStatusCode.OK, ((Ok<Restuarant>)testResult).StatusCode);
    }

    // TODO: See if we can figure out how to trigger model validation.  Then we can add tests for those scenarios on the Post method

    [Fact]
    public async Task Post_AddRestuarantFail_ReturnsOk()
    {
        // arrange
        Restuarant mockRequest = new()
        {
            Name = "test",
            CuisineType = "test"
        };
        bool mockResponse = false;
        mockOrchestration.Setup(r => r.InsertRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Post(mockRequest);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<Ok<bool>>(testResult);
        Assert.Equal((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
        Assert.False(((Ok<bool>)testResult).Value);
    }

    [Fact]
    public async Task Post_AddRestuarantSuccess_ReturnsOk()
    {
        // arrange
        Restuarant mockRequest = new()
        {
            Name = "test",
            CuisineType = "test"
        };
        bool mockResponse = true;
        mockOrchestration.Setup(r => r.InsertRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Post(mockRequest);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<Ok<bool>>(testResult);
        Assert.Equal((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
        Assert.True(((Ok<bool>)testResult).Value);
    }

    // TODO: See if we can figure out how to trigger model validation.  Then we can add tests for those scenarios on the Put method

    [Fact]
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
        mockOrchestration.Setup(r => r.UpdateRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Put(mockRequest);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<Ok<bool>>(testResult);
        Assert.Equal((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
        Assert.False(((Ok<bool>)testResult).Value);
    }

    [Fact]
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
        mockOrchestration.Setup(r => r.UpdateRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

        // act
        var testResult = await controller.Put(mockRequest);

        // assert
        Assert.NotNull(testResult);
        Assert.IsType<Ok<bool>>(testResult);
        Assert.Equal((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
        Assert.True(((Ok<bool>)testResult).Value);
    }
}