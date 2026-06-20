using Blog.Common;
using Blog.Common.DTOs.Authentication;
using Blog.Common.DTOs.User;

namespace Blog.Application.Services.Authentication
{
    public interface IAuthService
    {
		Task<ApiResponse<UserDTO>> RegisterAsync(UserCreateDTO dto);
		Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO dto);
		Task<ApiResponse<bool>> IsEmailExistsAsync(string email);
	}
}
