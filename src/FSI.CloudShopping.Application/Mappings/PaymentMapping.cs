using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Application.Mappings
{
    public static class PaymentMapping
    {
        public static Payment MapToEntity(PaymentDTO dto)
        {
            return new Payment(
                dto.OrderId,
                PaymentMethod.FromString(dto.Method),
                new Money(dto.Amount)
            );
        }
        public static PaymentDTO MapToDto(Payment entity)
        {
            if (entity == null) return null!;
            return new PaymentDTO
            {
                Id = entity.Id,
                OrderId = entity.OrderId,
                Method = entity.Method.Description,
                Amount = entity.Amount.Value,
                Status = entity.Status.Description
            };
        }
        public static PaymentDTO ToDto(this Payment entity) => MapToDto(entity);
        public static Payment ToEntity(this PaymentDTO dto) => MapToEntity(dto);
    }
}
