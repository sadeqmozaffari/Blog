using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.Authentication
{
	public class RefreshTokenRequestDTO
	{
		[Required]
		[MinLength(32)]
		public string RefreshToken { get; set; } = default!;
	}
}
