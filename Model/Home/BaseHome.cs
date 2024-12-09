public class BaseHome
{
    IEnumerable<News> news = new List<News>();
    IEnumerable<Product> mobiles = new List<Product>();
    IEnumerable<Product> makeupList = new List<Product>();
    IEnumerable<Product> discounts = new List<Product>();
    IEnumerable<Product> amazingOffers = new List<Product>();

    public IEnumerable<News> News { get => news; set => news = value; }
    public IEnumerable<Product> Mobiles { get => mobiles; set => mobiles = value; }
    public IEnumerable<Product> MakeupList { get => makeupList; set => makeupList = value; }
    public IEnumerable<Product> Discounts { get => discounts; set => discounts = value; }
    public IEnumerable<Product> AmazingOffers { get => amazingOffers; set => amazingOffers = value; }
}