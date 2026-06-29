using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.Authentication
{
	public class LoginRequestDTO
	{
		[Required]
		[EmailAddress]
		[MaxLength(200)]
		public required string Email { get; set; }

		[Required]
		[MinLength(6)]
		[MaxLength(100)]
		public required string Password { get; set; }
	}
}
