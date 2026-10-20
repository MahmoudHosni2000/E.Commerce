using E_Commerce.Domain.Entities.Products;
using E_Commerce.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Service.Specifications
{
    public class ProductCountSpecification : BaseSpecification<Product>
    {
        public ProductCountSpecification(ProductQueryParameters parameters)
                : base(CreateCriteria(parameters)) { }

        public static Expression<Func<Product, bool>> CreateCriteria(ProductQueryParameters parameters)
        {
            return p =>
                (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId.Value) &&
                (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId.Value) &&
                (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower()));
        }
    }
}

