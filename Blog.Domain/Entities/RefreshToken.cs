using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entities
{
	public class RefreshToken
	{
		[Key]
		public int Id { get; set; }
		public string UserId { get; set; } = default!;
		public string JwtTokenId { get; set; } = default!;
		public string RefreshTokenValue { get; set; } = default!;
		public bool IsValid { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ExpiresAt { get; set; }
		public DateTime? RevokedAt { get; set; }
		public ApplicationUser User { get; set; } = default!;
	}
}
