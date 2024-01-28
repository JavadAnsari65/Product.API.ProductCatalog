using Product.API.ProductCatalog.DTO.InternalAPI.Embeded;

namespace Product.API.ProductCatalog.DTO.InternalAPI.Request
{
    public class ProductRequest
    {
        public int ProductCatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<ProductEmbeded> Images { get; set; }
    }
}
