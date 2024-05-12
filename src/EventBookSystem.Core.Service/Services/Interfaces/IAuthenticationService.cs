using EventBookSystem.Common.DTO;
using Microsoft.AspNetCore.Identity;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<string> CreateToken();
    }
}
