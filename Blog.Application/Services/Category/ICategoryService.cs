using Blog.Common;
using Blog.Common.DTOs.Category;

namespace Blog.Application.Services.Category
{
	public interface ICategoryService
	{
		Task<ApiResponse<List<CategoryDTO>>> GetAllAsync();
		Task<ApiResponse<CategoryDTO>> GetByIdAsync(int id);
		Task<ApiResponse<CategoryDTO>> CreateAsync(CategoryCreateDTO dto);
		Task<ApiResponse<CategoryDTO>> UpdateAsync(CategoryUpdateDTO dto);
		Task<ApiResponse<object>> DeleteAsync(int id);
	}
}
