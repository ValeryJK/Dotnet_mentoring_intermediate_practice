using System.ComponentModel.DataAnnotations;

namespace EventBookSystem.Common.DTO
{
    public record UserForRegistrationDto
    {
        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        [Required(ErrorMessage = "Username is required")]
        public required string UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; init; }

        public string? Email { get; init; }

        public string? PhoneNumber { get; init; }

        public ICollection<string> Roles { get; init; } = new HashSet<string>();
    }
}