using Blog.Common;
using Blog.Common.DTOs.Authentication;
using Blog.Common.DTOs.User;
using Blog.MVC.Services.IServices;


namespace Blog.MVC.Services
{
	public class AuthService : BaseService, IAuthService
	{

		private const string APIEndpoint = "/api/auth";
		public AuthService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor,
			IConfiguration configuration, ITokenProvider tokenProvider)
			: base(httpClient, tokenProvider, httpContextAccessor)
		{
		}

		public Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
		{
			return SendAsync<T>(new ApiRequest
			{
				ApiType = SD.ApiType.POST,
				Data = loginRequestDTO,
				Url = APIEndpoint + "/login",
			}, withBearer: false);
		}

		public Task<T?> RefreshTokenAsync<T>(RefreshTokenRequestDTO refreshTokenRequestDTO)
		{
			return SendAsync<T>(new ApiRequest
			{
				ApiType = SD.ApiType.POST,
				Data = refreshTokenRequestDTO,
				Url = APIEndpoint + "/refresh-token",
			}, withBearer: false);
		}

		public Task<T?> RegisterAsync<T>(UserCreateDTO registerationRequestDTO)
		{
			return SendAsync<T>(new ApiRequest
			{
				ApiType = SD.ApiType.POST,
				Data = registerationRequestDTO,
				Url = APIEndpoint + "/register",
			}, withBearer: false);
		}
	}
}
