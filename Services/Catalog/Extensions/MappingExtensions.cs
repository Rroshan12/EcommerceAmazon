using Catalog.Entities;
using Catalog.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Extensions
{
    public static class MappingExtensions
    {
        public static ProductBrandDto ToDto(this ProductBrand brand)
        {
            if (brand == null) return null;
            return new ProductBrandDto
            {
                Id = brand.Id,
                Name = brand.Name
            };
        }

        public static ProductBrand ToEntity(this ProductBrandDto dto)
        {
            if (dto == null) return null;
            return new ProductBrand { Id = dto.Id, Name = dto.Name };
        }

        public static ProductTypeDto ToDto(this ProductType type)
        {
            if (type == null) return null;
            return new ProductTypeDto
            {
                Id = type.Id,
                Name = type.Name
            };
        }

        public static ProductType ToEntity(this ProductTypeDto dto)
        {
            if (dto == null) return null;
            return new ProductType { Id = dto.Id, Name = dto.Name };
        }

        public static ProductDto ToDto(this Product product)
        {
            if (product == null) return null;
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Summary = product.Summary,
                ImageFile = product.ImageFile,
                BrandId = product.BrandId,
                Brand = product.Brand?.ToDto(),
                TypeId = product.TypeId,
                Type = product.Type?.ToDto(),
                Price = product.Price,
                CreatedDate = product.CreatedDate
            };
        }

        public static IEnumerable<ProductDto> ToDto(this IEnumerable<Product> products)
        {
            return products?.Select(p => p.ToDto());
        }

        public static IEnumerable<ProductBrandDto> ToDto(this IEnumerable<ProductBrand> brands)
        {
            return brands?.Select(b => b.ToDto());
        }

        public static IEnumerable<ProductTypeDto> ToDto(this IEnumerable<ProductType> types)
        {
            return types?.Select(t => t.ToDto());
        }

        public static Product ToEntity(this ProductDto dto)
        {
            if (dto == null) return null;
            return new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Summary = dto.Summary,
                ImageFile = dto.ImageFile,
                BrandId = dto.BrandId,
                // Brand and Type navigation props are not set here to avoid extra DB lookups
                TypeId = dto.TypeId,
                Price = dto.Price,
                CreatedDate = dto.CreatedDate
            };
        }

        public static Product ToEntity(this CreateProductRequest dto)
        {
            if (dto == null) return null;
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Summary = dto.Summary,
                ImageFile = dto.ImageFile,
                BrandId = dto.BrandId,
                TypeId = dto.TypeId,
                Price = dto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
        }

        public static Product ToEntity(this UpdateProductRequest dto)
        {
            if (dto == null) return null;
            return new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Summary = dto.Summary,
                ImageFile = dto.ImageFile,
                BrandId = dto.BrandId,
                TypeId = dto.TypeId,
                Price = dto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
        }

        public static ProductBrand ToEntity(this CreateBrandRequest dto)
        {
            if (dto == null) return null;
            return new ProductBrand { Name = dto.Name };
        }

        public static ProductBrand ToEntity(this UpdateBrandRequest dto)
        {
            if (dto == null) return null;
            return new ProductBrand { Id = dto.Id, Name = dto.Name };
        }

        public static ProductType ToEntity(this CreateTypeRequest dto)
        {
            if (dto == null) return null;
            return new ProductType { Name = dto.Name };
        }

        public static ProductType ToEntity(this UpdateTypeRequest dto)
        {
            if (dto == null) return null;
            return new ProductType { Id = dto.Id, Name = dto.Name };
        }

        public static PaginationDto<TDto> ToDto<TSource, TDto>(this Catalog.Specifications.Pagination<TSource> pagination, System.Func<TSource, TDto> map)
            where TSource : class
            where TDto : class
        {
            if (pagination == null) return null;
            return new PaginationDto<TDto>(pagination.PageIndex, pagination.PageSize, pagination.Count, pagination.Data.Select(map).ToArray());
        }
    }
}