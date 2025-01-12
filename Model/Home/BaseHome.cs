public class BaseHome
{
    private IEnumerable<NewProduct> newProduct = new List<NewProduct>();
    private IEnumerable<ProductCategory> product_category = new List<ProductCategory>();

    public IEnumerable<NewProduct> NewProduct { get => newProduct; set => newProduct = value; }
    public IEnumerable<ProductCategory> ProductCategories { get => product_category; set => product_category = value; }
    
}