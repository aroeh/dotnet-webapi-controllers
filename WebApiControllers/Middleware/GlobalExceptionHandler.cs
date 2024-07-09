using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApiControllers.Middleware
{
    public class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("An error occurred while processing your request: {TraceId} | {Message}", httpContext.TraceIdentifier, exception.Message);

            // default the status code value to 500 for unhandled exception errors
            int statusCode = 500;
            string title = "An unhandled error occurred";

            // uncomment to extend and use to handle other exception types as desired.
            //switch (exception)
            //{
            //    case BadHttpRequestException:
            //        statusCode = (int)HttpStatusCode.BadRequest;
            //        title = exception.GetType().Name;
            //        break;
            //    default:
            //        statusCode = (int)HttpStatusCode.InternalServerError;
            //        title = "Internal Server Error";
            //        break;
            //}

            ProblemDetails problemDetails = new()
            {
                Status = statusCode,
                Type = exception.GetType().Name,
                Title = title
            };
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            // we can customize by environment if needed for additional debugging
            if (env.IsDevelopment())
            {
                problemDetails.Detail = exception.Message;
                //problemDetails.Extensions["data"] = exception.Data;
            }
            

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
