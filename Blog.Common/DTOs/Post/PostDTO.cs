using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.Post
{
    public class PostDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

		[Required]
		public int CategoryId { get; set; }

		public string? CategoryName { get; set; }
	}
}
