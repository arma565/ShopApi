using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Data;
using RealEstate.Models.Authentication;
using Shop.Data;
using Shop.Model.Shop.Brands;
using Shop.Model.Shop.Categories;
using Shop.Model.Shop.Main;
using Shop.Model.Shop.ProductCategories;
using Shop.Model.Shop.ProductCategories.Gallery;
using Shop.Model.Shop.Products;

namespace Shop.Service
{
    public class ShopService(AppDbContext context, UserManager<UserProfileIdentity> userManager,
SignInManager<UserProfileIdentity> signInManager,
ImageService imageService)
    {
        private readonly AppDbContext _context = context;
        private readonly UserManager<UserProfileIdentity> _userManager = userManager;
        private readonly SignInManager<UserProfileIdentity> _signInManager = signInManager;
        private readonly ImageService _imageService = imageService;

        #region Authentication

        /// <summary>
        /// This function return all registered users
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync().ConfigureAwait(false);
            var usersList = new List<User>();
            foreach (var userInUserManager in users)
            {
                var user = new User
                {
                    Id = userInUserManager.Id,
                    ProfileImagePath = userInUserManager.ProfileImageName,
                    FirstName = userInUserManager.FirstName!,
                    LastName = userInUserManager.LastName!,
                    AcceptTerms = userInUserManager.AcceptTerms,
                    UserName = userInUserManager.UserName ?? "",
                    Email = userInUserManager.Email ?? "",
                    PhoneNumber = userInUserManager.PhoneNumber ?? "",
                };
                usersList.Add(user);
            }
            return [.. usersList];
        }

