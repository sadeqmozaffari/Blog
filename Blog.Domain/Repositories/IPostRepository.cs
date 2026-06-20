using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Repositories
{
	public interface IPostRepository : IRepository<Post>
	{
		Task<List<Post>> GetByCategoryIdAsync(int categoryId);
		Task<List<Post>> GetLatestAsync(int count);
	}
}
