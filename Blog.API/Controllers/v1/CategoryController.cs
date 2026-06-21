using Asp.Versioning;
using Blog.Application.Services.Category;
using Blog.Common;
using Blog.Common.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
	[ApiController]
	[Route("api/v{version:apiVersion}/category")]
	[ApiVersion("1.0")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<List<CategoryDTO>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var result = await _categoryService.GetAllAsync();
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(typeof(ApiResponse<CategoryDTO>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(int id)
		{
			var result = await _categoryService.GetByIdAsync(id);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<CategoryDTO>), StatusCodes.Status201Created)]
		public async Task<IActionResult> Create(CategoryCreateDTO dto)
		{
			var result = await _categoryService.CreateAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPut("{id:int}")]
		[ProducesResponseType(typeof(ApiResponse<CategoryDTO>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(int id, CategoryUpdateDTO dto)
		{
			if (id != dto.Id)
				return StatusCode(400, ApiResponse<object>.BadRequest("Id mismatch"));

			var result = await _categoryService.UpdateAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _categoryService.DeleteAsync(id);
			return StatusCode(result.StatusCode, result);
		}
	}
}