        /// <summary>
        /// This function register a user in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterUser(Register model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            else
                return await _userManager.CreateAsync(
                       new UserProfileIdentity
                       {
                           UserName = model.UserName,
                           Email = model.Email,
                           AcceptTerms = model.AcceptTerms,
                       },
                       model.Password
                   ).ConfigureAwait(false);
        }


        /// <summary>
        /// This function delete all users
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAllUsers()
        {
            var users = await _userManager.Users.ToListAsync().ConfigureAwait(false);
            foreach (var user in users)
            {
                await _userManager.DeleteAsync(user).ConfigureAwait(false);
            }
            var rootFile = Path.Combine("wwwroot/images/auth");
            if (!(Directory.Exists(rootFile))){
                return;
            }
            var files = Directory.GetFiles(rootFile);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// This function Delete a user from identity store
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteUser(UserProfileIdentity user)
        {
            var environmentPath = _imageService.GetLocalImagesFullPath("auth");

            var filePath = Path.Combine(environmentPath, user?.ProfileImageName ?? "");

            if (File.Exists(filePath))
                File.Delete(filePath);

            return await _userManager.DeleteAsync(user!).ConfigureAwait(false);
        }

        /// <summary>
        /// Login user using username and password
        /// </summary>
        /// <param name="model">
        /// Login model containing username and password
        /// </param>
        /// <returns></returns>
        public async Task<SignInResult> LoginUser(Login model)
        {
            if (model is null)
            {
                return SignInResult.Failed;
            }
            return await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                false,
                false
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// This function create a token to reset password
        /// </summary>
        /// <param name="user">
        /// User account which needs reset
        /// </param>
        /// <returns></returns>
        public async Task<string> GenerateTokenToRecoverUser(UserProfileIdentity user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
        }

        /// <summary>
        /// Reset the user account password
        /// </summary>
        /// <param name="user">
        /// user account
        /// </param>
        /// <param name="token">
        /// Tokeen reset password
        /// </param>
        /// <param name="newPassword">
        /// new password of account
        /// </param>
        /// <returns></returns>
        public async Task<IdentityResult> ResetPassword(
            UserProfileIdentity user,
            string token,
            string newPassword
        )
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword).ConfigureAwait(false);
        }

        public async Task<IdentityResult> ChangePassword(UserProfileIdentity user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword).ConfigureAwait(false);
        }

        /// <summary>
        /// This function is useful to edit profile
        /// </summary>
        /// <param name="user">
        /// user account
        /// </param>
        /// <returns></returns>
        public async Task<IdentityResult> EditUserProfile(UserProfileIdentity user)
        {
            return await _userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<UserProfileIdentity?> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
        }

        public async Task<UserProfileIdentity?> FindUserByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
        }
        #endregion

        #region Shop

        /// <summary>
        /// Get Brands from database
        /// </summary>
        /// <returns>
        /// Return list of brands
        /// </returns>
        public async Task<IEnumerable<Brand>> GetBrands() => [.. await _context.Brands.AsNoTracking().ToListAsync()];


        /// <summary>
        /// Get Categories from database
        /// </summary>
        /// <returns>
        /// Return list of categories
        /// </returns>
        public async Task<IEnumerable<Category>> GetCategoreis() => [.. await _context.Categories.AsNoTracking().ToListAsync()];

        /// <summary>
        /// Get home page products items
        /// </summary>
        /// <returns>
        /// Return a BaseHome model
        /// </returns>
        public async Task<Home> GetHome()
        {
            var newProducts = await _context.Products.Where(p => p.DateTime >= DateTime.Now).OrderByDescending(p => p.DateTime).AsNoTracking().ToListAsync();
            var categories = await _context.Categories.Include(c => c.Products).AsNoTracking().ToListAsync();
            var productCategoryList = categories.Select(category => new ProductCategory
            {
                CatName = category.Title,
                Products = category.Products
            }).ToList();
            return new Home { NewProducts = newProducts, ProductCategories = productCategoryList };
        }

        /// <summary>
        /// Get product categories include products which are: mobile,makeup,trends,sports,home
        /// </summary>
        /// <returns>
        /// Return a ProductCategory model
        /// </returns>
        public async Task<IEnumerable<ProductCategory>> GetProductCategory()
        {
            var categories = await _context.Categories.Include(c => c.Products).AsNoTracking().ToListAsync();
            return [.. categories.Select(category => new ProductCategory {
                CatName = category.Title,
                Products = category.Products
            })];
        }

        /// <summary>
        /// Get all products from database
        /// </summary>
        /// <returns>
        /// Return list of products
        /// </returns>
        public async Task<IEnumerable<Product>> GetProducts() => [.. await _context.Products.AsNoTracking().ToListAsync()];

        /// <summary>
        /// Get brand using id
        /// </summary>
        /// <param name="id">
        /// require to fetch brand
        /// </param>
        /// <returns>
        /// retrun brand related to id
        /// </returns>
        public async Task<Brand> GetBrand(Guid id)
        {
            Brand? brand = await _context.Brands.AsNoTracking().SingleOrDefaultAsync(b => b.Id == id);
            if (brand == null)
            {
                return new Brand();
            }
            return brand;
        }

        /// <summary>
        /// Get category using id
        /// </summary>
        /// <param name="id">
        /// require to fetch category
        /// </param>
        /// <returns>
        /// retrun category related to id
        /// </returns>
        public async Task<Category> GetCategory(Guid id)
        {

            Category? category = await _context.Categories.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return new Category();
            }
            return category;

        }

        /// <summary>
        /// Get gallery using id
        /// </summary>
        /// <param name="id">
        /// require to fetch gallery
        /// </param>
        /// <returns>
        /// retrun gallery related to id
        /// </returns>
        public async Task<Gallery> GetGallery(Guid id)
        {

            Gallery? gallery = await _context.Galleries.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
            if (gallery == null)
            {
                return new Gallery();
            }
            return gallery;
        }

        /// <summary>
        /// Get a product using id
        /// </summary>
        /// <param name="id">
        /// require to fetch product
        /// </param>
        /// <returns>
        /// retrun product related to id
        /// </returns>
        public async Task<Product> GetProduct(Guid id)
        {
            Product? product = await _context.Products.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
            if (product == null)
            {
                return new Product();
            }
            return product;
        }

        /// <summary>
        /// Get list of products using catId
        /// </summary>
        /// <param name="catId">
        /// by this catId we can fetch products from db 
        /// </param>
        /// <returns>
        /// Return products using catId
        /// </returns>
        public async Task<IEnumerable<Product>> GetProducts(Guid catId) => await _context.Products.Where(p => p.CatId == catId).AsNoTracking().ToListAsync();

        public async Task<bool> IsBrandAvailable(Brand brand) => await _context.Brands.AsNoTracking().ContainsAsync(brand);
        public async Task<bool> IsCategoryAvailable(Category category) => await _context.Categories.AsNoTracking().ContainsAsync(category);
        public async Task<bool> IsGalleryAvailable(Gallery gallery) => await _context.Galleries.AsNoTracking().ContainsAsync(gallery);
        public async Task<bool> IsProductAvailable(Product product) => await _context.Products.AsNoTracking().ContainsAsync(product);

        /// <summary>
        /// Add a brand to database
        /// </summary>
        /// <param name="brand">
        /// brand parameter
        /// </param>
        /// <returns>
        /// added brand
        /// </returns>
        public async Task<Brand> AddBrand(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        /// <summary>
        /// Add a category to database
        /// </summary>
        /// <param name="category">
        /// category parameter
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
        /// Upload gallery image
        /// </summary>
        /// <param name="image">
        /// image parameter of product
        /// </param>
        /// <returns></returns>
        public async Task<Gallery> AddGallery(Gallery gallery)
        {
            await _context.Galleries.AddAsync(gallery);
            await _context.SaveChangesAsync();
            return gallery;
        }

        /// <summary>
        /// Add a product to database
        /// </summary>
        /// <param name="product">
        /// product parameter
        /// </param>
        /// <returns>
        /// added product
        /// </returns>
        public async Task<Product> AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Update a brand
        /// </summary>
        /// <param name="brand">
        /// brand parameter
        /// </param>
        public async Task UpdateBrand(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update a category
        /// </summary>
        /// <param name="category">
        /// category parameter
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
        /// gallery parameter
        /// </param>
        public async Task UpdateGallery(Gallery gallery)
        {
            _context.Galleries.Update(gallery);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="product">
        /// product parameter
        /// </param>
        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a brand
        /// </summary>
        /// <param name="brand">
        /// Brand parameter
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
        /// Delete a category
        /// </summary>
        /// <param name="category">
        /// category parameter
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
        /// gallery parameter
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
        /// Delete a product
        /// </summary>
        /// <param name="product">
        /// product parameter
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
}