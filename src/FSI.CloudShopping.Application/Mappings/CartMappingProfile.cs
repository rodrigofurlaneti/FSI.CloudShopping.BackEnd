using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Cart;
using FSI.CloudShopping.Domain.Entities;

namespace FSI.CloudShopping.Application.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal().Value));
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity.Value))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Value))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.TotalPrice.Value))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice.Value));
        }
    }
}