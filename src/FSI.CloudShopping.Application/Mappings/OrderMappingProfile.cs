using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Order;
using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Application.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Mapeamento do Cabeçalho do Pedido
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Value))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            // Mapeamento dos Itens do Pedido
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity.Value))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Value))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal().Value));
        }
    }
}