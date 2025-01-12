using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
public class Product
{
    private int id = 0;
    private string title = "";
    private bool isWarranty = false;
    private int count = 0;
    private string shortDescription = "";
    private string fullDescription = "";
    private bool isSpecial = false;
    private string discount = "";
    private float rate = 0.0f;
    private string price = "";
    private string icon = "";
    private Category? category;
    private Brand? brand;
     private int brand_id;
    private int cat_id;
    private IEnumerable<Gallery> galleries = new List<Gallery>();

    public int Id
    {
        get => id;
        set => id = value;
    }
    [Required]
    public string Title
    {
        get => title;
        set => title = value;
    }
    [Required]
    public bool IsWarranty
    {
        get => isWarranty;
        set => isWarranty = value;
    }
    [Required]
    public int Count
    {
        get => count;
        set => count = value;
    }
    [Required]
    public string ShortDescription
    {
        get => shortDescription;
        set => shortDescription = value;
    }
    [Required]
    public string FullDescription
    {
        get => fullDescription;
        set => fullDescription = value;
    }
    [Required]
    public bool IsSpecial
    {
        get => isSpecial;
        set => isSpecial = value;
    }
    [Required]
    public string Discount
    {
        get => discount;
        set => discount = value;
    }
    [Required]
    public float Rate
    {
        get => rate;
        set => rate = value;
    }
    [Required]
    public string Price
    {
        get => price;
        set => price = value;
    }
    [SwaggerSchema(Description = "The product icon", ReadOnly = true)]
    public string Icon
    {
        get => icon;
        set => icon = value;
    }
    [Required]
    public int CatId
    {
        get => cat_id;
        set => cat_id = value;
    }
    [Required]
    public int BrandId
    {
        get => brand_id;
        set => brand_id = value;
    }
    [SwaggerSchema(Description = "Category navigation", ReadOnly = true)]
    public Category? Category
    {
        get => category;
        set => category = value;
    }
    [SwaggerSchema(Description = "Brand navigation", ReadOnly = true)]
    public Brand? Brand
    {
        get => brand;
        set => brand = value;
    }
    [SwaggerSchema(Description = "Gallery navigation", ReadOnly = true)]
    public IEnumerable<Gallery> Galleries
    {
        get => galleries;
        set => galleries = value;
    }

}