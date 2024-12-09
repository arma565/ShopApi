public class BaseCategory
{
    private IEnumerable<Product> mobiles = new List<Product>();
    private IEnumerable<Product> makeup_list = new List<Product>();
    private IEnumerable<Product> trends = new List<Product>();
    private IEnumerable<Product> sports = new List<Product>();
    private IEnumerable<Product> home_page_list = new List<Product>();

    public IEnumerable<Product> Mobiles { get => mobiles; set => mobiles = value; }
    public IEnumerable<Product> MakeupList { get => makeup_list; set => makeup_list = value; }
    public IEnumerable<Product> Trends { get => trends; set => trends = value; }
    public IEnumerable<Product> Sports { get => sports; set => sports = value; }
    public IEnumerable<Product> HomePageList { get => home_page_list; set => home_page_list = value; }
}