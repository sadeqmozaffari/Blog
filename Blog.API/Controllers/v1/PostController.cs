using Asp.Versioning;
using Blog.Application.Services.Post;
using Blog.Common;
using Blog.Common.DTOs.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/v{version:apiVersion}/post")]
	[ApiVersion("1.0")]
	public class PostController : ControllerBase
	{
		private readonly IPostService _postService;

		public PostController(IPostService postService)
		{
			_postService = postService;
		}

		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse<List<PostDTO>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var result = await _postService.GetAllAsync();
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(typeof(ApiResponse<PostDTO>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(int id)
		{
			var result = await _postService.GetByIdAsync(id);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("category/{categoryId:int}")]
		[ProducesResponseType(typeof(ApiResponse<List<PostDTO>>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetByCategory(int categoryId)
		{
			var result = await _postService.GetByCategoryIdAsync(categoryId);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<PostDTO>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Create(PostCreateDTO dto)
		{
			var result = await _postService.CreateAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPut("{id:int}")]
		[ProducesResponseType(typeof(ApiResponse<PostDTO>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(int id, PostUpdateDTO dto)
		{
			if (id != dto.Id)
				return StatusCode(400, ApiResponse<object>.BadRequest("Id mismatch"));

			var result = await _postService.UpdateAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _postService.DeleteAsync(id);
			return StatusCode(result.StatusCode, result);
		}
	}
}