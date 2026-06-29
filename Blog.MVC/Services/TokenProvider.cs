using Blog.Common;
using Blog.MVC.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog.MVC.Services
{
	public class TokenProvider : ITokenProvider
	{

		private readonly IHttpContextAccessor _httpContextAccessor;

		public TokenProvider(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public void ClearToken()
		{
			_httpContextAccessor.HttpContext?.Session.Remove(SD.SessionAccessToken);
			_httpContextAccessor.HttpContext?.Session.Remove(SD.SessionRefreshToken);
		}

		public ClaimsPrincipal? CreatePrincipalFromJwtToken(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return null;
			}
			try
			{
				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(token);

				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

				var emailClaim = jwt.Claims.FirstOrDefault(u =>
					u.Type == "email" || u.Type == ClaimTypes.Email);
				if (emailClaim != null)
				{
					identity.AddClaim(new Claim(ClaimTypes.Name, emailClaim.Value));
				}

				var roleClaims = jwt.Claims.Where(u =>
					u.Type == "role" || u.Type == ClaimTypes.Role);
				foreach (var roleClaim in roleClaims)
				{
					identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
				}

				var nameClaim = jwt.Claims.FirstOrDefault(u =>
					u.Type == "name" || u.Type == "unique_name" || u.Type == ClaimTypes.Name);
				if (nameClaim != null)
				{
					identity.AddClaim(new Claim("FullName", nameClaim.Value));
				}


				return new ClaimsPrincipal(identity);

			}
			catch
			{
				return null;
			}
		}

		public string? GetAccessToken()
		{
			return _httpContextAccessor.HttpContext?.Session.GetString(SD.SessionAccessToken);
		}
		public string? GetRefreshToken()
		{
			return _httpContextAccessor.HttpContext?.Session.GetString(SD.SessionRefreshToken);
		}

		public void SetToken(string accessToken, string refreshToken)
		{
			_httpContextAccessor.HttpContext?.Session.SetString(SD.SessionAccessToken, accessToken);
			_httpContextAccessor.HttpContext?.Session.SetString(SD.SessionRefreshToken, refreshToken);
		}
	}
}
