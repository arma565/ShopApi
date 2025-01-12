public class ProductCategory
{
    private string cat_name = "";
    private IEnumerable<Product> products = new List<Product>();
    public string CatName { get => cat_name; set => cat_name = value; }
    public IEnumerable<Product> Products { get => products; set => products = value; }
    
}