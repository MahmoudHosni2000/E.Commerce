using AutoMapper;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Shared.Dtos.Products;

namespace E_Commerce.Service.MappingProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());
        }
    }
}
