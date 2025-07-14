using Shop.Model.Shop.Products;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model.Shop.Categories
{
    public class Category
    {
        private Guid id = new();
        private string title = "";
        private string description = "";
        private string icon = "";
        private ICollection<Product> products = [];

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
        public string Description
        {
            get => description;
            set => description = value;
        }
        [SwaggerSchema(Description = "Category icon", ReadOnly = true)]
        public string Icon
        {
            get => icon;
            set => icon = value;
        }

        [SwaggerSchema(Description = "The products navigation", ReadOnly = true)]
        public ICollection<Product> Products { get => products; set => products = value; }
    }
}