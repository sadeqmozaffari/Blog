using Blog.Common;
using Blog.Common.DTOs.Authentication;
using Blog.Common.DTOs.User;
using Blog.MVC.Services.IServices;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;


namespace Blog.MVC.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IMapper _mapper;
		private readonly ITokenProvider _tokenProvider;

		public AuthController(IAuthService authService, IMapper mapper, ITokenProvider tokenProvider)
		{
			_mapper = mapper;
			_authService = authService;
			_tokenProvider = tokenProvider;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
		{
			try
			{
				var response = await _authService.LoginAsync<ApiResponse<LoginResponseDTO>>(loginRequestDTO);
				if (response != null && response.Success && response.Data != null &&
					!string.IsNullOrEmpty(response.Data.AccessToken) &&
					!string.IsNullOrEmpty(response.Data.RefreshToken))
				{
					var principal = _tokenProvider.CreatePrincipalFromJwtToken(response.Data.AccessToken);
					if (principal != null)
					{
						await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
						_tokenProvider.SetToken(response.Data.AccessToken, response.Data.RefreshToken);
						return RedirectToAction("Index", "Home");
					}
					else
					{
						TempData["error"] = "Invalid token received. Please try again.";
					}
				}
				else
				{
					TempData["error"] = response?.Message ?? "Login failed. Please try again.";

				}
				return View(loginRequestDTO);
			}
			catch (Exception ex)
			{
				TempData["error"] = $"An error occurred: {ex.Message}";
			}

			return View();
		}


		[HttpGet]
		public IActionResult Register()
		{
			return View(new UserCreateDTO
			{
				Email = string.Empty,
				Name = string.Empty,
				Password = string.Empty,
				Role = "User"
			});
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(UserCreateDTO registerationRequestDTO)
		{
			try
			{
				ApiResponse<UserDTO>? response = await _authService.RegisterAsync<ApiResponse<UserDTO>>(registerationRequestDTO);
				if (response != null && response.Success && response.Data != null)
				{
					TempData["success"] = "Registration successful! Please login with your credentials.";
					return RedirectToAction("Login");
				}
				else
				{
					TempData["error"] = response?.Message ?? "Registration failed. Please try again.";
					return View(registerationRequestDTO);
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = $"An error occurred: {ex.Message}";
			}

			return View(registerationRequestDTO);
		}

		public IActionResult AccessDenied()
		{
			return View();
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			_tokenProvider.ClearToken();
			return RedirectToAction("Index", "Home");
		}


	}
}
