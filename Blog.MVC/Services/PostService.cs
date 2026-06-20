using Blog.Common;
using Blog.Common.DTOs.Post;
using Blog.MVC.Services.IServices;


namespace Blog.MVC.Services
{
    public class PostService : BaseService,IPostService
    {
        
        private const string APIEndpoint = "/api/post";
        public PostService(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
            : base(httpClient,httpContextAccessor)
        {
        }

        public Task<T?> CreateAsync<T>(PostCreateDTO dto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = APIEndpoint
            });
        }

        public Task<T?> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{APIEndpoint}/{id}"
            });
        }

        public Task<T?> GetAllAsync<T>()
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = $"{APIEndpoint}"
            });
        }

        public Task<T?> GetAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
             
                Url = $"{APIEndpoint}/{id}"
            });
        }

        public Task<T?> UpdateAsync<T>(PostUpdateDTO dto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = $"{APIEndpoint}/{dto.Id}"
            });
        }
    }
}
