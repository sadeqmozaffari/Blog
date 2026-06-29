using Blog.Domain.Entities;
using Blog.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Application.Services.Authentication
{
	public class TokenService : ITokenService
	{
		private const int DefaultAccessTokenMinutes = 15;
		private const int DefaultRefreshTokenDays = 7;

		private readonly IConfiguration _configuration;
		private readonly IRefreshTokenRepository _refreshRepository;
		private readonly UserManager<ApplicationUser> _userManager;

		public TokenService(
			IConfiguration configuration,
			IRefreshTokenRepository refreshRepository,
			UserManager<ApplicationUser> userManager)
		{
			_configuration = configuration;
			_refreshRepository = refreshRepository;
			_userManager = userManager;
		}

		public async Task<(string Token, DateTime ExpiresAt)> GenerateJwtTokenAsync(ApplicationUser user)
		{
			var secret = _configuration["JwtSettings:Secret"];
			if (string.IsNullOrWhiteSpace(secret))
				throw new InvalidOperationException("JwtSettings:Secret is not configured.");

			if (secret.Length < 32)
				throw new InvalidOperationException("JwtSettings:Secret must be at least 32 characters.");

			var expiresAt = DateTime.UtcNow.AddMinutes(GetConfiguredInt("JwtSettings:AccessTokenMinutes", DefaultAccessTokenMinutes));
			var key = Encoding.UTF8.GetBytes(secret);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
				new Claim(ClaimTypes.Name, user.Name),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var roles = await _userManager.GetRolesAsync(user);
			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires: expiresAt,
				signingCredentials: new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256));

			return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
		}

		public async Task<string> GenerateRefreshTokenAsync()
		{
			string refreshToken;
			do
			{
				refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
			}
			while (await _refreshRepository.ExistsAsync(refreshToken));

			return refreshToken;
		}

		public async Task SaveRefreshTokenAsync(string userId, string refreshToken, DateTime expiresAt, string? tokenFamilyId = null)
		{
			await _refreshRepository.AddAsync(new RefreshToken
			{
				UserId = userId,
				JwtTokenId = tokenFamilyId ?? Guid.NewGuid().ToString(),
				RefreshTokenValue = refreshToken,
				IsValid = true,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = expiresAt
			});

			await _refreshRepository.SaveChangesAsync();
		}

		public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
		{
			var token = await _refreshRepository.GetByTokenAsync(refreshToken);
			if (token == null)
				return false;

			token.IsValid = false;
			token.RevokedAt = DateTime.UtcNow;
			await _refreshRepository.SaveChangesAsync();

			return true;
		}

		public async Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken)
		{
			if (string.IsNullOrWhiteSpace(refreshToken))
				return null;

			var token = await _refreshRepository.GetByTokenAsync(refreshToken);
			if (token == null)
				return null;

			if (!token.IsValid)
			{
				await RevokeTokenFamilyAsync(token.UserId, token.JwtTokenId);
				return null;
			}

			if (token.ExpiresAt <= DateTime.UtcNow)
			{
				token.IsValid = false;
				token.RevokedAt = DateTime.UtcNow;
				await _refreshRepository.SaveChangesAsync();

				return null;
			}

			return token;
		}

		public int GetRefreshTokenDays()
		{
			return GetConfiguredInt("JwtSettings:RefreshTokenDays", DefaultRefreshTokenDays);
		}

		private int GetConfiguredInt(string key, int defaultValue)
		{
			return int.TryParse(_configuration[key], out var value) && value > 0
				? value
				: defaultValue;
		}

		private async Task RevokeTokenFamilyAsync(string userId, string tokenFamilyId)
		{
			var tokens = await _refreshRepository.GetTokenFamilyAsync(userId, tokenFamilyId);
			foreach (var token in tokens.Where(token => token.IsValid))
			{
				token.IsValid = false;
				token.RevokedAt = DateTime.UtcNow;
			}

			await _refreshRepository.SaveChangesAsync();
		}
	}
}
