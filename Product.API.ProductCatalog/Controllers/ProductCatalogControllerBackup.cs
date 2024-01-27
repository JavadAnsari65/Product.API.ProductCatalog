//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Product.API.ProductCatalog.Application;
//using Product.API.ProductCatalog.DTO.InternalAPI.Request;
//using Product.API.ProductCatalog.DTO.InternalAPI.Response;
//using Product.API.ProductCatalog.Infrastructure.Entities;

//namespace Product.API.ProductCatalog.Controllers
//{
//    [Route("api/[controller]/[action]")]
//    [ApiController]
//    public class ProductCatalogController : ControllerBase
//    {
//        private readonly IProductCatalog _productCatalog;
//        public ProductCatalogController(IProductCatalog productCatalog)
//        {
//            _productCatalog = productCatalog;
//        }

//        [HttpGet]
//        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
//        {
//            var products = _productCatalog.GetAllProduct();
//            return Ok(products);
//        }

//        [HttpGet]
//        public ActionResult<ProductResponse> SearchProduct(string fieldName, string fieldValue)
//        {
//            var productResult = _productCatalog.SearchProduct(fieldName, fieldValue);
//            return Ok(productResult);
//        }

//        [HttpPost]
//        public ActionResult<ProductResponse> AddProduct(ProductRequest product)
//        {
//            var newProduct = _productCatalog.AddProduct(product);
//            return Ok(newProduct);
//        }

//        [HttpPut]
//        public ActionResult<ProductResponse> UpdateProduct(Guid productId, ProductResponse product)
//        {
//            var updateProduct = _productCatalog.UpdateProduct(productId, product);
//            return Ok(updateProduct);
//        }

//        [HttpDelete]
//        public ActionResult<ProductResponse> DeleteProduct(Guid productId)
//        {
//            var result = _productCatalog.DeleteProduct(productId);
//            return Ok(result);
//        }
//    }
//}
