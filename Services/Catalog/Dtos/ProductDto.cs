using System;

namespace Catalog.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImageFile { get; set; }
        public string BrandId { get; set; }
        public ProductBrandDto Brand { get; set; }
        public string TypeId { get; set; }
        public ProductTypeDto Type { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}