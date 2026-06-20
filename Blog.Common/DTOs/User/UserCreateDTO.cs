using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.User
{
    public class UserCreateDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Password { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "Customer";
    }
}
