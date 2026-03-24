namespace FSI.CloudShopping.Application.Commands.Product;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public record UpdateProductCommand(
    int ProductId,
    string Name,
    string Slug,
    string Description,
    string ShortDescription,
    decimal Price,
    decimal CostPrice,
    int MinStockAlert,
    string? ImageUrl = null,
    decimal? CompareAtPrice = null
) : IRequest<Result<ProductDto>>;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            return new Result<ProductDto>.Failure("Product not found");
        }

        try
        {
            var slug = new Slug(request.Slug);
            var price = new Money(request.Price);
            var costPrice = new Money(request.CostPrice);
            var compareAtPrice = request.CompareAtPrice.HasValue ? new Money(request.CompareAtPrice.Value) : null;

            product.Update(
                request.Name,
                slug,
                request.Description,
                request.ShortDescription,
                price,
                costPrice,
                request.MinStockAlert,
                request.ImageUrl,
                compareAtPrice
            );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDto = _mapper.Map<ProductDto>(product);
            return new Result<ProductDto>.Success(productDto);
        }
        catch (Domain.Core.DomainException ex)
        {
            return new Result<ProductDto>.Failure(ex.Message);
        }
    }
}
