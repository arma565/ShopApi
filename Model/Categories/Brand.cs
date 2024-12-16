using System.ComponentModel.DataAnnotations;

public class Brand
{
    private int id = 0;
    private string product_brand = "";

    public int Id { get => id; set => id = value; }
    [Required]
    public string ProductBrand
    {
        get => product_brand;
        set => product_brand = value;
    }

}
