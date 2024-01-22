using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.ProductCatalog.DTO.InternalAPI.Embeded;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;
using Product.API.ProductCatalog.DTO.InternalAPI.Response;
using Product.API.ProductCatalog.Extensions.SearchClasses;
using Product.API.ProductCatalog.Infrastructure.Entities;
using Product.API.ProductCatalog.Infrastructure.Repository;
using Product.API.ProductCatalog.wwwroot.StaticFiles;
using System.Linq.Expressions;

namespace Product.API.ProductCatalog.Application
{
    public class ProductCatalog : IProductCatalog
    {
        private readonly IMapper _mapper;
        private readonly CRUDService _crudServices;
        public ProductCatalog(IMapper mapper, CRUDService addService)
        {
            _mapper = mapper;
            _crudServices = addService;
        }

        //public List<ProductEntity> GetAllProduct()
        public List<ProductResponse> GetAllProduct()
        {
            var products = _crudServices.GetAllProductOfDB();
            foreach (var product in products)
            {
                foreach (var image in product.Images)
                {
                    image.ImageUrl = StaticUrls.ProductImageUrl + image.ImageUrl;
                }
            }

            var productList = _mapper.Map<List<ProductResponse>>(products);
            return productList;
            //return products;
        }

        public ProductResponse AddProduct(ProductRequest product)
        {
            if (product != null)
            {
                //تولید شناسه یکتا برای محصول
                Guid productId = Guid.NewGuid();

                // بارگیری و ذخیره تصاویر بیشتر در جدول Images 
                var additionalImages = new List<ImageEntity>();
                foreach (var image in product.Images.Images)
                {
                    //Save Image with base64
                    var base64 = image.ImageUrl.Split(',')[1];
                    var bytes = System.Convert.FromBase64String(base64);
                    var randName = Guid.NewGuid().ToString();

                    additionalImages.Add(new ImageEntity
                    {
                        ProductId = productId,
                        Caption = image.Caption,
                        ImageUrl = randName+".png"
                    });

                    //Save Image on Server
                    var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
                        StaticUrls.ProductImageUrl + randName + ".png");
                    System.IO.File.WriteAllBytes(path, bytes);
                }

                var newProduct = _mapper.Map<ProductEntity>(product);
                newProduct.ProductId = productId;
                newProduct.Images = additionalImages;
                newProduct.CreateDate = DateTime.Now;
                newProduct.UpdateDate = DateTime.Now;
                newProduct.IsApproved = true;

                var addResult = _crudServices.AddProductToDB(newProduct);

                if (addResult == "Success")
                {
                    var productResult = _mapper.Map<ProductResponse>(newProduct);
                    return productResult;
                }
                else if (addResult == "Fail")
                {
                    return GetErrorResponse("Add Product is failure");
                }
            }
            return GetErrorResponse("Input Product is Null"); 
        }


        public ProductResponse UpdateProduct(Guid productId, ProductResponse product)
        {
            var existProduct = _crudServices.FindProductByIdInDB(productId);
            if (existProduct != null)
            {
                if (product.ProductId==productId)
                {
                    var resultUpdate = _crudServices.UpdateProductInDB(existProduct, product);

                    if (resultUpdate == "UpdateSuccess")
                        return product;
                    else if (resultUpdate == "UpdateFailed")
                        return GetErrorResponse("Error: Update Product is failure");
                }
                else
                {
                    return GetErrorResponse("Error: ProductId is not like to ProuctId fieldSearch");
                }
            }
 
            return GetErrorResponse("Error: Unexpected condition");
        }

        
        public ProductResponse DeleteProduct(Guid productId)
        {
            var foundProduct = _crudServices.FindProductByIdInDB(productId);
            if (foundProduct != null)
            {
                var product = _mapper.Map<ProductEntity>(foundProduct);
                var result = _crudServices.DeleteProductOfDB(product);
                if(result == "DeleteSuccess")
                {
                    return GetErrorResponse($"Delete Product with ID {productId} Success.");
                }
                else
                {
                    return GetErrorResponse($"Delete Product with ID {productId} Failed.");
                }
            }
            return GetErrorResponse($"The ProductId with ID {productId} is not found!");
        }
        
        private ProductResponse GetErrorResponse(string errorMessage)
        {
            return new ProductResponse
            {
                ProductId = Guid.Empty,
                Name = errorMessage,
                Description = string.Empty,
                CreateDate = DateTime.MinValue,
                UpdateDate = DateTime.MinValue,
                IsApproved = false,
                Images = new ProductEmbeded(),
            };
        }
        
        public IQueryable<ProductEntity> CreateQuery()
        {
            var query = _crudServices.CreateQueryFromDB();
            return query;
        }

        public List<ProductResponse> SearchProduct(string fieldName, string fieldValue)
        {
            var query = CreateQuery();
            var filterService = new EntityFilterService<ProductEntity>(query);
            var parameter = Expression.Parameter(typeof(ProductEntity), fieldName);
            var property = Expression.Property(parameter, fieldName);

            object convertedValue;
            if (fieldName.ToLower() == "productid")
            {
                convertedValue = Guid.Parse(fieldValue);
            }
            else
            {
                convertedValue = Convert.ChangeType(fieldValue, property.Type);
            }

            var constant = Expression.Constant(convertedValue);
            var equals = Expression.Equal(property, constant);
            var lambada = Expression.Lambda<Func<ProductEntity, bool>>(equals, parameter);

            var productResult = _crudServices.SearchProductInDB(filterService, lambada);

            var productMap = _mapper.Map<List<ProductResponse>>(productResult);
            return productMap;
        }
    }
}
