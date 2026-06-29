using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.Category
{
	public class CategoryCreateDTO
	{
		[Required]
		[MaxLength(100)]
		public required string Title { get; set; }

		public string? Description { get; set; }


	}
}
