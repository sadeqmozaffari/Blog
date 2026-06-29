using Blog.Domain.Repositories;
using Blog.Domain.UnitOfWork;
using Blog.Infrastructure.Contexts;
using Blog.Infrastructure.Repositories;


namespace Blog.Infrastructure.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;

		public ICategoryRepository Categories { get; }
		public IPostRepository Posts { get; }

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;

			Categories = new CategoryRepository(context);
			Posts = new PostRepository(context);
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
