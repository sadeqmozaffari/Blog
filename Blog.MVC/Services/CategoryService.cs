using Blog.Common;
using Blog.Common.DTOs.Category;
using Blog.MVC.Services.IServices;

namespace Blog.MVC.Services
{
	public class CategoryService : BaseService, ICategoryService
	{

		private const string APIEndpoint = "/api/v2/category";
		public CategoryService(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITokenProvider tokenProvider)
			: base(httpClient, tokenProvider, httpContextAccessor)
		{
		}

		public Task<T?> CreateAsync<T>(CategoryCreateDTO dto)
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

		public Task<T?> UpdateAsync<T>(CategoryUpdateDTO dto)
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

