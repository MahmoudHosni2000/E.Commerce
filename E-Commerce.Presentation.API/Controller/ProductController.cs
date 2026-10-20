using E_Commerce.Service.Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.Dtos.Products;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.API.Controller
{
    public class ProductController(IProductService productService) : APIBaseController
    {
        [HttpGet]
        public async Task<ActionResult<ProductDto>> GetByIdAsync(int id)
        {
            var product = await productService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetProductsAsync([FromQuery] ProductQueryParameters parameters)
        {
            var products = await productService.GetProductsAsync(parameters);
            return Ok(products);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetTypesAsync()
        {
            var types = await productService.GetTypesAsync();
            return Ok(types);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrandsAsync()
        {
            var brands = await productService.GetBrandsAsync();
            return Ok(brands);
        }
    }
}
