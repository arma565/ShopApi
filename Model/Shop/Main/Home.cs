using Shop.Model.Shop.ProductCategories;
using Shop.Model.Shop.Products;


namespace Shop.Model.Shop.Main
{
    public class Home
    {
        private IEnumerable<Product> newProducts = [];

        private IEnumerable<ProductCategory> product_category = [];

        public IEnumerable<ProductCategory> ProductCategories
        {
            get => product_category;
            set => product_category = value;
        }

        public IEnumerable<Product> NewProducts
        {
            get => newProducts;
            set => newProducts = value;
        }
    }
}