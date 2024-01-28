using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.API.ProductCatalog.DTO.InternalAPI.Embeded;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;
using Product.API.ProductCatalog.DTO.InternalAPI.Response;
using Product.API.ProductCatalog.Extensions.ExtraClasses;
using Product.API.ProductCatalog.Extensions.SearchClasses;
using Product.API.ProductCatalog.Infrastructure.Configuration;
using Product.API.ProductCatalog.Infrastructure.Entities;
using Product.API.ProductCatalog.wwwroot.StaticFiles;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace Product.API.ProductCatalog.Infrastructure.Repository
{
    public class CRUDService
    {
        private readonly ProductDbContext _dbContext;
        public CRUDService(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ProductEntity> GetAllProductOfDB()
        {
            var products = _dbContext.Products
                .Include(p=>p.Images)
                .ToList();
            return products;
        }

        public ApiResponse<ProductEntity> AddProductToDB(ProductEntity product)
        {
            try
            {
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return new ApiResponse<ProductEntity>
                {
                    Result = true,
                    Data = product
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductEntity>
                {
                    Result = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
        
        public ApiResponse<ProductEntity> FindProductByIdInDB(Guid id)
        {
            try
            {
                var existProduct = _dbContext.Products
                .Include(p => p.Images)
                .FirstOrDefault(p => p.ProductId == id);
                return new ApiResponse<ProductEntity>
                {
                    Result = true,
                    Data = existProduct
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductEntity>
                {
                    Result = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        private ApiResponse<ProductEntity> UpdateProductImagesInDB(ProductEntity product, List<ImageEntity> images)
        {

            try
            {
                // حذف تصاویر فعلی محصول
                product.Images.Clear();

                // افزودن تصاویر جدید به محصول
                if (images.Count>0)
                {
                    foreach (var image in images)
                    {
                        var base64 = image.ImageUrl.Split(',')[1];
                        var bytes = System.Convert.FromBase64String(base64);
                        var randName = Guid.NewGuid().ToString();

                        var imageEntity = new ImageEntity
                        {
                            Caption = image.Caption,
                            ImageUrl = randName + ".png",

                            //ProductId = product.ProductId,
                        };

                        product.Images.Add(imageEntity);

                        //Save Image on Server
                        var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                            StaticUrls.ProductImageUrl + randName + ".png");
                        System.IO.File.WriteAllBytes(path, bytes);
                    }

                    return new ApiResponse<ProductEntity>
                    {
                        Result = true,
                        Data = product
                    };
                }
                else
                {
                    return new ApiResponse<ProductEntity>
                    {
                        Result = false,
                        ErrorMessage = "At least one image must be included for the product"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductEntity>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                };
            }



            //// حذف تصاویر فعلی محصول
            //product.Images.Clear();

            //// افزودن تصاویر جدید به محصول
            //if (images != null && images.Images != null)
            //{
            //    foreach (var imageRequest in images.Images)
            //    {
            //        var base64 = imageRequest.ImageUrl.Split(',')[1];
            //        var bytes = System.Convert.FromBase64String(base64);
            //        var randName = Guid.NewGuid().ToString();

            //        var imageEntity = new ImageEntity
            //        {
            //            Caption = imageRequest.Caption,
            //            ImageUrl = randName + ".png",
            //        };

            //        product.Images.Add(imageEntity);

            //        //Save Image on Server
            //        var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
            //            StaticUrls.ProductImageUrl + randName + ".png");
            //        System.IO.File.WriteAllBytes(path, bytes);
            //    }
            //}
        }

        public ApiResponse<ProductEntity> UpdateProductInDB(ProductEntity existProduct, ProductEntity updatedProduct)
        {
            try
            {
                // به روزرسانی ویژگی‌های محصول با اطلاعات ارسالی از درخواست
                existProduct.ProductCatId = updatedProduct.ProductCatId;
                existProduct.Name = updatedProduct.Name;
                existProduct.Description = updatedProduct.Description;
                existProduct.Price = updatedProduct.Price;
                existProduct.CreateDate = updatedProduct.CreateDate;
                existProduct.UpdateDate = DateTime.Now;
                existProduct.IsApproved = updatedProduct.IsApproved;


                //##########################################################################

                existProduct.Images.Clear();

                if (updatedProduct.Images.Count > 0)
                {
                    foreach (var image in updatedProduct.Images)
                    {
                        var base64 = image.ImageUrl.Split(',')[1];
                        var bytes = System.Convert.FromBase64String(base64);
                        var randName = Guid.NewGuid().ToString();

                        var imageEntity = new ImageEntity
                        {
                            Caption = image.Caption,
                            ImageUrl = randName + ".png",

                            ProductId = existProduct.ProductId,
                        };

                        existProduct.Images.Add(imageEntity);

                        //Save Image on Server
                        var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                            StaticUrls.ProductImageUrl + randName + ".png");
                        System.IO.File.WriteAllBytes(path, bytes);
                    }

                    _dbContext.SaveChanges();

                    return new ApiResponse<ProductEntity>
                    {
                        Result = true,
                        Data = existProduct
                    };
                }
                else
                {
                    return new ApiResponse<ProductEntity>
                    {
                        Result = false,
                        ErrorMessage = "At least one image must be included for the product"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductEntity>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                };
            }




            /*
            try
            {
                // به روزرسانی ویژگی‌های محصول با اطلاعات ارسالی از درخواست
                existProduct.ProductCatId = updatedProduct.ProductCatId;
                existProduct.Name = updatedProduct.Name;
                existProduct.Description = updatedProduct.Description;
                existProduct.Price = updatedProduct.Price;
                existProduct.CreateDate = updatedProduct.CreateDate;
                existProduct.UpdateDate = DateTime.Now;
                existProduct.IsApproved = updatedProduct.IsApproved;

                // به روزرسانی تصاویر محصول
                var updateResult = UpdateProductImagesInDB(existProduct, updatedProduct.Images);

                if (updateResult.Result)
                {
                    _dbContext.SaveChanges();

                    return new ApiResponse<ProductEntity>
                    {
                        Result = true,
                        Data = updateResult.Data
                    };
                }
                else
                {
                    return new ApiResponse<ProductEntity>
                    {
                        Result = false,
                        ErrorMessage = updateResult.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductEntity>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                };
            }
            */
        }
        
        public ApiResponse<ProductEntity> DeleteProductOfDB(ProductEntity product)
        {
            try
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();

                return new ApiResponse<ProductEntity>
                {
                    Result = true,
                    Data = product
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductEntity>
                {
                    Result = false,
                    ErrorMessage = ex.Message
                };
            }
        }
        
        public IQueryable<ProductEntity> CreateQueryFromDB()
        {
            var query = _dbContext.Products
                .Include(p=>p.Images)
                .AsQueryable();

            return query;
        }

        public List<ProductEntity> SearchProductInDB(EntityFilterService<ProductEntity> filterService, Expression<Func<ProductEntity, bool>> lambada)
        {
            var query = filterService.ApplyFilter(lambada);
            var productResult = query.ToList();
            return productResult;
        }
    }
}
