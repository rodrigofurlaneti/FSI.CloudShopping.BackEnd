namespace FSI.CloudShopping.Application.Commands.Product;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

public record CreateProductCommand(
    string Sku,
    string Name,
    string Slug,
    string Description,
    string ShortDescription,
    decimal Price,
    decimal CostPrice,
    int StockQuantity,
    int MinStockAlert,
    Guid CategoryId,
    string? ImageUrl = null,
    double Weight = 0,
    decimal? CompareAtPrice = null
) : IRequest<Result<ProductDto>>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Validate SKU is unique
        var existingSku = await _productRepository.GetBySkuAsync(new SKU(request.Sku), cancellationToken);
        if (existingSku != null)
        {
            return new Result<ProductDto>.Failure("SKU already exists");
        }

        // Validate category exists
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            return new Result<ProductDto>.Failure("Category not found");
        }

        try
        {
            var sku = new SKU(request.Sku);
            var slug = new Slug(request.Slug);
            var price = new Money(request.Price);
            var costPrice = new Money(request.CostPrice);
            var compareAtPrice = request.CompareAtPrice.HasValue ? new Money(request.CompareAtPrice.Value) : null;

            var product = Product.Create(
                sku,
                request.Name,
                slug,
                request.Description,
                request.ShortDescription,
                price,
                costPrice,
                request.StockQuantity,
                request.MinStockAlert,
                request.CategoryId,
                request.ImageUrl,
                request.Weight,
                compareAtPrice
            );

            await _productRepository.AddAsync(product, cancellationToken);
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
