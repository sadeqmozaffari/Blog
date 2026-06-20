using Blog.Common;
using Blog.Common.DTOs.Post;
using Blog.MVC.Models;
using Blog.MVC.Services.IServices;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RoyalVillaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public HomeController(IPostService Service, IMapper mapper )
        {
            _mapper = mapper;
			_postService = Service;
        }

        public async Task<IActionResult> Index()
        {
            List<PostDTO> villaList = new();
            try
            {
                var response = await _postService.GetAllAsync<ApiResponse<List<PostDTO>>>();
                if(response!=null && response.Success && response.Data != null)
                {
                    villaList= response.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }

            return View(villaList);
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
