using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Product;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.Sku, o => o.MapFrom(s => s.Sku.Code))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price.Value))
                .ForMember(d => d.StockQuantity, o => o.MapFrom(s => s.Stock.Value));

            CreateMap<ProductDTO, Product>()
                .ConstructUsing(d => new Product(
                    new SKU(d.Sku),
                    d.Name,
                    new Money(d.Price),
                    new Quantity(d.StockQuantity)));
        }
    }
}