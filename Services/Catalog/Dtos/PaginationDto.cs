using System.Collections.Generic;

namespace Catalog.Dtos
{
    public class PaginationDto<T> where T : class
    {
        public PaginationDto()
        {
        }

        public PaginationDto(int pageIndex, int pageSize, int count, IReadOnlyCollection<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyCollection<T> Data { get; set; }
    }
}