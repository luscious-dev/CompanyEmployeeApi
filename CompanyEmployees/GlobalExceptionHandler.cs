using Contracts;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILoggerManager _loggerManager;

        public GlobalExceptionHandler(ILoggerManager loggerManager)
        {
            _loggerManager = loggerManager;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";

            var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature != null)
            {
                // Getting the appropriate type of code
                httpContext.Response.StatusCode = contextFeature.Error switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError,
                };

                _loggerManager.LogError($"Something went wrong: {contextFeature.Error.Message}");

                await httpContext.Response.
                    WriteAsync(new ErrorDetails() { 
                        StatusCode = httpContext.Response.StatusCode, 
                        Message = contextFeature.Error.Message 
                    }.ToString());
            }

            return true;
        }
    }
}
