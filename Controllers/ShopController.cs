using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("controller")]
public class ShopController : ControllerBase
{
    private readonly ShopService _service;
    private readonly ImageService _imageService;
    private readonly ILogger<ShopService> _logger;

    public ShopController(ShopService service,
    ImageService imageService,
    ILogger<ShopService> logger)
    {
        _imageService = imageService;
        _service = service;
        _logger = logger;
    }

    [HttpPost("gallery/upload/{productId}")]
    public async Task<IActionResult> UploadGalleryImage(int productId, IFormFile image)
    {
        var product = await _service.GetProduct(productId);
        if (product == null) return NotFound("Product not found!");
        if (image != null || image!.Length > 0)
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
    public async Task<IActionResult> UploadCategoryImage(int id, IFormFile image)
    {
        var category = await _service.GetCategory(id);
        if (category == null) return NotFound("Category not found!");
        if (image != null || image!.Length > 0)
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

    [HttpPost("newProduct/upload/{id}")]
    public async Task<IActionResult> UploadNewProductImage(int id, IFormFile image)
    {
        var newProduct = await _service.GetNewProduct(id);
        if (newProduct == null) return NotFound("NewProduct not found!");
        if (image != null || image!.Length > 0)
        {
            var imageUrl = await _imageService.UploadImage(image);
            // Update category
            newProduct.Icon = imageUrl;
            await _service.UpdateNewProduct(newProduct);
            return NoContent();
        }
        else
        {
            return BadRequest("Image not provided!");
        }
    }

    [HttpPost("product/upload/{id}")]
    public async Task<IActionResult> UploadProductImage(int id, IFormFile image)
    {
        var product = await _service.GetProduct(id);
        if (product == null) return NotFound("Product not found!");
        if (image != null || image!.Length > 0)
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

    [HttpGet("download/{url}")]
    public IActionResult DownloadImage(string url)
    {
        if (string.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return BadRequest("Invalid URL");
        }
        Uri uri = new Uri(url);
        var fileName = Path.GetFileName(uri.LocalPath);
        Console.WriteLine(fileName);
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
        var filePath = Path.Combine(
            imageDir,
            fileName.Trim().Replace(" ", "").Replace("-", "").Replace("_", "")
        );
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

    #region Get
    [HttpGet("shop/getBaseCategories")]
    public async Task<BaseCategories> GetBaseCategories() => await _service.GetBaseCategories();

    [HttpGet("shop/getProductCategories")]
    public async Task<IEnumerable<ProductCategory>> GetProductCategories() => [..await _service.GetProductCategories()];

    [HttpGet("shop/getNewProducts")]
    public async Task<IEnumerable<NewProduct>> GetNewProducts() => await _service.GetNewProducts();

    [HttpPost("shop/getProducts/{catId}")]
    public async Task<IEnumerable<Product>> GetProducts(int catId) => await _service.GetProducts(catId);

    [HttpGet("shop/getHome")]
    public async Task<BaseHome> GetHome() => await _service.GetHome();

    [HttpGet("shop/getBrand/{id}")]
    public async Task<ActionResult<Brand>> GetBrand(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id");
        }

        var brand = await _service.GetBrand(id);
        if (brand == null)
        {
            return BadRequest("Brand not available!");
        }
        return Ok(brand);
    }

    [HttpGet("shop/getCategory/{id}")]
    public async Task<ActionResult<Brand>> GetCategory(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id");
        }

        var category = await _service.GetCategory(id);
        if (category == null)
        {
            return BadRequest("Category not available!");
        }
        return Ok(category);
    }

    [HttpGet("shop/getGallery/{id}")]
    public async Task<ActionResult<Gallery>> GetGallery(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id");
        }

        var gallery = await _service.GetGallery(id);
        if (gallery == null)
        {
            return BadRequest("Gallery not available!");
        }
        return Ok(gallery);
    }

    [HttpGet("shop/getNewProduct/{id}")]
    public async Task<ActionResult<NewProduct>> GetNewProduct(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id");
        }

        var newProduct = await _service.GetNewProduct(id);
        if (newProduct == null)
        {
            return BadRequest("newProduct not available!");
        }
        return Ok(newProduct);
    }

    [HttpGet("shop/getProduct/{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id");
        }

