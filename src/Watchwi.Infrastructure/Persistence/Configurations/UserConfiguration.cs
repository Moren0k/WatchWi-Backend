using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Watchwi.Domain.Entities.Users;

namespace Watchwi.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(UserRole.User);

        builder.Property(u => u.Plan)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(UserPlan.None);

        builder.HasOne(u => u.ProfileImage)
            .WithOne()
            .HasForeignKey<User>(u => u.ProfileImageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.FavoriteMedias)
            .WithMany(m => m.FavoritedBy)
            .UsingEntity(j => j.ToTable("UserFavoriteMedias"));
    }
}