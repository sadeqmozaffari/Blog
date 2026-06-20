using Blog.Common.DTOs.User;

namespace Blog.Common.DTOs.Authentication
{
    public class LoginResponseDTO
    {

        public string? Token { get; set; }

        public UserDTO? UserDTO { get; set; }
    }
}
