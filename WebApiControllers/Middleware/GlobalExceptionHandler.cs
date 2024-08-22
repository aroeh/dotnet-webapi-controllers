using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApiControllers.Middleware
{
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
            if (isDevelopment)
            {
                problemDetails.Detail = exception.Message;
                //problemDetails.Extensions["data"] = exception.Data;
            }
            

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
