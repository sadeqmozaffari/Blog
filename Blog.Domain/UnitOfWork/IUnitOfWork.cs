using Blog.Domain.Repositories;

namespace Blog.Domain.UnitOfWork
{
	public interface IUnitOfWork
	{
		ICategoryRepository Categories { get; }
		IPostRepository Posts { get; }

		Task SaveAsync();
	}
}
