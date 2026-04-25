using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Demo.Restuarants.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly bool isDevelopment;
    private readonly ILogger<GlobalExceptionHandler> logger;

    public GlobalExceptionHandler(IHostEnvironment env, ILoggerFactory logFactory)
    {
        isDevelopment = env.IsDevelopment();
        logFactory = LoggerFactory.Create(builder =>
        {
            builder.AddJsonConsole(options =>
                options.JsonWriterOptions = new JsonWriterOptions()
                {
                    Indented = true
                });
        });
        logger = logFactory.CreateLogger<GlobalExceptionHandler>();
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.Error(httpContext.TraceIdentifier, exception.Message);

        ProblemDetails problemDetails = SetProblemDetails(httpContext, exception);

        httpContext.Response.StatusCode = problemDetails.Status is null ? 500 : problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private ProblemDetails SetProblemDetails(HttpContext httpContext, Exception exception)
    {
        ProblemDetails problemDetails = new()
        {
            Type = exception.GetType().Name
        };
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        switch (exception)
        {
            case OperationCanceledException:
                problemDetails.Status = 499; // No defined standard code exists for cancelled.  499 seems to be a nginx code which might fit.
                problemDetails.Title = "Requested cancelled";
                problemDetails.Detail = "Requested operation was cancelled by the user";
                break;
            case BadHttpRequestException: // this will not be triggered by model validation on the controller.  only explicitly throwing this type will reach here
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Bad Request";
                problemDetails.Detail = "";
                break;
            case TimeoutException:
                problemDetails.Status = (int)HttpStatusCode.RequestTimeout;
                problemDetails.Title = "Request Timeout";
                problemDetails.Detail = "The request exceeded allowed time and could not be completed";
                break;
            default:
                problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                problemDetails.Title = "Internal Server Error";
                problemDetails.Detail = "There was an unhandled exception processing the request";
                break;
        }

        // we can customize by environment if needed for additional debugging
        if (isDevelopment)
        {
            problemDetails.Extensions["data"] = exception.Data;
        }

        return problemDetails;
    }
}
