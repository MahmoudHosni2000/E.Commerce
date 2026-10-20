using AutoMapper;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Shared.Dtos.Products;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Service.MappingProfile
{
    public class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.PictureUrl))
                return string.Empty;

            var baseUrl = configuration["baseUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
                return source.PictureUrl;

            return $"{baseUrl.TrimEnd('/')}/{source.PictureUrl.TrimStart('/')}";
        }
    }
}
