using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Service.Abstraction;
using E_Commerce.Service.Specifications;
using E_Commerce.Shared;
using E_Commerce.Shared.Dtos.Products;

namespace E_Commerce.Service.Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public Task<ProductDto> GetByIdAsync(int id)
        {
            return GetByIdInternalAsync(id);
        }

        public Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return GetProductsInternalAsync();
        }

        public async Task<IEnumerable<TypeDto>> GetTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return mapper.Map<IEnumerable<TypeDto>>(types);
        }

        public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        private async Task<ProductDto> GetByIdInternalAsync(int id)
        {
            var products = await unitOfWork.GetRepository<Product, int>()
                .GetAllAsync(new ProductWithBrandTypeSpecification(id));

            var match = products.FirstOrDefault(p => p.Id == id);

            return match is null ? new ProductDto() : mapper.Map<ProductDto>(match);
        }

        private async Task<IEnumerable<ProductDto>> GetProductsInternalAsync()
        {
            var products = await unitOfWork.GetRepository<Product, int>()
                .GetAllAsync(p => p.ProductBrand, p => p.ProductType);

            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<PaginatedResult<ProductDto>> GetProductsAsync(ProductQueryParameters parameters)
        {
            var specification = new ProductWithBrandTypeSpecification(parameters);
            var repository = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specification);
            var countSpecs = new ProductWithBrandTypeSpecification(parameters);
            var totalCount = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpecs);
            var result = mapper.Map<IEnumerable<ProductDto>>(repository);

            return new PaginatedResult<ProductDto>(parameters.PageIndex, parameters.PageSize, totalCount, result);
        }
    }
}
