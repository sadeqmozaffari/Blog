using Blog.Domain.Entities;
using Blog.Domain.Repositories;
using Blog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
	

	public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<User?> GetByEmailAsync(string email)
		{
			return await _context.Users
				.FirstOrDefaultAsync(x => x.Email == email);
		}
	}
}
