
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
