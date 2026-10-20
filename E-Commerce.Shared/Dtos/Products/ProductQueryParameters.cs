using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace E_Commerce.Shared.Dtos.Products
{
    public class ProductQueryParameters
    {
        private const int DefaultPageSize = 6;
        private const int MaxPageSize = 10;

        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        public string? Search { get; set; }
        public ProductSort? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int pageSize = DefaultPageSize;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))] // This attribute allows the enum to be serialized and deserialized as a string in JSON.
    public enum ProductSort
    {
        NameAsc = 1,
        NameDesc = 2,
        PriceAsc = 3,
        PriceDesc = 4
    }
}
