using EventBookSystem.API.ActionFilters;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.Controllers
{
    [Route("users")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authorizationService;

        public UsersController(IAuthenticationService authorizationService) => _authorizationService = authorizationService;

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _authorizationService.RegisterUser(userForRegistration);

            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
    }
}