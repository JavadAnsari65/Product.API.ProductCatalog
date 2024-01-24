using Microsoft.AspNetCore.Mvc;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;
using Product.API.ProductCatalog.DTO.InternalAPI.Response;
using Product.API.ProductCatalog.Infrastructure.Entities;

namespace Product.API.ProductCatalog.Application
{
    public interface IProductCatalog
    {
        //public List<ProductResponse> GetAllProduct();
        public List<ProductEntity> GetAllProduct();

        public ProductResponse AddProduct(ProductRequest product);
        public ProductResponse UpdateProduct(Guid productId, ProductResponse product);
        public ProductResponse DeleteProduct(Guid productId);
        public IQueryable<ProductEntity> CreateQuery();
        public List<ProductResponse> SearchProduct(string fieldName, string fieldValue);
    }
}
