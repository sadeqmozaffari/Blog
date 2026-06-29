using Blog.Domain.Entities;
using Blog.Domain.Repositories;
using Blog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		private readonly ApplicationDbContext _context;

		public RefreshTokenRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<bool> ExistsAsync(string refreshToken)
		{
			return await _context.RefreshTokens
				.AnyAsync(x => x.RefreshTokenValue == refreshToken);
		}

		public async Task AddAsync(RefreshToken refreshToken)
		{
			await _context.RefreshTokens.AddAsync(refreshToken);
		}

		public async Task<RefreshToken?> GetByTokenAsync(string refreshToken)
		{
			return await _context.RefreshTokens
				.FirstOrDefaultAsync(x => x.RefreshTokenValue == refreshToken);
		}

		public async Task<List<RefreshToken>> GetTokenFamilyAsync(string userId, string jwtTokenId)
		{
			return await _context.RefreshTokens
				.Where(x => x.UserId == userId &&
							x.JwtTokenId == jwtTokenId)
				.ToListAsync();
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
