public class BaseCategories
{
    private IEnumerable<Category> categories = new List<Category>();
    private IEnumerable<Brand> brands= new List<Brand>();

    public IEnumerable<Category> Categories { get => categories; set => categories = value; }
    public IEnumerable<Brand> Brands { get => brands; set => brands = value; }
}