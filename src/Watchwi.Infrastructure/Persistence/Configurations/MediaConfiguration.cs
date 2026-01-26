using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Watchwi.Domain.Entities.Medias;

namespace Watchwi.Infrastructure.Persistence.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.ToTable("Medias");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(m => m.Title);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(m => m.MediaType)
            .HasConversion<string>()
            .HasMaxLength(30)
            .HasDefaultValue(MediaType.None);

        builder.HasOne(m => m.Poster)
            .WithMany()
            .HasForeignKey(m => m.PosterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Categories)
            .WithMany(c => c.Medias)
            .UsingEntity(j => j.ToTable("MediaCategories"));

        builder.HasMany(m => m.FavoritedBy)
            .WithMany(u => u.FavoriteMedias)
            .UsingEntity(j => j.ToTable("UserFavoriteMedias")); 
    }
}