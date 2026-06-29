using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable("Categories");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Title)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(x => x.Description)
				.HasMaxLength(500); // اختیاری

			builder.Property(x => x.CreatedDate)
				.IsRequired();

			builder.Property(x => x.UpdatedDate)
				.IsRequired(false);
			//builder.Property(x => x.UpdatedDate)
			//	.IsRequired();

			// Relationship
			builder.HasMany(x => x.Posts)
				.WithOne(x => x.Category)   // فرض بر این است Post دارای Category است
				.HasForeignKey(x => x.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
