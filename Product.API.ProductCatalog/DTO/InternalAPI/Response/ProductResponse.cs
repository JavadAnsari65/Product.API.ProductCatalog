﻿using Product.API.ProductCatalog.DTO.InternalAPI.Embeded;

namespace Product.API.ProductCatalog.DTO.InternalAPI.Response
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public int ProductCatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsApproved { get; set; }
        public List<ProductEmbeded> Images { get; set; }
    }
}
