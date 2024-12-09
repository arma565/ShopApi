using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    #region BaseCategory
    public DbSet<BaseCategories> baseCategories => Set<BaseCategories>();
    public DbSet<Category> category => Set<Category>();
    public DbSet<Brand> brand => Set<Brand>();
    #endregion 

    #region Category
    public DbSet<BaseCategory> baseCategory => Set<BaseCategory>();
    public DbSet<Gallery> galleries => Set<Gallery>();

    #endregion

    #region Home
    public DbSet<BaseHome> baseHome => Set<BaseHome>();
    #endregion

    #region News
    public DbSet<News> news => Set<News>();
    #endregion

    #region ProductCategory
    public DbSet<Product> products => Set<Product>();
    #endregion

}