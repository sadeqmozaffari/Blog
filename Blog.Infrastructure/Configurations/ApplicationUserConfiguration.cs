using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
	public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(x => x.CreatedDate)
				.IsRequired();

			builder.Property(x => x.UpdatedDate)
				.IsRequired(false);
		}
	}
}
