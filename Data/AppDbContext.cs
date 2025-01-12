using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Gallery> Galleries => Set<Gallery>();
    public DbSet<NewProduct> NewProducts => Set<NewProduct>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>()
        .HasMany(p => p.Galleries)
        .WithOne(g => g.Product)
        .HasForeignKey(g => g.ProductId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Product>()
        .HasOne(p => p.Category)
        .WithMany(c => c.Products)
        .HasForeignKey(p => p.CatId)
        .HasPrincipalKey(c => c.Id);

        builder.Entity<Product>()
        .HasOne(p => p.Brand)
        .WithMany(b => b.Products)
        .HasForeignKey(p => p.BrandId)
        .HasPrincipalKey(b => b.Id);


        base.OnModelCreating(builder);
    }
}