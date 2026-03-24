namespace FSI.CloudShopping.Application.Commands.Coupon;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public record CreateCouponCommand(
    string Code,
    string? Description,
    CouponDiscountType DiscountType,
    decimal DiscountValue,
    decimal? MinOrderValue,
    int MaxUsages,
    DateTime ValidFrom,
    DateTime ValidTo
) : IRequest<Result<CouponDto>>;

public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, Result<CouponDto>>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCouponCommandHandler(ICouponRepository couponRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CouponDto>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate code
        var existing = await _couponRepository.GetByCodeAsync(request.Code, cancellationToken);
        if (existing != null)
            return new Result<CouponDto>.Failure($"Cupom com código '{request.Code}' já existe.");

        var minOrderValue = request.MinOrderValue.HasValue
            ? new Money(request.MinOrderValue.Value)
            : (Money?)null;

        var coupon = Coupon.Create(
            request.Code.ToUpper().Trim(),
            request.Description,
            request.DiscountType,
            request.DiscountValue,
            minOrderValue,
            request.MaxUsages,
            request.ValidFrom,
            request.ValidTo
        );

        await _couponRepository.AddAsync(coupon, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new Result<CouponDto>.Success(_mapper.Map<CouponDto>(coupon));
    }
}
