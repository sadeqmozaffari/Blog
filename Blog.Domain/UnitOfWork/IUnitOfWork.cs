using Blog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.UnitOfWork
{
	public interface IUnitOfWork
	{
		IUserRepository Users { get; }
		ICategoryRepository Categories { get; }
		IPostRepository Posts { get; }

		Task SaveAsync();
	}
}
