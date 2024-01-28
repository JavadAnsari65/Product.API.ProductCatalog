using Product.API.ProductCatalog.DTO.ExternalAPI.Embeded;

namespace Product.API.ProductCatalog.DTO.ExternalAPI.Request
{
    public class ProductRequest
    {
        public int ProductCatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        //public ProductEmbeded Images { get; set; }
        public List<ProductEmbeded> Images { get; set; }
    }
}
