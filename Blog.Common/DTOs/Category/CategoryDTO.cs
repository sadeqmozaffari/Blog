using System.ComponentModel.DataAnnotations;

namespace Blog.Common.DTOs.Category
{
	public class CategoryDTO
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public required string Title { get; set; }

		public string? Description { get; set; }



	}
}
