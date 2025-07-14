using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using Shop.Model.Shop.Brands;
using Shop.Model.Shop.Categories;
using Shop.Model.Shop.ProductCategories.Gallery;
using Shop.Model.Shop.Products;

namespace Shop.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserProfileIdentity>(options)
    {
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

            builder.Entity<IdentityUserLogin<string>>().HasNoKey();

            builder.Entity<IdentityUserRole<string>>().HasNoKey();

            builder.Entity<IdentityUserToken<string>>().HasNoKey();
        }
    }
}