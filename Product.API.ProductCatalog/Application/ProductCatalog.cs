﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.ProductCatalog.DTO.InternalAPI.Embeded;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;
using Product.API.ProductCatalog.DTO.InternalAPI.Response;
using Product.API.ProductCatalog.Extensions.ExtraClasses;
using Product.API.ProductCatalog.Extensions.SearchClasses;
using Product.API.ProductCatalog.Infrastructure.Entities;
using Product.API.ProductCatalog.Infrastructure.Repository;
using Product.API.ProductCatalog.wwwroot.StaticFiles;
using System.Collections.Generic;
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

        //public ApiResponse<List<ProductEntity>> GetAllProduct()
        public ApiResponse<List<ProductResponse>> GetAllProduct()
        {
            try
            {
                var getProducts = _crudServices.GetAllProductOfDB();
                
                foreach (var product in getProducts)
                {
                    foreach (var image in product.Images)
                    {
                        image.ImageUrl = StaticUrls.ProductImageUrl + image.ImageUrl;
                    }
                }
                var products = _mapper.Map<List<ProductResponse>>(getProducts);

                return new ApiResponse<List<ProductResponse>>
                {
                    Result = true,
                    Data = products
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductResponse>>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public ApiResponse<ProductResponse> AddProduct(ProductRequest product)
        {
            try
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
                            ImageUrl = randName + ".png"
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

                    if (addResult.Result)
                    {
                        // اگر .Data رو نذاری خالی برمیگردونه
                        var mapAddResult = _mapper.Map<ProductResponse>(addResult.Data);
                        return new ApiResponse<ProductResponse>
                        {
                            Result = addResult.Result,
                            Data = mapAddResult
                        };
                    }
                    else
                    {
                        return new ApiResponse<ProductResponse>
                        {
                            Result = false,
                            ErrorMessage = addResult.ErrorMessage
                        };
                    }
                }
                else
                {
                    return new ApiResponse<ProductResponse>
                    {
                        Result = false,
                        ErrorMessage = "The input product cannot be null"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductResponse>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                };
            }

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


        public ApiResponse<ProductResponse> DeleteProduct(ProductIdRequest delProduct)
        {
            try
            {
                var productId = delProduct.ProductId;
                var foundProduct = _crudServices.FindProductByIdInDB(productId);

                if(foundProduct != null)
                {
                    var product = _mapper.Map<ProductEntity>(foundProduct);
                    var result = _crudServices.DeleteProductOfDB(product);

                    if (result.Result)
                    {
                        var mapResult = _mapper.Map<ProductResponse>(result.Data);
                        return new ApiResponse<ProductResponse>
                        {
                            Result = result.Result,
                            Data = mapResult
                        };
                    }
                    else
                    {
                        return new ApiResponse<ProductResponse>
                        {
                            Result = result.Result,
                            ErrorMessage = result.ErrorMessage
                        };
                    }  
                }
                else
                {
                    return new ApiResponse<ProductResponse>
                    {
                        Result = false,
                        ErrorMessage = "The Product is not found!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductResponse>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                };
            }
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
