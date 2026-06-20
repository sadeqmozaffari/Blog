using Blog.Domain.Entities;
using Blog.Domain.Repositories;
using Blog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
namespace Blog.Infrastructure.Repositories
{
	

	public class PostRepository : Repository<Post>, IPostRepository
	{
		private readonly ApplicationDbContext _context;

		public PostRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Post>> GetByCategoryIdAsync(int categoryId)
		{
			return await _context.Posts
				.Where(x => x.CategoryId == categoryId)
				.ToListAsync();
		}

		public async Task<List<Post>> GetLatestAsync(int count)
		{
			return await _context.Posts
				.OrderByDescending(x => x.CreatedDate)
				.Take(count)
				.ToListAsync();
		}
	}
}
