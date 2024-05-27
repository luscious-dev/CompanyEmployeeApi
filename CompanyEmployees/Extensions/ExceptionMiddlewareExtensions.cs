using Azure.Core;
using Contracts;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees.Extensions
{
    // Error handling using built in middleware
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeatures = context.Features.Get<IExceptionHandlerFeature>();

                    if(contextFeatures != null)
                    {
                        context.Response.StatusCode = contextFeatures.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"Something went wrong: {contextFeatures.Error}");
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = contextFeatures.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
