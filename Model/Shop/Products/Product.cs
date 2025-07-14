using System.ComponentModel.DataAnnotations;
using Shop.Model.Shop.Brands;
using Shop.Model.Shop.Categories;
using Shop.Model.Shop.ProductCategories.Gallery;
using Swashbuckle.AspNetCore.Annotations;

namespace Shop.Model.Shop.Products
{
    public class Product
    {
        private Guid id = new();
        private string title = "";
        private bool isWarranty = false;
        private long count = 0;
        private string shortDescription = "";
        private string fullDescription = "";
        private bool isSpecial = false;
        private string discount = "";
        private float rate = 0.0f;
        private string price = "";
        private string icon = "";
        private DateTime dateTime = DateTime.Now;
        private Category? category;
        private Brand? brand;
        private Guid brand_id;
        private Guid cat_id;
        private IEnumerable<Gallery> galleries = [];

        public Guid Id
        {
            get => id;
            set => id = value;
        }
        [Required]
        public string Title
        {
            get => title;
            set => title = value;
        }
        [Required]
        public bool IsWarranty
        {
            get => isWarranty;
            set => isWarranty = value;
        }
        [Required]
        public long Count
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
        public bool IsSpecial
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
        [SwaggerSchema(Description = "The product icon", ReadOnly = true)]
        public string Icon
        {
            get => icon;
            set => icon = value;
        }
        [Required]
        public Guid CatId
        {
            get => cat_id;
            set => cat_id = value;
        }
        [Required]
        public Guid BrandId
        {
            get => brand_id;
            set => brand_id = value;
        }
        [SwaggerSchema(Description = "Category navigation", ReadOnly = true)]
        public Category? Category
        {
            get => category;
            set => category = value;
        }
        [SwaggerSchema(Description = "Brand navigation", ReadOnly = true)]
        public Brand? Brand
        {
            get => brand;
            set => brand = value;
        }
        [SwaggerSchema(Description = "Gallery navigation", ReadOnly = true)]
        public IEnumerable<Gallery> Galleries
        {
            get => galleries;
            set => galleries = value;
        }
        public DateTime DateTime
        {
            get => dateTime;
            set => dateTime = value;
        }
    }
}