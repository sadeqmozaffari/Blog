
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public required string Name { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime? UpdatedDate { get; set; }
	}
}
