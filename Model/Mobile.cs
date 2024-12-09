public class Mobile
{
    int id = 0;
    string catId = "0";
    string catName = "";
    string title = "";
    string brand = "";
    string warranty = "";
    string count = "";
    string shortDescription = "";
    string fullDescription = "";
    string special = "";
    string discount = "";
    string rate = "";
    string price = "";
    string icon = "";
    IEnumerable<Gallery> galleries = new List<Gallery>();

    public int Id
    {
        get => id;
        set => id = value;
    }
    public string CatId
    {
        get => catId;
        set => catId = value;
    }
    public string CatName
    {
        get => catName;
        set => catName = value;
    }
    public string Title
    {
        get => title;
        set => title = value;
    }
    public string Brand
    {
        get => brand;
        set => brand = value;
    }
    public string Warranty
    {
        get => warranty;
        set => warranty = value;
    }
    public string Count
    {
        get => count;
        set => count = value;
    }
    public string ShortDescription
    {
        get => shortDescription;
        set => shortDescription = value;
    }
    public string FullDescription
    {
        get => fullDescription;
        set => fullDescription = value;
    }

    public string Special
    {
        get => special;
        set => special = value;
    }
    public string Discount
    {
        get => discount;
        set => discount = value;
    }
    public string Rate
    {
        get => rate;
        set => rate = value;
    }
    public string Price
    {
        get => price;
        set => price = value;
    }
    public string Icon
    {
        get => icon;
        set => icon = value;
    }
    public IEnumerable<Gallery> Galleries
    {
        get => galleries;
        set => galleries = value;
    }
}
