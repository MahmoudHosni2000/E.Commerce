using E_Commerce.Shared;
using E_Commerce.Shared.Dtos.Products;

namespace E_Commerce.Service.Abstraction
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(int id);

        Task<PaginatedResult<ProductDto>> GetProductsAsync(ProductQueryParameters parameters);

        Task<IEnumerable<TypeDto>> GetTypesAsync();

        Task<IEnumerable<BrandDto>> GetBrandsAsync();
    }
}