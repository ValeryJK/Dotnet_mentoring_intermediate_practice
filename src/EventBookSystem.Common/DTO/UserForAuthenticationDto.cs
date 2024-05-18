using System.ComponentModel.DataAnnotations;

namespace EventBookSystem.Common.DTO
{
    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "User name is required")]
        public required string UserName { get; init; }

        [Required(ErrorMessage = "Password name is required")]
        public required string Password { get; init; }
    }
}