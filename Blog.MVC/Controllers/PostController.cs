using Blog.Common;
using Blog.Common.DTOs.Category;
using Blog.Common.DTOs.Post;
using Blog.MVC.Services.IServices;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.MVC.Controllers
{
	public class PostController : Controller
	{
		private readonly IPostService _postService;
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;

		public PostController(IPostService postService, ICategoryService categoryService, IMapper mapper)
		{
			_mapper = mapper;
			_postService = postService;
			_categoryService = categoryService;
		}

		public async Task<IActionResult> Index()
		{
			List<PostDTO> postList = new();
			try
			{
				var response = await _postService.GetAllAsync<ApiResponse<List<PostDTO>>>();
				if (response != null && response.Success && response.Data != null)
				{
					postList = response.Data;
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = $"An error occurred: {ex.Message}";
			}

			return View(postList);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create()
		{
			await PopulateCategoriesAsync();
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(PostCreateDTO createDTO)
		{
			if (createDTO.CategoryId <= 0)
			{
				ModelState.AddModelError(nameof(createDTO.CategoryId), "Please select a category.");
			}

			if (!ModelState.IsValid)
			{
				await PopulateCategoriesAsync(createDTO.CategoryId);
				return View(createDTO);
			}

			try
			{
				var response = await _postService.CreateAsync<ApiResponse<PostDTO>>(createDTO);
				if (response != null && response.Success && response.Data != null)
				{
					TempData["success"] = "Post created successfully.";
					return RedirectToAction(nameof(Index));
				}

				TempData["error"] = response?.Message ?? "Post creation failed. Please try again.";
			}
			catch (Exception ex)
			{
				TempData["error"] = $"An error occurred: {ex.Message}";
			}

			await PopulateCategoriesAsync(createDTO.CategoryId);
			return View(createDTO);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			if (id <= 0)
			{
				TempData["error"] = "Invalid post ID.";
				return RedirectToAction(nameof(Index));
			}
			try
			{
				var response = await _postService.GetAsync<ApiResponse<PostDTO>>(id);
				if (response != null && response.Success && response.Data != null)
				{
					await PopulateCategoriesAsync(response.Data.CategoryId);
					return View(_mapper.Map<PostUpdateDTO>(response.Data));
				}

				TempData["error"] = response?.Message ?? "Post not found.";
			}
			catch (Exception ex)
			{
				TempData["error"] = $"An error occurred: {ex.Message}";
			}


			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(PostUpdateDTO updateDTO)
		{
			if (updateDTO.CategoryId <= 0)
			{
				ModelState.AddModelError(nameof(updateDTO.CategoryId), "Please select a category.");
			}

			if (!ModelState.IsValid)
			{
				await PopulateCategoriesAsync(updateDTO.CategoryId);
				return View(updateDTO);
			}

			try
			{
				var response = await _postService.UpdateAsync<ApiResponse<PostDTO>>(updateDTO);
				if (response != null && response.Success)
				{
					TempData["success"] = "Post updated successfully.";
					return RedirectToAction(nameof(Index));
				}

				TempData["error"] = response?.Message ?? "Post update failed. Please try again.";
			}
			catch (Exception ex)
			{
				TempData["error"] = $"An error occurred: {ex.Message}";
			}

			await PopulateCategoriesAsync(updateDTO.CategoryId);
			return View(updateDTO);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0)
			{
				TempData["error"] = "Invalid post ID.";
				return RedirectToAction(nameof(Index));
			}
			try
			{
				var response = await _postService.GetAsync<ApiResponse<PostDTO>>(id);
				if (response != null && response.Success && response.Data != null)
				{
					return View(response.Data);
				}

				TempData["error"] = response?.Message ?? "Post not found.";
			}
			catch (Exception ex)
			{
				TempData["error"] = $"An error occurred: {ex.Message}";
			}


			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(PostDTO postDTO)
		{

			try
			{
				var response = await _postService.DeleteAsync<ApiResponse<object>>(postDTO.Id);
				if (response != null && response.Success)
				{
					TempData["success"] = "Post deleted successfully.";
				}
				else
				{
					TempData["error"] = response?.Message ?? "Post deletion failed. Please try again.";
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = $"An error occurred: {ex.Message}";
			}

			return RedirectToAction(nameof(Index));
		}

		private async Task PopulateCategoriesAsync(int? selectedCategoryId = null)
		{
			var categories = new List<CategoryDTO>();
			var response = await _categoryService.GetAllAsync<ApiResponse<List<CategoryDTO>>>();

			if (response != null && response.Success && response.Data != null)
			{
				categories = response.Data;
			}

			ViewBag.CategoryList = new SelectList(categories, "Id", "Title", selectedCategoryId);
		}

	}
}
