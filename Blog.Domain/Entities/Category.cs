using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
	public class Category
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public required string Title { get; set; }

		public string? Description { get; set; }

		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }

		public Collection<Post> Posts { get; set; }
	}
}
