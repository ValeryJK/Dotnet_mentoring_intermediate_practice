using EventBookSystem.API.Models;
using EventBookSystem.Common.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.Controllers
{
    public class ApiControllerBase : Controller
    {
        public IActionResult ProcessError(ApiBaseResponse baseResponse)
        {
            return baseResponse switch
            {
                ApiNotFoundResponse => NotFound(new ErrorDetails
                {
                    Message = ((ApiNotFoundResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status404NotFound
                }),
                ApiBadRequestResponse => BadRequest(new ErrorDetails
                {
                    Message = ((ApiBadRequestResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status400BadRequest
                }),
                _ => throw new NotImplementedException()
            };
        }
    }
}