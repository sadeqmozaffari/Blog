using Blog.Common.DTOs.User;

namespace Blog.Common.DTOs.Authentication
{
	public class LoginResponseDTO
	{
		public string? AccessToken { get; set; }

		public string? Token { get; set; }

		public string? RefreshToken { get; set; }

		public DateTime? ExpiresAt { get; set; }

		public UserDTO? UserDTO { get; set; }
	}
}
