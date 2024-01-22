using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Product.API.ProductCatalog.DTO.InternalAPI.Embeded;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;
using Product.API.ProductCatalog.DTO.InternalAPI.Response;
using Product.API.ProductCatalog.Infrastructure.Entities;

namespace Product.API.ProductCatalog.Infrastructure.Configuration
{
    public class CustomMap:Profile
    {
        public CustomMap()
        {
            CreateMap<ProductEntity, ProductResponse>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();

            CreateMap<ProductEntity, ProductRequest>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();

            CreateMap<ProductRequest, ProductResponse>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();

            CreateMap<ImageEntity, ImageRequest>().ReverseMap();
            CreateMap<ImageEntity, ImageResponse>().ReverseMap();

            CreateMap<ProductEmbeded, List<ImageEntity>>().ReverseMap();

        }
    }
}
