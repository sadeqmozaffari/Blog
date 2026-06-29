using Blog.Domain.Entities;


namespace Blog.Domain.Repositories
{
	public interface IRefreshTokenRepository
	{
		Task<bool> ExistsAsync(string refreshToken);

		Task AddAsync(RefreshToken refreshToken);

		Task<RefreshToken?> GetByTokenAsync(string refreshToken);

		Task SaveChangesAsync();

		Task<List<RefreshToken>> GetTokenFamilyAsync(string userId, string jwtTokenId);
	}
}
