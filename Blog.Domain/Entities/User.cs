using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.Entities
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public required string Email { get; set; }

		[Required]
		[MaxLength(100)]
		public required string Name { get; set; }

		[Required]
		public required string Password { get; set; }

		[Required]
		[MaxLength(50)]
		public required string Role { get; set; } = "User";

		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}
