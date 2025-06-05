using Microsoft.EntityFrameworkCore;
using Shop.Model.Brands;
using Shop.Model.Categories;
using Shop.Model.ProductCategories.Gallery;
using Shop.Model.Products;

namespace Shop.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Brand> Brands { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<Gallery> Galleries { set; get; }
        public DbSet<Product> Products { set; get; }

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
}