namespace Catalog.Dtos
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImageFile { get; set; }
        public string BrandId { get; set; }
        public string TypeId { get; set; }
        public decimal Price { get; set; }
    }
}