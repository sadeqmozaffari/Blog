using Blog.Domain.Entities;


namespace Blog.Domain.Repositories
{
	public interface IUserRepository : IRepository<User>
	{
		Task<User?> GetByEmailAsync(string email);
	}
}
