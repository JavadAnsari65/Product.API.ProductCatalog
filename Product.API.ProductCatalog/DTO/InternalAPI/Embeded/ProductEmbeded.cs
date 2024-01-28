using Newtonsoft.Json;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;

namespace Product.API.ProductCatalog.DTO.InternalAPI.Embeded
{
    public class ProductEmbeded
    {
        [JsonIgnore]
        public List<ImageRequest> Images { get; set; }
    }
}
