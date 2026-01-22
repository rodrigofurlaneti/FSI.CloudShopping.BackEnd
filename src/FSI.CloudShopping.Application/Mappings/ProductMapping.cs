using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Application.Mappings
{
    public static class ProductMapping
    {
        public static Product MapToEntity(ProductDTO dto)
        {
            if (dto == null) return null!;

            return new Product(
                new SKU(dto.Sku),
                dto.Name,
                new Money(dto.Price),
                new Quantity(dto.Stock)
            );
        }

        public static ProductDTO MapToDto(Product entity)
        {
            if (entity == null) return null!;

            return new ProductDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Sku = entity.Sku.Code,
                Price = entity.Price.Value,
                Stock = entity.Stock.Value
            };
        }
        public static ProductDTO ToDto(this Product entity) => MapToDto(entity);
        public static Product ToEntity(this ProductDTO dto) => MapToEntity(dto);
    }
}