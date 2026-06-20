using Blog.Common.DTOs.Authentication;
using Blog.Common.DTOs.User;

namespace Blog.Application.Services.Authentication
{
    public interface IAuthService
    {
        Task<UserDTO?> RegisterAsync(UserCreateDTO registerationRequestDTO);

        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<bool> IsEmailExistsAsync(string email);
    }
}
