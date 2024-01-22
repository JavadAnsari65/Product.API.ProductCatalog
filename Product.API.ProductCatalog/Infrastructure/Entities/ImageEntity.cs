using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Product.API.ProductCatalog.Infrastructure.Entities
{
    public class ImageEntity
    {
        [Key]
        public int ImageId { get; set; }
        public Guid ProductId { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public ProductEntity Product { get; set; }
    }
}
