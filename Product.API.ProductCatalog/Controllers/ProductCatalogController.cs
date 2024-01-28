using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.ProductCatalog.Application;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;

namespace Product.API.ProductCatalog.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductCatalogController : ControllerBase
    {
        private readonly IProductCatalog _productCatalog;
        private readonly IMapper _mapper;
        public ProductCatalogController(IProductCatalog productCatalog, IMapper mapper)
        {
            _productCatalog = productCatalog;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<DTO.ExternalAPI.Response.ProductResponse>> GetProducts()
        {
            var getResult = _productCatalog.GetAllProduct();
            if (getResult.Result)
            {
                var products = _mapper.Map<List<DTO.ExternalAPI.Response.ProductResponse>>(getResult.Data);
                return Ok(products);
            }
            else
            {
                return BadRequest(getResult.ErrorMessage);
            }
        }

        
        //[HttpGet]
        //public ActionResult<List<DTO.ExternalAPI.Response.ProductResponse>> SearchProduct(string fieldName, string fieldValue)
        //{
        //    var productResult = _productCatalog.SearchProduct(fieldName, fieldValue);
        //    return Ok(productResult);
        //}
        
        [HttpPost]
        public ActionResult<DTO.ExternalAPI.Response.ProductResponse> AddProduct(DTO.ExternalAPI.Request.ProductRequest product)
        {
            
            try
            {
                var newInternalProduct = _mapper.Map<ProductRequest>(product);
                var newProduct = _productCatalog.AddProduct(newInternalProduct);

                if (newProduct.Result)
                {
                    var mapExternalProduct = _mapper.Map<DTO.ExternalAPI.Response.ProductResponse>(newProduct.Data);
                    return Ok(mapExternalProduct);
                }
                else
                {
                    return BadRequest(newProduct.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public ActionResult<DTO.ExternalAPI.Response.ProductResponse> UpdateProduct(Guid productId, DTO.ExternalAPI.Request.ProductRequest product)
        {
            try
            {
                var mapInternalProduct = _mapper.Map<ProductRequest>(product);

                var updateProduct = _productCatalog.UpdateProduct(productId, mapInternalProduct);

                if (updateProduct.Result)
                {
                    var mapExternalProduct = _mapper.Map<DTO.ExternalAPI.Response.ProductResponse>(updateProduct.Data);
                    return Ok(mapExternalProduct);
                }
                else
                {
                    return BadRequest(updateProduct.ErrorMessage);
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        public ActionResult<DTO.ExternalAPI.Response.ProductResponse> DeleteProduct(DTO.ExternalAPI.Request.ProductIdRequest product)
        {
            try
            {
                var mapInternalPId = _mapper.Map<ProductIdRequest>(product);
                var result = _productCatalog.DeleteProduct(mapInternalPId);

                if(result.Result)
                {
                    var mapExternal = _mapper.Map<DTO.ExternalAPI.Response.ProductResponse>(result.Data);
                    return Ok(mapExternal);
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
