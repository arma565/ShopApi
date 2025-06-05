using Microsoft.AspNetCore.Mvc;
using Shop.Model.Brands;
using Shop.Model.Categories;
using Shop.Model.CategoryProducts;
using Shop.Model.Main;
using Shop.Model.ProductCategories.Gallery;
using Shop.Model.Products;
using Shop.Service;

namespace Shop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly ShopService _service;
        private readonly ImageService _imageService;

        public ShopController(ShopService service, ImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        #region Upload/Download

        [HttpPost("gallery/upload/{productId}")]
        public async Task<IActionResult> UploadGalleryImage(Guid productId, IFormFile image)
        {
            var product = await _service.GetProduct(productId);
            if (product == null) return NotFound("Product not found!");
            if (image != null && image!.Length > 0)
            {
                var imageUrl = await _imageService.UploadImage(image);
                // Add the image to the gallery
                var gallery = new Gallery
                {
                    ProductId = productId,
                    Img = imageUrl!
                };
                var res = await _service.AddGallery(gallery);
                return Ok(res);
            }
            else
            {
                return BadRequest("Image not provided!");
            }

        }

        [HttpPost("category/upload/{id}")]
        public async Task<IActionResult> UploadCategoryImage(Guid id, IFormFile image)
        {
            var category = await _service.GetCategory(id);
            if (category == null) return NotFound("Category not found!");
            if (image != null && image!.Length > 0)
            {
                var imageUrl = await _imageService.UploadImage(image);
                // Update category
                category.Icon = imageUrl;
                await _service.UpdateCategory(category);
                return NoContent();
            }
            else
            {
                return BadRequest("Image not provided!");
            }
        }

        [HttpPost("product/upload/{id}")]
        public async Task<IActionResult> UploadProductImage(Guid id, IFormFile image)
        {
            var product = await _service.GetProduct(id);
            if (product == null) return NotFound("Product not found!");
            if (image != null && image!.Length > 0)
            {
                var imageUrl = await _imageService.UploadImage(image);
                // Update category
                product.Icon = imageUrl;
                await _service.UpdateProduct(product);
                return NoContent();
            }
            else
            {
                return BadRequest("Image not provided!");
            }
        }

        [HttpGet("download")]
        public IActionResult DownloadImage([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return BadRequest("Invalid URL");
            }
            Uri uri = new(url);
            var fileName = Path.GetFileName(uri.LocalPath);

            //check if filename is not empty or null
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name can't be empty!");
            }
            if (fileName.Contains("..") || Path.GetInvalidFileNameChars().Any(fileName.Contains))
            {
                return BadRequest("Invalid file name.");
            }

            //check directory if exist
            var imageDir = Path.Combine("wwwroot", "images");
            var filePath = Path.Combine(imageDir, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }
            // Get the file's content type
            var fileExtension = Path.GetExtension(fileName).ToLower();
            var contentType = fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream",
            };

            try
            {
                // Open the file stream asynchronously
                FileStream stream = _imageService.ReadImage(filePath);
                // Return the file stream as FileContentResult
                return File(stream, contentType, fileName);
            }
            catch (IOException ex)
            {
                // Log the error if needed
                return StatusCode(500, "Error reading the file!. Error =" + ex.Message);
            }
        }
        #endregion

        #region GetAll

        [HttpGet("shop/getBrands")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands() => Ok(await _service.GetBrands());
       
        [HttpGet("shop/getCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories() => Ok(await _service.GetCategoreis());
      
        [HttpGet("shop/getHome")]
        public async Task<ActionResult<Home>> GetHome() => Ok(await _service.GetHome());

        [HttpGet("shop/getProductCategory")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategory() => Ok(await _service.GetProductCategory());

        [HttpGet("shop/getProducts")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetPoducts() => Ok(await _service.GetProducts());
        #endregion

        #region GetUsingID

        [HttpGet("shop/getBrand/{id}")]
        public async Task<ActionResult<Brand>> GetBrand(Guid id)
        {
            var brand = await _service.GetBrand(id);
            if (brand == null)
            {
                return BadRequest("Brand not available!");
            }
            return Ok(brand);
        }

        [HttpGet("shop/getCategory/{id}")]
        public async Task<ActionResult<Brand>> GetCategory(Guid id)
        {
            var category = await _service.GetCategory(id);
            if (category == null)
            {
                return BadRequest("Category not available!");
            }
            return Ok(category);
        }

        [HttpGet("shop/getGallery/{id}")]
        public async Task<ActionResult<Gallery>> GetGallery(Guid id)
        {
            var gallery = await _service.GetGallery(id);
            if (gallery == null)
            {
                return BadRequest("Gallery not available!");
            }
            return Ok(gallery);
        }

        [HttpGet("shop/getProduct/{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _service.GetProduct(id);
            if (product == null)
            {
                return BadRequest("Product is not available!");
            }
            return Ok(product);
        }

        [HttpPost("shop/getProducts/{catId}")]
        public async Task<IEnumerable<Product>> GetProducts(Guid catId) => await _service.GetProducts(catId);
        #endregion

        #region Add

        [HttpPost("shop/addBrand")]
        public async Task<IActionResult> AddBrand([FromBody] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addedBrand = await _service.AddBrand(brand);
            return CreatedAtAction(nameof(GetBrand), new { id = addedBrand.Id }, addedBrand);
        }

        [HttpPost("shop/addCategory")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedCategory = await _service.AddCategory(category);
            return CreatedAtAction(nameof(GetCategory), new { id = addedCategory.Id }, addedCategory);
        }

        [HttpPost("shop/addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedProduct = await _service.AddProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id = addedProduct.Id }, addedProduct);
        }
        #endregion

        #region Update

        [HttpPut("shop/updateBrand")]
        public async Task<IActionResult> UpdateBrand([FromBody] Brand brand)
        {
            if (!(await _service.IsBrandAvailable(brand)))
            {
                return NotFound("Brand not found!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.UpdateBrand(brand);
            return NoContent();
        }

        [HttpPut("shop/updateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            if (!(await _service.IsCategoryAvailable(category)))
            {
                return NotFound("Category not found!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.UpdateCategory(category);
            return NoContent();
        }

        [HttpPut("shop/updateGallery")]
        public async Task<IActionResult> UpdateGallery([FromBody] Gallery gallery)
        {
            if (!(await _service.IsGalleryAvailable(gallery)))
            {
                return NotFound("Gallery not found!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.UpdateGallery(gallery);
            return NoContent();
        }

        [HttpPut("shop/updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (!(await _service.IsProductAvailable(product)))
            {
                return NotFound("Product not found!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.UpdateProduct(product);
            return NoContent();
        }
        #endregion

        #region DeleteAll

        [HttpDelete("shop/deleteAllBrands")]
        public async Task<IActionResult> DeleteAllBrands()
        {
            await _service.DeleteAllBrands();
            return NoContent();
        }

        [HttpDelete("shop/deleteAllCategories")]
        public async Task<IActionResult> DeleteAllCategories()
        {
            await _service.DeleteAllCategories();
            return NoContent();
        }

        [HttpDelete("shop/deleteAllGalleries")]
        public async Task<IActionResult> DeleteAllGalleries()
        {
            await _service.DeleteAllGalleries();
            return NoContent();
        }

        [HttpDelete("shop/deleteAllProducts")]
        public async Task<IActionResult> DeleteAllProducts()
        {
            await _service.DeleteAllProducts();
            return NoContent();
        }
        #endregion

        #region DeleteUsingID

        [HttpDelete("shop/deleteBrand/{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            var brand = await _service.GetBrand(id);
            if (brand == null)
            {
                return NotFound("Brand not found!");
            }
            await _service.DeleteBrand(brand);
            return NoContent();
        }

        [HttpDelete("shop/deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _service.GetCategory(id);
            if (category == null)
            {
                return NotFound("Category not found!");
            }
            await _service.DeleteCategory(category);
            return NoContent();
        }

        [HttpDelete("shop/deleteGallery/{id}")]
        public async Task<IActionResult> DeleteGallery(Guid id)
        {
            var gallery = await _service.GetGallery(id);
            if (gallery == null)
            {
                return NotFound("Gallery not found!");
            }
            await _service.DeleteGallery(gallery);
            return NoContent();
        }

        [HttpDelete("shop/deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _service.GetProduct(id);
            if (product == null)
            {
                return NotFound("product not found!");
            }
            await _service.DeleteProduct(product);
            return NoContent();
        }

        #endregion
    }
}