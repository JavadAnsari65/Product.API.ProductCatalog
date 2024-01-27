using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Product.API.ProductCatalog.DTO.InternalAPI.Embeded;
using Product.API.ProductCatalog.DTO.InternalAPI.Request;
using Product.API.ProductCatalog.DTO.InternalAPI.Response;
using Product.API.ProductCatalog.Extensions.ExtraClasses;
using Product.API.ProductCatalog.Infrastructure.Entities;

namespace Product.API.ProductCatalog.Infrastructure.Configuration
{
    public class CustomMap:Profile
    {
        public CustomMap()
        {
            //CreateMap<ProductEntity, ProductResponse>()
            //    .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            //    .ReverseMap();

            //CreateMap<ProductEntity, ProductRequest>()
            //    .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            //    .ReverseMap();

            //CreateMap<ProductRequest, ProductResponse>()
            //    .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            //    .ReverseMap();

            //CreateMap<ImageEntity, ImageRequest>().ReverseMap();
            //CreateMap<ImageEntity, ImageResponse>().ReverseMap();

            //CreateMap<ProductEmbeded, List<ImageEntity>>().ReverseMap();

            //#########################################################################################################

            //CreateMap(typeof(ApiResponse<>), typeof(ProductResponse)).ReverseMap();
            //CreateMap(typeof(ApiResponse<>), typeof(ProductEntity)).ReverseMap();


            //CreateMap<ProductRequest, ProductResponse>()
            //    .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            //    .ReverseMap();

            //CreateMap<ImageEntity, DTO.ExternalAPI.Request.ImageRequest>().ReverseMap();
            //CreateMap<ImageEntity, DTO.ExternalAPI.Response.ImageResponse>().ReverseMap();

            
            CreateMap<ProductEntity, DTO.ExternalAPI.Response.ProductResponse>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();

            CreateMap<ProductEntity, DTO.ExternalAPI.Request.ProductRequest>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();

            CreateMap<DTO.ExternalAPI.Embeded.ProductEmbeded, List<ImageEntity>>().ReverseMap();  //Mohem

            CreateMap<ProductEntity, ProductResponse>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();
            CreateMap<List<ImageEntity>, ProductEmbeded>().ReverseMap();

            CreateMap<ProductResponse, DTO.ExternalAPI.Response.ProductResponse>().ReverseMap();

            CreateMap<ProductEmbeded, DTO.ExternalAPI.Embeded.ProductEmbeded>().ReverseMap();


            //########################################################

            CreateMap<ProductRequest, DTO.ExternalAPI.Request.ProductRequest>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();

            CreateMap<ImageRequest, DTO.ExternalAPI.Request.ImageRequest>().ReverseMap();

            CreateMap<ProductEntity, ProductRequest>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();

            //#######################################################

            CreateMap(typeof(ApiResponse<>), typeof(ProductResponse)).ReverseMap();  //OK

            //##############################################################################

            CreateMap(typeof(ApiResponse<>), typeof(ProductEntity)).ReverseMap();

            CreateMap<ProductIdRequest, DTO.ExternalAPI.Request.ProductIdRequest>().ReverseMap();

        }

    }
}
