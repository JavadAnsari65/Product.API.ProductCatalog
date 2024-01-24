using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.API.ProductCatalog.DTO.InternalAPI.Embeded;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;
using Product.API.ProductCatalog.DTO.InternalAPI.Response;
using Product.API.ProductCatalog.Extensions.SearchClasses;
using Product.API.ProductCatalog.Infrastructure.Configuration;
using Product.API.ProductCatalog.Infrastructure.Entities;
using Product.API.ProductCatalog.wwwroot.StaticFiles;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
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

        public string AddProductToDB(ProductEntity product)
        {
            try
            {
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return ("Success");
            }
            catch (Exception)
            {

                return ("Fail");
            }
        }

        public ProductEntity FindProductByIdInDB(Guid id)
        {
            var existProduct = _dbContext.Products
                .Include(p => p.Images)
                .FirstOrDefault(p => p.ProductId == id);
            return existProduct;
        }

        private void UpdateProductImagesInDB(ProductEntity product, ProductEmbeded images)
        {
            // حذف تصاویر فعلی محصول
            product.Images.Clear();

            // افزودن تصاویر جدید به محصول
            if (images != null && images.Images != null)
            {
                foreach (var imageRequest in images.Images)
                {
                    var base64 = imageRequest.ImageUrl.Split(',')[1];
                    var bytes = System.Convert.FromBase64String(base64);
                    var randName = Guid.NewGuid().ToString();

                    var imageEntity = new ImageEntity
                    {
                        Caption = imageRequest.Caption,
                        ImageUrl = randName + ".png",
                    };

                    product.Images.Add(imageEntity);

                    //Save Image on Server
                    var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                        StaticUrls.ProductImageUrl + randName + ".png");
                    System.IO.File.WriteAllBytes(path, bytes);
                }
            }
        }

        public string UpdateProductInDB(ProductEntity existProduct, ProductResponse updatedProduct)
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

                // به روزرسانی تصاویر محصول
                UpdateProductImagesInDB(existProduct, updatedProduct.Images);

                _dbContext.SaveChanges();

                return "UpdateSuccess";
            }
            catch (Exception)
            {
                return "UpdateFailed";
            }

        }
        
        public string DeleteProductOfDB(ProductEntity product)
        {
            try
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                return ("DeleteSuccess");
            }
            catch (Exception)
            {
                return ("DeleteFailed");
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
