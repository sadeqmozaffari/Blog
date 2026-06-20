using Blog.Common;
using Blog.Common.DTOs.Authentication;
using Blog.Common.DTOs.User;
using Blog.Domain.Entities;
using Blog.Domain.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Application.Services.Authentication
{
	

	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public AuthService(
			IUserRepository userRepository,
			IMapper mapper,
			IConfiguration configuration)
		{
			_userRepository = userRepository;
			_mapper = mapper;
			_configuration = configuration;
		}

		public async Task<ApiResponse<bool>> IsEmailExistsAsync(string email)
		{
			var exists = await _userRepository.GetByEmailAsync(email) != null;

			return ApiResponse<bool>.Ok(exists, "Checked successfully");
		}

		public async Task<ApiResponse<UserDTO>> RegisterAsync(UserCreateDTO dto)
		{
			var exists = await _userRepository.GetByEmailAsync(dto.Email);

			if (exists != null)
				return ApiResponse<UserDTO>.Conflict("Email already exists");

			var user = dto.Adapt<User>();

			user.CreatedDate = DateTime.UtcNow;
			user.Role ??= "User";

			await _userRepository.AddAsync(user);
			await _userRepository.SaveAsync();

			var result = _mapper.Map<UserDTO>(user);

			return ApiResponse<UserDTO>.CreatedAt(result, "User registered successfully");
		}

		public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO dto)
		{
			var user = await _userRepository.GetByEmailAsync(dto.Email);

			if (user == null || user.Password != dto.Password)
				return ApiResponse<LoginResponseDTO>.NotFound("Invalid email or password");

			var token = GenerateJwtToken(user);

			var response = new LoginResponseDTO
			{
				UserDTO = _mapper.Map<UserDTO>(user),
				Token = token
			};

			return ApiResponse<LoginResponseDTO>.Ok(response, "Login successful");
		}

		private string GenerateJwtToken(User user)
		{
			var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
				new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
				new Claim(ClaimTypes.Email,user.Email),
				new Claim(ClaimTypes.Name,user.Name),
				new Claim(ClaimTypes.Role,user.Role)
			}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}