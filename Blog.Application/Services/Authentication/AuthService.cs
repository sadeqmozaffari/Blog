using Blog.Common;
using Blog.Common.DTOs.Authentication;
using Blog.Common.DTOs.User;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application.Services.Authentication
{
	public class AuthService : IAuthService
	{
		private const string DefaultRole = "User";
		private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
		{
			"User",
			"Admin"
		};

		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ITokenService _tokenService;

		public AuthService(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			RoleManager<IdentityRole> roleManager,
			ITokenService tokenService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_tokenService = tokenService;
		}

		public async Task<ApiResponse<bool>> IsEmailExistsAsync(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return ApiResponse<bool>.BadRequest("Email is required.");

			var exists = await _userManager.FindByEmailAsync(NormalizeEmail(email)) != null;
			return ApiResponse<bool>.Ok(exists);
		}

		public async Task<ApiResponse<UserDTO>> RegisterAsync(UserCreateDTO dto)
		{
			var email = NormalizeEmail(dto.Email);
			var name = dto.Name.Trim();
			var role = GetRequestedRole(dto.Role);

			if (await _userManager.FindByEmailAsync(email) != null)
				return ApiResponse<UserDTO>.Conflict("Email already exists.");

			await EnsureRoleAsync(role);

			var user = new ApplicationUser
			{
				UserName = email,
				Email = email,
				Name = name,
				EmailConfirmed = false,
				CreatedDate = DateTime.UtcNow
			};

			var createResult = await _userManager.CreateAsync(user, dto.Password);
			if (!createResult.Succeeded)
				return ApiResponse<UserDTO>.ValidationError(ToIdentityErrors(createResult), "Registration failed.");

			var roleResult = await _userManager.AddToRoleAsync(user, role);
			if (!roleResult.Succeeded)
				return ApiResponse<UserDTO>.ValidationError(ToIdentityErrors(roleResult), "User role assignment failed.");

			return ApiResponse<UserDTO>.Created(
				await MapUserAsync(user),
				"User registered successfully");
		}

		public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO dto)
		{
			var email = NormalizeEmail(dto.Email);
			var user = await _userManager.FindByEmailAsync(email);

			if (user == null)
				return ApiResponse<LoginResponseDTO>.Unauthorized("Invalid email or password.");

			if (await _userManager.IsLockedOutAsync(user))
				return ApiResponse<LoginResponseDTO>.Forbidden("User account is locked.");

			var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
			if (!signInResult.Succeeded)
				return ApiResponse<LoginResponseDTO>.Unauthorized("Invalid email or password.");

			var accessToken = await _tokenService.GenerateJwtTokenAsync(user);
			var refreshToken = await _tokenService.GenerateRefreshTokenAsync();
			var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenDays());

			await _tokenService.SaveRefreshTokenAsync(
				user.Id,
				refreshToken,
				refreshTokenExpiresAt);

			var response = new LoginResponseDTO
			{
				UserDTO = await MapUserAsync(user),
				AccessToken = accessToken.Token,
				Token = accessToken.Token,
				RefreshToken = refreshToken,
				ExpiresAt = accessToken.ExpiresAt
			};

			return ApiResponse<LoginResponseDTO>.Ok(response, "Login successful.");
		}

		public async Task<ApiResponse<TokenDTO>> RefreshTokenAsync(string refreshToken)
		{
			if (string.IsNullOrWhiteSpace(refreshToken))
				return ApiResponse<TokenDTO>.BadRequest("Refresh token is required.");

			var token = await _tokenService.ValidateRefreshTokenAsync(refreshToken);

			if (token == null)
				return ApiResponse<TokenDTO>.Unauthorized("Invalid refresh token.");

			var user = await _userManager.FindByIdAsync(token.UserId);

			if (user == null)
				return ApiResponse<TokenDTO>.Unauthorized("User not found.");

			await _tokenService.RevokeRefreshTokenAsync(refreshToken);

			var accessToken = await _tokenService.GenerateJwtTokenAsync(user);
			var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();
			var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenDays());

			await _tokenService.SaveRefreshTokenAsync(
				user.Id,
				newRefreshToken,
				refreshTokenExpiresAt,
				token.JwtTokenId);

			return ApiResponse<TokenDTO>.Ok(new TokenDTO
			{
				AccessToken = accessToken.Token,
				RefreshToken = newRefreshToken,
				ExpiresAt = accessToken.ExpiresAt
			});
		}

		public async Task<ApiResponse<object>> RevokeRefreshTokenAsync(string refreshToken)
		{
			if (string.IsNullOrWhiteSpace(refreshToken))
				return ApiResponse<object>.BadRequest("Refresh token is required.");

			var revoked = await _tokenService.RevokeRefreshTokenAsync(refreshToken);
			if (!revoked)
				return ApiResponse<object>.NotFound("Refresh token not found.");

			return ApiResponse<object>.Ok(new object(), "Refresh token revoked successfully.");
		}

		private async Task<UserDTO> MapUserAsync(ApplicationUser user)
		{
			var roles = await _userManager.GetRolesAsync(user);

			return new UserDTO
			{
				Id = user.Id,
				Email = user.Email ?? string.Empty,
				Name = user.Name,
				Role = roles.FirstOrDefault() ?? DefaultRole
			};
		}

		private async Task EnsureRoleAsync(string role)
		{
			if (!await _roleManager.RoleExistsAsync(role))
				await _roleManager.CreateAsync(new IdentityRole(role));
		}

		private static string GetRequestedRole(string? role)
		{
			if (string.IsNullOrWhiteSpace(role))
				return DefaultRole;

			var normalizedRole = role.Trim();
			return AllowedRoles.Contains(normalizedRole) ? normalizedRole : DefaultRole;
		}

		private static string NormalizeEmail(string email)
		{
			return email.Trim().ToLowerInvariant();
		}

		private static Dictionary<string, string[]> ToIdentityErrors(IdentityResult result)
		{
			return result.Errors
				.GroupBy(error => error.Code)
				.ToDictionary(
					group => group.Key,
					group => group.Select(error => error.Description).ToArray());
		}
	}
}
