﻿using Newtonsoft.Json;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;

namespace Product.API.ProductCatalog.DTO.InternalAPI.Embeded
{
    public class ProductEmbeded
    {
        //public List<ImageRequest> Images { get; set; }

        public string Caption { get; set; }
        public string ImageUrl { get; set; }
    }
}
