using Newtonsoft.Json;
using Product.API.ProductCatalog.DTO.ExternalAPI.Request;

namespace Product.API.ProductCatalog.DTO.ExternalAPI.Embeded
{
    public class ProductEmbeded
    {
        [JsonIgnore]
        public List<ImageRequest> Images { get; set; }
    }
}
