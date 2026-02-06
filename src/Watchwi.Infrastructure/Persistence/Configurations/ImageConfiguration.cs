using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Watchwi.Domain.Entities.Images;

namespace Watchwi.Infrastructure.Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable("Images");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.PublicId)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(i => i.PublicId).IsUnique();

        builder.Property(i => i.Url)
            .IsRequired()
            .HasMaxLength(1000);
    }
}