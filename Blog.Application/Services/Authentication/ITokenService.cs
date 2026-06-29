using Blog.Domain.Entities;

namespace Blog.Application.Services.Authentication
{
	public interface ITokenService
	{
		Task<(string Token, DateTime ExpiresAt)> GenerateJwtTokenAsync(ApplicationUser user);
		Task<string> GenerateRefreshTokenAsync();

		Task SaveRefreshTokenAsync(string userId, string refreshToken, DateTime expiresAt, string? tokenFamilyId = null);

		Task<bool> RevokeRefreshTokenAsync(string refreshToken);


		Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken);

		int GetRefreshTokenDays();
	}
}
