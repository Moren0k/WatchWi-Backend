using Microsoft.EntityFrameworkCore;
using Watchwi.Domain.Entities.Categories;
using Watchwi.Domain.Entities.Images;
using Watchwi.Domain.Entities.Medias;
using Watchwi.Domain.Entities.Users;

namespace Watchwi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet
    public DbSet<User> Users => Set<User>();
    public DbSet<Media> Medias => Set<Media>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}