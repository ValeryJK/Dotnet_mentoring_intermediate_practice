using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.ActionFilters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();

            context.Result = exceptionType switch
            {
                Type t when t == typeof(KeyNotFoundException) =>
                    new NotFoundObjectResult(new { context.Exception.Message }),
                _ => new ObjectResult(new { Message = "An error occurred." })
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                }
            };

            context.ExceptionHandled = true;
        }
    }
}