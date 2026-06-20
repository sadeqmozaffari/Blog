using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.Post
{
    public class PostUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

		[Required]
		public int CategoryId { get; set; }

	}
}