        var product = await _service.GetProduct(id);
        if (product == null)
        {
            return BadRequest("product not available!");
        }
        return Ok(product);
    }
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

    [HttpPost("shop/addNewProduct")]
    public async Task<IActionResult> AddNewProduct([FromBody] NewProduct newProduct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var addedNewproduct = await _service.AddNewProduct(newProduct);
        return CreatedAtAction(nameof(GetNewProduct), new { id = addedNewproduct.Id }, addedNewproduct);
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
        if (brand.Id <= 0)
        {
            return BadRequest("Updating brand is not possible without id!");
        }
        if (!await _service.IsBrandAvailable(brand))
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
        if (category.Id <= 0)
        {
            return BadRequest("Updating category is not possible without id!");
        }
        if (!await _service.IsCategoryAvailable(category))
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
        if (gallery.Id <= 0)
        {
            return BadRequest("Updating gallery is not possible without id!");
        }
        if (!await _service.IsGalleryAvailable(gallery))
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

    [HttpPut("shop/updateNewProduct")]
    public async Task<IActionResult> UpdateNewProduct([FromBody] NewProduct newProduct)
    {
        if (newProduct.Id <= 0)
        {
            return BadRequest("Updating newProduct is not possible without id!");
        }
        if (!await _service.IsNewProductAvailable(newProduct))
        {
            return NotFound("NewProduct not found!");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _service.UpdateNewProduct(newProduct);
        return NoContent();
    }

    [HttpPut("shop/updateProduct")]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        if (product.Id <= 0)
        {
            return BadRequest("Updating product is not possible without id!");
        }
        if (!await _service.IsProductAvailable(product))
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

    #region Delete
    [HttpDelete("shop/deleteBrand/{id}")]
    public async Task<IActionResult> DeleteBrand(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Deleting brand is not possible without id!");
        }
        var brand = await _service.GetBrand(id);
        if (brand == null)
        {
            return NotFound("Brand not found!");
        }
        await _service.DeleteBrand(brand);
        return NoContent();
    }

    [HttpDelete("shop/deleteAllBrands")]
    public async Task<IActionResult> DeleteAllBrands()
    {
        await _service.DeleteAllBrands();
        return NoContent();
    }

    [HttpDelete("shop/deleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Deleting category is not possible without id!");
        }
        var category = await _service.GetCategory(id);
        if (category == null)
        {
            return NotFound("Category not found!");
        }
        await _service.DeleteCategory(category);
        return NoContent();
    }

    [HttpDelete("shop/deleteAllCategories")]
    public async Task<IActionResult> DeleteAllCategories()
    {
        await _service.DeleteAllCategories();
        return NoContent();
    }

    [HttpDelete("shop/deleteGallery/{id}")]
    public async Task<IActionResult> DeleteGallery(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Deleting gallery is not possible without id!");
        }
        var gallery = await _service.GetGallery(id);
        if (gallery == null)
        {
            return NotFound("Gallery not found!");
        }
        await _service.DeleteGallery(gallery);
        return NoContent();
    }

    [HttpDelete("shop/deleteAllGalleries")]
    public async Task<IActionResult> DeleteAllGalleries()
    {
        await _service.DeleteAllGalleries();
        return NoContent();
    }

    [HttpDelete("shop/deleteNewProduct/{id}")]
    public async Task<IActionResult> DeleteNewProduct(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Deleting newProduct is not possible without id!");
        }
        var newProduct = await _service.GetNewProduct(id);
        if (newProduct == null)
        {
            return NotFound("newProduct not found!");
        }
        await _service.DeleteNewProduct(newProduct);
        return NoContent();
    }

    [HttpDelete("shop/deleteAllNewProducts")]
    public async Task<IActionResult> DeleteAllNewProducts()
    {
        await _service.DeleteAllNewProducts();
        return NoContent();
    }

    [HttpDelete("shop/deleteProduct/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Deleting product is not possible without id!");
        }
        var product = await _service.GetProduct(id);
        if (product == null)
        {
            return NotFound("product not found!");
        }
        await _service.DeleteProduct(product);
        return NoContent();
    }

    [HttpDelete("shop/deleteAllProducts")]
    public async Task<IActionResult> DeleteAllProducts()
    {
        await _service.DeleteAllProducts();
        return NoContent();
    }
    #endregion
}