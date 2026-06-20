using Blog.Common;
using Blog.Common.DTOs.Post;


namespace Blog.Application.Services.Post
{
	public interface IPostService
	{
		Task<ApiResponse<List<PostDTO>>> GetAllAsync();
		Task<ApiResponse<PostDTO>> GetByIdAsync(int id);
		Task<ApiResponse<List<PostDTO>>> GetByCategoryIdAsync(int categoryId);

		Task<ApiResponse<PostDTO>> CreateAsync(PostCreateDTO dto);
		Task<ApiResponse<PostDTO>> UpdateAsync(PostUpdateDTO dto);

		Task<ApiResponse<object>> DeleteAsync(int id);
	}
}
