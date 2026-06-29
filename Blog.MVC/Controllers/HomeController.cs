using Blog.Common;
using Blog.Common.DTOs.Post;
using Blog.MVC.Models;
using Blog.MVC.Services.IServices;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPostService _postService;
		private readonly IMapper _mapper;

		public HomeController(IPostService postService, IMapper mapper)
		{
			_mapper = mapper;
			_postService = postService;
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

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
