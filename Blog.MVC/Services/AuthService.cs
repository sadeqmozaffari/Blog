using Blog.Common;
using Blog.Common.DTOs.Authentication;
using Blog.Common.DTOs.User;
using Blog.MVC.Services.IServices;


namespace Blog.MVC.Services
{
    public class AuthService : BaseService, IAuthService
    {

        private const string APIEndpoint = "/api/auth";
        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
            : base(httpClient,httpContextAccessor)
        {
        }

        public Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDTO,
                Url = APIEndpoint+"/login",
            });
        }

        public Task<T?> RegisterAsync<T>(UserCreateDTO registerationRequestDTO)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = registerationRequestDTO,
                Url = APIEndpoint+ "/register",
            });
        }
    }
}
