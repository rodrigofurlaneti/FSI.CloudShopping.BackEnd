namespace FSI.CloudShopping.Application.Commands.Product;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record UpdateStockCommand(int ProductId, int Quantity, string Reason, bool IsAddition) : IRequest<Result<ProductDto>>;

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateStockCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            return new Result<ProductDto>.Failure("Product not found");
        }

        try
        {
            if (request.IsAddition)
            {
                product.AddStock(request.Quantity);
            }
            else
            {
                product.RemoveStock(request.Quantity);
            }

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
