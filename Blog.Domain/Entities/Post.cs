using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
	public class Post
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public required string Title { get; set; }
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }

		[Required]
		[ForeignKey(nameof(Category))]
		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
