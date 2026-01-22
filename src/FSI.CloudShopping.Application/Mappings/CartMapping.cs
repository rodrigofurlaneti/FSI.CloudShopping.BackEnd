using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using System.Linq;

namespace FSI.CloudShopping.Application.Mappings
{
    public static class CartMapping
    {
        public static CartDTO ToDto(this Cart entity)
        {
            if (entity == null) return new CartDTO();

            return new CartDTO
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                Items = entity.Items?.Select(i => i.ToDto()).ToList() ?? new System.Collections.Generic.List<CartItemDTO>(),
                TotalAmount = entity.Items?.Sum(x => x.TotalPrice.Value) ?? 0
            };
        }

        public static CartItemDTO ToDto(this CartItem entity)
        {
            if (entity == null) return null!;

            return new CartItemDTO(
                entity.ProductId,
                "Produto",
                entity.Quantity.Value,
                entity.UnitPrice.Value,
                entity.TotalPrice.Value);
        }
    }
}