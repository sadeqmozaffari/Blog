using Blog.Domain.Entities;
using Blog.Domain.Repositories;
using Blog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
namespace Blog.Infrastructure.Repositories
{


	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		private readonly ApplicationDbContext _context;

		public CategoryRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Category?> GetWithPostsAsync(int id)
		{
			return await _context.Categories
				.Include(x => x.Posts)
				.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
