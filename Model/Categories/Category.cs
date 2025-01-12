using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

public class Category
{
    private int id = 0;
    private string title = "";
    private string description = "";
    private string icon = "";
    private ICollection<Product> products = new List<Product>();
    
    public int Id
    {
        get => id;
        set => id = value;
    }
    public string Title
    {
        get => title;
        set => title = value;
    }
    public string Description
    {
        get => description;
        set => description = value;
    }
    [SwaggerSchema(Description = "Category icon", ReadOnly = true)]
    public string Icon
    {
        get => icon;
        set => icon = value;
    }
    [SwaggerSchema(Description = "The products navigation", ReadOnly = true)]
    public ICollection<Product> Products => products;

}
