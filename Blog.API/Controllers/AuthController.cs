using Asp.Versioning;
using Blog.Application.Services.Authentication;
using Blog.Common;
using Blog.Common.DTOs.Authentication;
using Blog.Common.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
	[ApiController]
	[Route("api/auth")]
	[ApiVersionNeutral]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		[ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Register(UserCreateDTO dto)
		{
			if (!ModelState.IsValid)
				return UnprocessableEntity(ApiResponse<object>.ValidationError(ModelState));

			var result = await _authService.RegisterAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPost("login")]
		[ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Login(LoginRequestDTO dto)
		{
			if (!ModelState.IsValid)
				return UnprocessableEntity(ApiResponse<object>.ValidationError(ModelState));

			var result = await _authService.LoginAsync(dto);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPost("refresh-token")]
		[ProducesResponseType(typeof(ApiResponse<TokenDTO>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO dto)
		{
			if (!ModelState.IsValid)
				return UnprocessableEntity(ApiResponse<object>.ValidationError(ModelState));

			var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
			return StatusCode(result.StatusCode, result);
		}

		[HttpPost("revoke-refresh-token")]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> RevokeRefreshToken(RefreshTokenRequestDTO dto)
		{
			if (!ModelState.IsValid)
				return UnprocessableEntity(ApiResponse<object>.ValidationError(ModelState));

			var result = await _authService.RevokeRefreshTokenAsync(dto.RefreshToken);
			return StatusCode(result.StatusCode, result);
		}

		[HttpGet("check-email")]
		[ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> CheckEmail([FromQuery] string email)
		{
			var result = await _authService.IsEmailExistsAsync(email);
			return StatusCode(result.StatusCode, result);
		}
	}
}
