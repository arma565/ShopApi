using Shop.Model.Products;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model.ProductCategories.Gallery
{
    public class Gallery
    {
        private Guid id = new();
        private string img = "";
        private Guid product_id = new();
        private Product? product = null;

        public Guid Id
        {
            get => id;
            set => id = value;
        }

        [Required]
        public string Img
        {
            get => img;
            set => img = value;
        }
        [Required]
        public Guid ProductId
        {
            get => product_id;
            set => product_id = value;
        }
        public Product? Product
        {
            get => product;
            set => product = value;
        }
    }
}