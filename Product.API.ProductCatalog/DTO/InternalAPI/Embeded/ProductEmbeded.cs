using Product.API.ProductCatalog.DTO.InternalAPI.Request;
using Product.API.ProductCatalog.Infrastructure.Entities;

namespace Product.API.ProductCatalog.DTO.InternalAPI.Embeded
{
    public class ProductEmbeded
    {
        public List<ImageRequest> Images { get; set; }
    }
}
