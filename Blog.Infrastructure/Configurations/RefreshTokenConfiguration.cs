using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.ToTable("RefreshTokens");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.JwtTokenId)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(x => x.UserId)
				.IsRequired()
				.HasMaxLength(450);

			builder.Property(x => x.RefreshTokenValue)
				.IsRequired()
				.HasMaxLength(500);

			builder.Property(x => x.IsValid)
				.IsRequired();

			builder.Property(x => x.CreatedAt)
				.IsRequired();

			builder.Property(x => x.ExpiresAt)
				.IsRequired();

			builder.HasIndex(x => x.RefreshTokenValue)
				.IsUnique();

			builder.HasIndex(x => new { x.UserId, x.JwtTokenId });

			builder.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
