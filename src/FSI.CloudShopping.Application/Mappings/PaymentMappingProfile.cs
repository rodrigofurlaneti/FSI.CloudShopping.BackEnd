using AutoMapper;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Mappings
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Payment, PaymentDTO>()
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount.Value))
                .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.Method.Description))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            CreateMap<PaymentDTO, Payment>()
                .ConstructUsing(d => new Payment(
                    d.OrderId,
                    PaymentMethod.FromString(d.PaymentMethod),
                    new Money(d.Amount)));
        }
    }
}