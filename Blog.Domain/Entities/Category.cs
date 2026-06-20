

namespace Blog.Domain.Entities
{
	public class Category
	{
		//[Key]
		public int Id { get; set; }

		//[Required]
		//[MaxLength(100)]
		public required string Title { get; set; }

		public string? Description { get; set; }

		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }

		public ICollection<Post> Posts { get; set; } = new List<Post>();
	}
}
