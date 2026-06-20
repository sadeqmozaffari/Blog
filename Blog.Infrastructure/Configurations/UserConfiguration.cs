using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Blog.Infrastructure.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Email)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(x => x.Password)
				.IsRequired();

			builder.Property(x => x.Role)
				.IsRequired()
				.HasMaxLength(50)
				.HasDefaultValue("User");

			builder.Property(x => x.CreatedDate)
				.IsRequired();

			builder.Property(x => x.UpdatedDate)
				.IsRequired(false);

			// Email باید یکتا باشد
			builder.HasIndex(x => x.Email)
				.IsUnique();
		}
	}
}
