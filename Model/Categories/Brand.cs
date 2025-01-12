using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

public class Brand
{
    private int id = 0;
    private string product_brand = "";
    private ICollection<Product> products = new List<Product>();

    public int Id { get => id; set => id = value; }
    public string ProductBrand
    {
        get => product_brand;
        set => product_brand = value;
    }
    [SwaggerSchema(Description = "Product navigation", ReadOnly = true)]
    public ICollection<Product> Products => products;

}
