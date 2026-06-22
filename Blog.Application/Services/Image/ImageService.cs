using Microsoft.AspNetCore.Http;

namespace Blog.Application.Services.Image
{
	public class ImageServcie : IImageService
	{
		private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

		private static readonly HashSet<string> AllowedExtensions =
			new(StringComparer.OrdinalIgnoreCase)
			{
				".jpg", ".jpeg", ".png"
			};

		private readonly string _uploadPath;

		public ImageServcie(string webRootPath)
		{
			_uploadPath = Path.Combine(webRootPath, "images", "posts");
		}

		public async Task<string> UploadImageAsync(IFormFile file)
		{
			if (!ValidateImage(file))
				throw new InvalidOperationException("Invalid image file");

			EnsureFolderExists();

			var extension = Path.GetExtension(file.FileName);
			var fileName = $"{Guid.NewGuid():N}{extension}";
			var filePath = Path.Combine(_uploadPath, fileName);

			await using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return $"/images/posts/{fileName}";
		}

		public async Task<bool> DeleteImageAsync(string imageUrl)
		{
			if (string.IsNullOrWhiteSpace(imageUrl))
				return false;

			try
			{
				var fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);
				var filePath = Path.Combine(_uploadPath, fileName);

				if (!File.Exists(filePath))
					return false;

				File.Delete(filePath);
				return await Task.FromResult(true);
			}
			catch
			{
				return false;
			}
		}

		public bool ValidateImage(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return false;

			if (file.Length > MaxFileSize)
				return false;

			var extension = Path.GetExtension(file.FileName);

			return AllowedExtensions.Contains(extension);
		}

		private void EnsureFolderExists()
		{
			if (!Directory.Exists(_uploadPath))
			{
				Directory.CreateDirectory(_uploadPath);
			}
		}
	}
}