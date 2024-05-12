﻿using EventBookSystem.API.ActionFilters;
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

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _service.AuthenticationService.RegisterUser(userForRegistration);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _service.AuthenticationService.ValidateUser(user))
                return Unauthorized();

            return Ok(new
            {
                Token = await _service.AuthenticationService.CreateToken()
            });
        }
    }
}
