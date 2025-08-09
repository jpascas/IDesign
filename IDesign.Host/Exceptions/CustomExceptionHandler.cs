using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace IDesign.Host.Exceptions
{
    public class CustomExceptionHandler(IHostEnvironment env, ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        private const string UNHANDLED_EXCEPTION_MESSAGE = "An unhandled exception has occurred while executing the request.";

        private Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers => new()
            {
                // add your custom exception types and handlers here                
            };

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionType = exception.GetType();

            if (_exceptionHandlers.ContainsKey(exceptionType))
            {
                await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
                return true;
            }

            logger.LogError(exception, UNHANDLED_EXCEPTION_MESSAGE);

            var problemDetails = CreateProblemDetails(httpContext, exception);

            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }

        private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
        {
            var statusCode = context.Response.StatusCode;
            var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
            if (string.IsNullOrEmpty(reasonPhrase))
            {
                reasonPhrase = UNHANDLED_EXCEPTION_MESSAGE;
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = reasonPhrase
            };

            if (!env.IsDevelopment())
            {
                return problemDetails;
            }

            problemDetails.Detail = exception.ToString();
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
            problemDetails.Extensions["data"] = exception.Data;

            return problemDetails;
        }

    }
}
