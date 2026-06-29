using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.User
{
	public class UserCreateDTO
	{
		[Required]
		[EmailAddress]
		[MaxLength(200)]
		public required string Email { get; set; }

		[Required]
		[MinLength(2)]
		[MaxLength(100)]
		public required string Name { get; set; }

		[Required]
		[MinLength(8)]
		[MaxLength(100)]
		public required string Password { get; set; }

		[MaxLength(50)]
		public string? Role { get; set; }
	}
}
