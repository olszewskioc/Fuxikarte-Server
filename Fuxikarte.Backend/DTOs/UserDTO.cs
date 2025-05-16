using System.ComponentModel.DataAnnotations;

namespace Fuxikarte.Backend.DTOs
{
    public class UserRegistrationDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public required string Password { get; set; }
    }

    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }

    public class UserUpdateDTO
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Username { get; set; }

        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }
    }

    public class LoginDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}
