using Product.API.ProductCatalog.DTO.InternalAPI.Embeded;

namespace Product.API.ProductCatalog.DTO.InternalAPI.Request
{
    public class ProductRequest
    {
        public int ProductCatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        //public ProductEmbeded Images { get; set; }
        public List<ImageRequest> Images { get; set; }
    }
}
