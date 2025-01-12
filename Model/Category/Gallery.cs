using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

public class Gallery
{
    private int id = 0;
    private string img = "";
    private int product_id = 0;
    private Product? product = null;

    public int Id { get => id; set => id = value; }
    [Required]
    public string Img
    {
        get => img;
        set => img = value;
    }
    [Required]
    public int ProductId { get => product_id; set => product_id = value; }
    public Product? Product { get => product; set => product = value; }
}
