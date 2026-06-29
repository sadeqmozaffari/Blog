using Blog.Domain.Entities;

namespace Blog.Domain.Repositories
{
	public interface ICategoryRepository : IRepository<Category>
	{
		Task<Category?> GetWithPostsAsync(int id);
	}
}
