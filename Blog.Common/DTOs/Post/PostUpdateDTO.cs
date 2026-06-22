using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.Post
{
    public class PostUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
		public IFormFile? ImageFile { get; set; }
		[Required]
		public int CategoryId { get; set; }

	}
}
