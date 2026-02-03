namespace Catalog.Specifications
{
    public class CatalogSpecParams
    {
        private const int _maxPageSize = 50;
        private  int _pageSize = 10;

        public int PageNumber { get; set; }
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = (value > _maxPageSize) ? _maxPageSize: value;
        
        }
        public int MaxPageSize { get; set; }

        public string? BrandId  { get; set; }
        public string? TypeId { get; set; }
        public string? Description { get; set; }

        public string? Search { get; set;}
    }
}
