using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using System.Linq;

namespace FSI.CloudShopping.Application.Mappings
{
    public static class OrderMapping
    {
        public static Order ToEntity(this OrderDTO dto)
        {
            if (dto == null) return null!;
            var cartItems = dto.Items.Select(i =>
                new CartItem(i.ProductId, new Quantity(i.Quantity), new Money(i.UnitPrice)));

            return new Order(dto.CustomerId, cartItems);
        }

        public static OrderDTO ToDto(this Order entity)
        {
            if (entity == null) return null!;

            return new OrderDTO
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                Status = entity.Status?.Code ?? string.Empty,
                TotalAmount = entity.TotalAmount?.Value ?? 0,
                Items = entity.Items?.Select(i => i.ToDto()).ToList() ?? new List<OrderItemDTO>()
            };
        }

        public static OrderItemDTO ToDto(this OrderItem entity) =>
            new OrderItemDTO
            {
                ProductId = entity.ProductId,
                Quantity = entity.Quantity.Value,
                UnitPrice = entity.UnitPrice.Value
            };

        public static OrderItem ToOrderItemEntity(this OrderItemDTO dto) =>
            new OrderItem(dto.ProductId, new Quantity(dto.Quantity), new Money(dto.UnitPrice));
    }
}