using Blog.Domain.Entities;

namespace Blog.Domain.Repositories
{
	public interface IPostRepository : IRepository<Post>
	{
		Task<List<Post>> GetByCategoryIdAsync(int categoryId);
		Task<List<Post>> GetLatestAsync(int count);
	}
}
