using System.ComponentModel.DataAnnotations;

public class Product
{
    int id = 0;
    string catId = "0";
    string catName = "";
    string title = "";
    string brand = "";
    bool isWarranty = false;
    int count = 0;
    string shortDescription = "";
    string fullDescription = "";
    bool isSpecial = false;
    string discount = "";
    float rate = 0.0f;
    string price = "";
    string icon = "";
    IEnumerable<Gallery> galleries = new List<Gallery>();

    public int Id
    {
        get => id;
        set => id = value;
    }

    [Required]
    public string CatId
    {
        get => catId;
        set => catId = value;
    }
    [Required]
    public string CatName
    {
        get => catName;
        set => catName = value;
    }
    [Required]
    public string Title
    {
        get => title;
        set => title = value;
    }
    [Required]
    public string Brand
    {
        get => brand;
        set => brand = value;
    }
    [Required]
    public bool Warranty
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
    public bool Special
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
    [Required]
    public string Icon
    {
        get => icon;
        set => icon = value;
    }
    [Required]
    public IEnumerable<Gallery> Galleries
    {
        get => galleries;
        set => galleries = value;
    }
}