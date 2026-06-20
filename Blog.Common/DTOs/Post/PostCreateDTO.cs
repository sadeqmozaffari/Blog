using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.Post
{
    public class PostCreateDTO
    {
        [MaxLength(50)]
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
		[Required]
		public int CategoryId { get; set; }

	}
}
