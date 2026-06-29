using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Contexts
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<Post> Posts { get; set; } = null!;
		public DbSet<Category> Categories { get; set; } = null!;
		public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//modelBuilder.ApplyConfiguration(new CategoryConfiguration());
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

			//		modelBuilder.Entity<Category>().HasData(
			//			new Category
			//			{
			//				Id = 1,
			//				Title = "Travel"
			//			},
			//			new Category
			//			{
			//				Id = 2,
			//				Title = "Technology"
			//			},
			//			new Category
			//			{
			//				Id = 3,
			//				Title = "Lifestyle"
			//			}
			//		);

			//		modelBuilder.Entity<Post>().HasData(
			//	new Post
			//	{
			//		Id = 1,
			//		CategoryId = 1,
			//		Title = "Exploring the Mountains",
			//		Description = "A journey through beautiful mountain landscapes.",
			//		ImageUrl = "https://picsum.photos/id/1018/800/600",
			//		CreatedDate = new DateTime(2024, 1, 1),
			//		UpdatedDate = new DateTime(2024, 1, 1)
			//	},
			//	new Post
			//	{
			//		Id = 2,
			//		CategoryId = 1,
			//		Title = "City Life Vibes",
			//		Description = "Experience the energy of modern cities.",
			//		ImageUrl = "https://picsum.photos/id/1011/800/600",
			//		CreatedDate = new DateTime(2024, 1, 2),
			//		UpdatedDate = new DateTime(2024, 1, 2)
			//	},
			//	new Post
			//	{
			//		Id = 3,
			//		CategoryId = 1,
			//		Title = "Ocean Breeze",
			//		Description = "Relaxing views of the endless ocean.",
			//		ImageUrl = "https://picsum.photos/id/1016/800/600",
			//		CreatedDate = new DateTime(2024, 1, 3),
			//		UpdatedDate = new DateTime(2024, 1, 3)
			//	},
			//	new Post
			//	{
			//		Id = 4,
			//		CategoryId = 1,
			//		Title = "Forest Walk",
			//		Description = "Peaceful walk through green forests.",
			//		ImageUrl = "https://picsum.photos/id/1015/800/600",
			//		CreatedDate = new DateTime(2024, 1, 4),
			//		UpdatedDate = new DateTime(2024, 1, 4)
			//	},
			//	new Post
			//	{
			//		Id = 5,
			//		CategoryId = 1,
			//		Title = "Sunset Moments",
			//		Description = "Beautiful sunset over the horizon.",
			//		ImageUrl = "https://picsum.photos/id/1003/800/600",
			//		CreatedDate = new DateTime(2024, 1, 5),
			//		UpdatedDate = new DateTime(2024, 1, 5)
			//	},
			//	new Post
			//	{
			//		Id = 6,
			//		CategoryId = 1,
			//		Title = "Modern Architecture",
			//		Description = "Stunning building designs from around the world.",
			//		ImageUrl = "https://picsum.photos/id/1025/800/600",
			//		CreatedDate = new DateTime(2024, 1, 6),
			//		UpdatedDate = new DateTime(2024, 1, 6)
			//	},
			//	new Post
			//	{
			//		Id = 7,
			//		CategoryId = 1,
			//		Title = "Snow Adventure",
			//		Description = "Winter sports and snowy landscapes.",
			//		ImageUrl = "https://picsum.photos/id/1002/800/600",
			//		CreatedDate = new DateTime(2024, 1, 7),
			//		UpdatedDate = new DateTime(2024, 1, 7)
			//	},
			//	new Post
			//	{
			//		Id = 8,
			//		CategoryId = 1,
			//		Title = "Desert Journey",
			//		Description = "Golden sands and desert adventures.",
			//		ImageUrl = "https://picsum.photos/id/1005/800/600",
			//		CreatedDate = new DateTime(2024, 1, 8),
			//		UpdatedDate = new DateTime(2024, 1, 8)
			//	},
			//	new Post
			//	{
			//		Id = 9,
			//		CategoryId = 1,
			//		Title = "Lake View",
			//		Description = "Calm and peaceful lake scenery.",
			//		ImageUrl = "https://picsum.photos/id/1019/800/600",
			//		CreatedDate = new DateTime(2024, 1, 9),
			//		UpdatedDate = new DateTime(2024, 1, 9)
			//	},
			//	new Post
			//	{
			//		Id = 10,
			//		CategoryId = 1,
			//		Title = "Street Photography",
			//		Description = "Life captured in urban streets.",
			//		ImageUrl = "https://picsum.photos/id/1027/800/600",
			//		CreatedDate = new DateTime(2024, 1, 10),
			//		UpdatedDate = new DateTime(2024, 1, 10)
			//	},
			//	new Post
			//	{
			//		Id = 11,
			//		CategoryId = 1,
			//		Title = "Wild Nature",
			//		Description = "Animals in their natural habitat.",
			//		ImageUrl = "https://picsum.photos/id/1024/800/600",
			//		CreatedDate = new DateTime(2024, 1, 11),
			//		UpdatedDate = new DateTime(2024, 1, 11)
			//	},
			//	new Post
			//	{
			//		Id = 12,
			//		CategoryId = 2,
			//		Title = "Night City Lights",
			//		Description = "Beautiful city lights at night.",
			//		ImageUrl = "https://picsum.photos/id/1012/800/600",
			//		CreatedDate = new DateTime(2024, 1, 12),
			//		UpdatedDate = new DateTime(2024, 1, 12)
			//	},
			//	new Post
			//	{
			//		Id = 13,
			//		CategoryId = 1,
			//		Title = "Countryside Calm",
			//		Description = "Peaceful rural landscapes.",
			//		ImageUrl = "https://picsum.photos/id/1020/800/600",
			//		CreatedDate = new DateTime(2024, 1, 13),
			//		UpdatedDate = new DateTime(2024, 1, 13)
			//	},
			//	new Post
			//	{
			//		Id = 14,
			//		CategoryId = 1,
			//		Title = "Bridge View",
			//		Description = "Famous bridges around the world.",
			//		ImageUrl = "https://picsum.photos/id/1031/800/600",
			//		CreatedDate = new DateTime(2024, 1, 14),
			//		UpdatedDate = new DateTime(2024, 1, 14)
			//	},
			//	new Post
			//	{
			//		Id = 15,
			//		CategoryId = 1,
			//		Title = "Travel Moments",
			//		Description = "Memories from amazing journeys.",
			//		ImageUrl = "https://picsum.photos/id/1040/800/600",
			//		CreatedDate = new DateTime(2024, 1, 15),
			//		UpdatedDate = new DateTime(2024, 1, 15)
			//	}
			//);

		}
	}
}
