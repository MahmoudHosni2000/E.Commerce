using E_Commerce.Domain.Entities.Products;
using E_Commerce.Shared.Dtos.Products;
using System.Linq.Expressions;

namespace E_Commerce.Service.Specifications
{
    public class ProductWithBrandTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandTypeSpecification(ProductQueryParameters parameters) 
            : base(ProductCountSpecification.CreateCriteria(parameters))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            ApplyPaging(parameters.PageSize, parameters.PageIndex);

            switch (parameters.Sort)
            {
                case ProductSort.NameAsc:
                    OrderBy = p => p.Name;
                    break;
                case ProductSort.NameDesc:
                    OrderByDescending = p => p.Name;
                    break;
                case ProductSort.PriceAsc:
                    OrderBy = p => p.Price;
                    break;
                case ProductSort.PriceDesc:
                    OrderByDescending = p => p.Price;
                    break;
                default:
                    OrderBy = p => p.Name;
                    break;
            }
        }
        public ProductWithBrandTypeSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
