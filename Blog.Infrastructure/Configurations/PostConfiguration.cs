using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
	public class PostConfiguration : IEntityTypeConfiguration<Post>
	{
		public void Configure(EntityTypeBuilder<Post> builder)
		{
			builder.ToTable("Posts");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Title)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(x => x.Description)
				.HasMaxLength(1000);

			builder.Property(x => x.ImageUrl)
				.HasMaxLength(500);

			builder.Property(x => x.CreatedDate)
				.IsRequired();

			builder.Property(x => x.UpdatedDate)
				.IsRequired(false);

			// Relationship (Many Posts -> One Category)
			builder.HasOne(x => x.Category)
				.WithMany(x => x.Posts)
				.HasForeignKey(x => x.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}