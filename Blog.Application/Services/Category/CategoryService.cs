using Blog.Common;
using Blog.Common.DTOs.Category;
using Blog.Domain.Entities;
using Blog.Domain.Repositories;
using Blog.Domain.UnitOfWork;
using Mapster;
using MapsterMapper;

namespace Blog.Application.Services.Category
{
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApiResponse<List<CategoryDTO>>> GetAllAsync()
		{
			var categories = await _unitOfWork.Categories.GetAllAsync();

			var data = _mapper.Map<List<CategoryDTO>>(categories);

			return ApiResponse<List<CategoryDTO>>
				.Ok(data, "Categories retrieved successfully");
		}

		public async Task<ApiResponse<CategoryDTO>> GetByIdAsync(int id)
		{
			var category = await _unitOfWork.Categories.GetByIdAsync(id);

			if (category == null)
				return ApiResponse<CategoryDTO>.NotFound("Category not found");

			var data = _mapper.Map<CategoryDTO>(category);

			return ApiResponse<CategoryDTO>
				.Ok(data, "Category retrieved successfully");
		}

		public async Task<ApiResponse<CategoryDTO>> CreateAsync(CategoryCreateDTO dto)
		{
			var category = dto.Adapt<Domain.Entities.Category>();

			category.CreatedDate = DateTime.UtcNow;
			category.UpdatedDate = DateTime.UtcNow;

			await _unitOfWork.Categories.AddAsync(category);
			await _unitOfWork.SaveAsync();

			var result = _mapper.Map<CategoryDTO>(category);

			return ApiResponse<CategoryDTO>
				.CreatedAt(result, "Category created successfully");
		}

		public async Task<ApiResponse<CategoryDTO>> UpdateAsync(CategoryUpdateDTO dto)
		{
			var category = await _unitOfWork.Categories.GetByIdAsync(dto.Id);

			if (category == null)
				return ApiResponse<CategoryDTO>.NotFound("Category not found");

			category.Title = dto.Title;
			category.Description = dto.Description;
			category.UpdatedDate = DateTime.UtcNow;

			_unitOfWork.Categories.Update(category);
			await _unitOfWork.SaveAsync();

			var result = _mapper.Map<CategoryDTO>(category);

			return ApiResponse<CategoryDTO>
				.Ok(result, "Category updated successfully");
		}

		public async Task<ApiResponse<object>> DeleteAsync(int id)
		{
			var category = await _unitOfWork.Categories.GetByIdAsync(id);

			if (category == null)
				return ApiResponse<object>.NotFound("Category not found");

			_unitOfWork.Categories.Delete(category);
			await _unitOfWork.SaveAsync();

			return ApiResponse<object>
				.NoContent("Category deleted successfully");
		}
	}
}