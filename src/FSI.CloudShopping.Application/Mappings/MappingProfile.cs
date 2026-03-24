namespace FSI.CloudShopping.Application.Mappings;

using AutoMapper;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Enums;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer Mappings
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses))
            .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Contacts));

        CreateMap<Individual, IndividualDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName.FullName));

        CreateMap<Company, CompanyDto>();

        CreateMap<Address, AddressDto>()
            .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType.ToString()))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCode.Value));

        CreateMap<Contact, ContactDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone != null ? src.Phone.Value : null));

        // Product Mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.CompareAtPrice, opt => opt.MapFrom(src => src.CompareAtPrice != null ? src.CompareAtPrice.Amount : null))
            .ForMember(dest => dest.CostPrice, opt => opt.MapFrom(src => src.CostPrice.Amount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku.Value))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug.Value))
            .ForMember(dest => dest.AvailableStock, opt => opt.MapFrom(src => src.GetAvailableStock()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        CreateMap<Product, ProductSummaryDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.CompareAtPrice, opt => opt.MapFrom(src => src.CompareAtPrice != null ? src.CompareAtPrice.Amount : null))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug.Value))
            .ForMember(dest => dest.AvailableStock, opt => opt.MapFrom(src => src.GetAvailableStock()));

        CreateMap<ProductImage, ProductImageDto>();

        // Category Mappings
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug.Value))
            .ForMember(dest => dest.ChildCategories, opt => opt.MapFrom(src => src.ChildCategories));

        // Cart Mappings
        CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal().Amount))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.GetItemCount()))
            .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired));

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Amount))
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Subtotal.Amount));

        // Order Mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal.Amount))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount.Amount))
            .ForMember(dest => dest.ShippingCost, opt => opt.MapFrom(src => src.ShippingCost.Amount))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Amount))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<Order, OrderSummaryDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Amount))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.Items.Count));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Amount))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount.Amount))
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Subtotal.Amount));

        // Payment Mappings
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Amount));

        // Coupon Mappings
        CreateMap<Coupon, CouponDto>()
            .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => src.DiscountType.ToString()))
            .ForMember(dest => dest.MinOrderValue, opt => opt.MapFrom(src => src.MinOrderValue != null ? src.MinOrderValue.Amount : null));

        // Audit Log Mappings
        CreateMap<AuditLog, AuditLogDto>()
            .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action.ToString()));
    }
}
