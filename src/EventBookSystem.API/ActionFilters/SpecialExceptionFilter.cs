using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.ActionFilters
{
    public class SpecialExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                KeyNotFoundException => new NotFoundObjectResult(new { context.Exception.Message }),
                BadHttpRequestException => new ObjectResult(new { context.Exception.Message })
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                },
                _ => new ObjectResult(new { Message = "An error occurred." })
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                }
            };

            context.ExceptionHandled = true;
        }
    }
}