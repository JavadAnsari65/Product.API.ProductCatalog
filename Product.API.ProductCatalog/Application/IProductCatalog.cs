using Microsoft.AspNetCore.Mvc;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;
using Product.API.ProductCatalog.DTO.InternalAPI.Response;
using Product.API.ProductCatalog.Extensions.ExtraClasses;
using Product.API.ProductCatalog.Infrastructure.Entities;

namespace Product.API.ProductCatalog.Application
{
    public interface IProductCatalog
    {
        //public List<ProductResponse> GetAllProduct();
        public ApiResponse<List<ProductResponse>> GetAllProduct();

        public ApiResponse<ProductResponse> AddProduct(ProductRequest product);
        //public ProductResponse UpdateProduct(Guid productId, ProductResponse product);
        public ApiResponse<ProductResponse> UpdateProduct(Guid productId, ProductRequest product);
        public ApiResponse<ProductResponse> DeleteProduct(ProductIdRequest delProduct);
        public IQueryable<ProductEntity> CreateQuery();
        public List<ProductResponse> SearchProduct(string fieldName, string fieldValue);
    }
}
