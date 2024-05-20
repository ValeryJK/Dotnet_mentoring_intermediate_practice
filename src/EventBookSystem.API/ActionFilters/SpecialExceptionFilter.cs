using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.ActionFilters
{
    public class SpecialExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not KeyNotFoundException)
            { 
                return; 
            }

            context.Result = new NotFoundObjectResult(new { context.Exception.Message });

            context.ExceptionHandled = true;
        }
    }
}