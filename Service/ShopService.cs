using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ShopService(AppDbContext context)
{
    private readonly AppDbContext _context = context;
    #region Get
    /// <summary>
    /// Get BaseCategories include categories and brands
    /// </summary>
    /// <returns>
    /// Return a baseCategoreis model
    /// </returns>
    public async Task<BaseCategories> GetBaseCategories()
    {
        var categories = await _context.Categories.AsNoTracking().ToListAsync();
        var brands = await _context.Brands.AsNoTracking().ToListAsync();
        return new BaseCategories { Categories = categories, Brands = brands };
    }

    /// <summary>
    /// Get product categories include products which are: mobile,makeup,trends,sports,home
    /// </summary>
    /// <returns>
    /// Return a ProductCategory model
    /// </returns>
    public async Task<ProductCategory> GetProductCategories()
    {
        var mobile = await _context.Products.Where(p => p.CatId == "1").AsNoTracking().ToListAsync();
        var makeup = await _context.Products.Where(p => p.CatId == "2").AsNoTracking().ToListAsync();
        var trends = await _context.Products.Where(p => p.CatId == "3").AsNoTracking().ToListAsync();
        var sports = await _context.Products.Where(p => p.CatId == "4").AsNoTracking().ToListAsync();
        var home = await _context.Products.Where(p => p.CatId == "5").AsNoTracking().ToListAsync();
        return new ProductCategory { Mobiles = mobile, MakeupList = makeup, Trends = trends, Sports = sports, Home = home };
    }

    /// <summary>
    /// Get new products which is products that are added to store recently
    /// </summary>
    /// <returns>
    /// Return a NewProduct model
    /// </returns>
    public async Task<IEnumerable<NewProduct>> GetNewProducts() => await _context.NewProducts.AsNoTracking().ToListAsync();

    /// <summary>
    /// Get products using catId
    /// </summary>
    /// <param name="catId">
    /// by this catId we can fetch products from db 
    /// </param>
    /// <returns>
    /// Return products using catId
    /// </returns>
    public async Task<IEnumerable<Product>> GetProducts(string catId) => await _context.Products.Where(p => p.CatId == catId).AsNoTracking().ToListAsync();

    /// <summary>
    /// Get home page products items
    /// </summary>
    /// <returns>
    /// Return a BaseHome model
    /// </returns>
    public async Task<BaseHome> GetHome()
    {
        var newProducts = await _context.NewProducts.AsNoTracking().ToListAsync();
        var mobile = await _context.Products.Where(p => p.CatId == "1").AsNoTracking().ToListAsync();
        var makeup = await _context.Products.Where(p => p.CatId == "2").AsNoTracking().ToListAsync();
        var discounts = await _context.Products.Where(p => p.CatId == "6").AsNoTracking().ToListAsync();
        var amazingOffers = await _context.Products.Where(p => p.CatId == "7").AsNoTracking().ToListAsync();
        return new BaseHome { NewProduct = newProducts, Mobiles = mobile, MakeupList = makeup, Discounts = discounts, AmazingOffers = amazingOffers };
    }

    /// <summary>
    /// Get brand using id
    /// </summary>
    /// <param name="id">
    /// require to fetch brand
    /// </param>
    /// <returns>
    /// retrun brand related to id
    /// </returns>
    public async Task<Brand?> GetBrand(int id) => await _context.Brands.AsNoTracking().SingleOrDefaultAsync(b => b.Id == id);

    /// <summary>
    /// Get category using id
    /// </summary>
    /// <param name="id">
    /// require to fetch category
    /// </param>
    /// <returns>
    /// retrun category related to id
    /// </returns>
    public async Task<Category?> GetCategory(int id) => await _context.Categories.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);

    /// <summary>
    /// Get gallery using id
    /// </summary>
    /// <param name="id">
    /// require to fetch gallery
    /// </param>
    /// <returns>
    /// retrun gallery related to id
    /// </returns>
    public async Task<Gallery?> GetGallery(int id) => await _context.Galleries.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);

    /// <summary>
    /// Get newProduct using id
    /// </summary>
    /// <param name="id">
    /// require to fetch newProduct
    /// </param>
    /// <returns>
    /// retrun newProduct related to id
    /// </returns>
    public async Task<NewProduct?> GetNewProduct(int id) => await _context.NewProducts.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);

    /// <summary>
    /// Get product using id
    /// </summary>
    /// <param name="id">
    /// require to fetch product
    /// </param>
    /// <returns>
    /// retrun product related to id
    /// </returns>
    public async Task<Product?> GetProduct(int id) => await _context.Products.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);

    public async Task<bool> IsBrandAvailable(int id) => await _context.Brands.FindAsync(id) != null;
    public async Task<bool> IsCategoryAvailable(int id) => await _context.Categories.FindAsync(id) != null;
    public async Task<bool> IsGalleryAvailable(int id) => await _context.Galleries.FindAsync(id) != null;
    public async Task<bool> IsNewProductAvailable(int id) => await _context.NewProducts.FindAsync(id) != null;
    public async Task<bool> IsProductAvailable(int id) => await _context.Products.FindAsync(id) != null;

    #endregion

    #region Add
    /// <summary>
    /// Add a brand to brands in database
    /// </summary>
    /// <param name="brand">
    /// brand to be add
    /// </param>
    /// <returns>
    /// added brand
    /// </returns>
    private async Task<Brand> AddBrand(Brand brand)
    {
        await _context.Brands.AddAsync(brand);
        await _context.SaveChangesAsync();
        return brand;
    }

    /// <summary>
    /// Add category to db
    /// </summary>
    /// <param name="category">
    /// category to be add
    /// </param>
    /// <returns>
    /// added category
    /// </returns>
    public async Task<Category> AddCategory(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    /// <summary>
    /// Add a gallery to db
    /// </summary>
    /// <param name="gallery">
    /// gallery to be add
    /// </param>
    /// <returns>
    /// added gallery
    /// </returns>
    public async Task<Gallery> AddGallery(Gallery gallery)
    {
        await _context.Galleries.AddAsync(gallery);
        await _context.SaveChangesAsync();
        return gallery;
    }

    /// <summary>
    /// Add a new product to db
    /// </summary>
    /// <param name="newProduct">
    /// New product to be add
    /// </param>
    /// <returns>
    /// added new product
    /// </returns>
    public async Task<NewProduct> AddNewProduct(NewProduct newProduct)
    {
        await _context.NewProducts.AddAsync(newProduct);
        await _context.SaveChangesAsync();
        return newProduct;
    }

    /// <summary>
    /// Add a product to products in database
    /// </summary>
    /// <param name="product">
    /// product to be add
    /// </param>
    /// <returns>
    /// added product
    /// </returns>
    public async Task<Product> AddProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        await AddBrand( new Brand { ProductBrand = product.Brand });
        await _context.SaveChangesAsync();
        return product;
    }
    #endregion

    #region Update
    /// <summary>
    /// Update a brand
    /// </summary>
    /// <param name="brand">
    /// brand to be update
    /// </param>
    public async Task UpdateBrand(Brand brand)
    {
        _context.Brands.Update(brand);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update category
    /// </summary>
    /// <param name="category">
    /// category to be update
    /// </param>
    public async Task UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update a gallery
    /// </summary>
    /// <param name="gallery">
    /// gallery to be update
    /// </param>
    public async Task UpdateGallery(Gallery gallery)
    {
        _context.Galleries.Update(gallery);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update new product
    /// </summary>
    /// <param name="newProduct">
    /// New product to be update
    /// </param>
    public async Task UpdateNewProduct(NewProduct newProduct)
    {
        _context.NewProducts.Update(newProduct);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Update a product
    /// </summary>
    /// <param name="product">
    /// product to be update
    /// </param>
    public async Task UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Delete
    /// <summary>
    /// Delete a brand
    /// </summary>
    /// <param name="brand">
    /// Brand to be delete
    /// </param>
    public async Task DeleteBrand(Brand brand)
    {
        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete all brands
    /// </summary>
    public async Task DeleteAllBrands()
    {
        await _context.Brands.ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete category
    /// </summary>
    /// <param name="category">
    /// category to be delete
    /// </param>
    public async Task DeleteCategory(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete all categories
    /// </summary>
    public async Task DeleteAllCategories()
    {
        await _context.Categories.ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete a gallery
    /// </summary>
    /// <param name="gallery">
    /// gallery to be delete
    /// </param>
    public async Task DeleteGallery(Gallery gallery)
    {
        _context.Galleries.Remove(gallery);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete all galleries
    /// </summary>
    public async Task DeleteAllGalleries()
    {
        await _context.Galleries.ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete new product
    /// </summary>
    /// <param name="newProduct">
    /// New product to be delete
    /// </param>
    public async Task DeleteNewProduct(NewProduct newProduct)
    {
        _context.NewProducts.Remove(newProduct);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete all new products
    /// </summary>
    public async Task DeleteAllNewProducts()
    {
        await _context.NewProducts.ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="product">
    /// product to be delete
    /// </param>
    public async Task DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete all products
    /// </summary>
    public async Task DeleteAllProducts()
    {
        await _context.Products.ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }
    #endregion
}