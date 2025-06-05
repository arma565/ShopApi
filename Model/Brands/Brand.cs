using System.ComponentModel.DataAnnotations;
using Shop.Model.Products;
using Swashbuckle.AspNetCore.Annotations;

namespace Shop.Model.Brands
{
    public class Brand
    {
        private Guid id = new();
        private string product_brand = "";
        private readonly ICollection<Product> products = [];

        public Guid Id
        {
            get => id;
            set => id = value;
        }
        [Required]
        public string ProductBrand
        {
            get => product_brand;
            set => product_brand = value;
        }
        [SwaggerSchema(Description = "Product navigation", ReadOnly = true)]
        public ICollection<Product> Products => products;

    }
}