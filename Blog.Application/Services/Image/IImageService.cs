using Microsoft.AspNetCore.Http;

namespace Blog.Application.Services.Image
{
	public interface IImageService
	{
		Task<string> UploadImageAsync(IFormFile file);
		Task<bool> DeleteImageAsync(string imageUrl);
		bool ValidateImage(IFormFile file);
	}
}
