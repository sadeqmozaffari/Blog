
using Blog.Common.DTOs.Authentication;
using Blog.Common.DTOs.User;

namespace Blog.MVC.Services.IServices
{
	public interface IAuthService
	{
		Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO);
		Task<T?> RegisterAsync<T>(UserCreateDTO registerationRequestDTO);
		Task<T?> RefreshTokenAsync<T>(RefreshTokenRequestDTO refreshTokenRequestDTO);

	}
}
