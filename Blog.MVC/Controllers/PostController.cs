using Blog.Common;
using Blog.Common.DTOs.Post;
using Blog.MVC.Services.IServices;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RoyalVillaWeb.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _villaService;
        private readonly IMapper _mapper;

        public PostController(IPostService villaService, IMapper mapper )
        {
            _mapper = mapper;
            _villaService = villaService;
        }

        public async Task<IActionResult> Index()
        {
            List<PostDTO> villaList = new();
            try
            {
                var response = await _villaService.GetAllAsync<ApiResponse<List<PostDTO>>>();
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

        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(createDTO);
            }

            try
            {
                var response = await _villaService.CreateAsync<ApiResponse<PostDTO>>(createDTO);
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["success"] = "Villa created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }

            return View(createDTO);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid villa ID";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var response = await _villaService.GetAsync<ApiResponse<PostDTO>>(id);
                if (response != null && response.Success && response.Data != null)
                {
                    return View(_mapper.Map<PostUpdateDTO>(response.Data));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }


            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostUpdateDTO updateDTO)
        {

            try
            {
                var response = await _villaService.UpdateAsync<ApiResponse<object>>(updateDTO);
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["success"] = "Villa updated successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid villa ID";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var response = await _villaService.GetAsync<ApiResponse<PostDTO>>(id);
                if (response != null && response.Success && response.Data != null)
                {
                    return View(response.Data);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }


            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(PostDTO PostDTO)
        {
            
            try
            {
                var response = await _villaService.DeleteAsync<ApiResponse<object>>(PostDTO.Id);
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["success"] = "Villa deleted successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
