using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Shared.Dtos.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string PictureUrl { get; init; }
        public string Price { get; init; }
        public string Type { get; init; }
        public string Brand { get; init; }
    }
}