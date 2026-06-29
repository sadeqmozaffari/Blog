using System.Security.Claims;

namespace Blog.MVC.Services.IServices
{
	public interface ITokenProvider
	{
		void SetToken(string accessToken, string refreshToken);
		string? GetAccessToken();
		string? GetRefreshToken();
		void ClearToken();
		ClaimsPrincipal? CreatePrincipalFromJwtToken(string token);
	}
}
