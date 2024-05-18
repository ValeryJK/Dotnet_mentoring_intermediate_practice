using EventBookSystem.API.Models;
using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace EventBookSystem.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var errorMessage = "Internal Server Error.";

                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                    }

                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = errorMessage,
                    }.ToString());
                });
            });
        }
    }
}