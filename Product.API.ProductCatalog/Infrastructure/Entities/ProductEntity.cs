using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations;

namespace Product.API.ProductCatalog.Infrastructure.Entities
{
    public class ProductEntity
    {
        //Properties
        [Key]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
        public bool IsApproved { get; set; } = false;
        public List<ImageEntity> Images { get; set; }

    }
}
