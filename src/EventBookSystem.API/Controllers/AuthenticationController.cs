using EventBookSystem.API.ActionFilters;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.Controllers
{
    [Route("authentication")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AuthenticationController : Controller
    {
        private readonly IServiceManager _service;

        public AuthenticationController(IServiceManager service) => _service = service;

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (await _service.AuthenticationService.ValidateUser(user))
            {
                return Ok(new { Token = await _service.AuthenticationService.CreateToken() });
            }

            return BadRequest(StatusCodes.Status404NotFound);
        }
    }
}