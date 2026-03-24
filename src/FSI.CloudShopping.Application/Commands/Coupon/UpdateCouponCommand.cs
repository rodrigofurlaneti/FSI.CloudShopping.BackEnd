namespace FSI.CloudShopping.Application.Commands.Coupon;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public record UpdateCouponCommand(
    Guid Id,
    string? Description,
    decimal? DiscountValue,
    decimal? MinOrderValue,
    int? MaxUsages,
    DateTime? ValidFrom,
    DateTime? ValidTo,
    bool? IsActive
) : IRequest<Result<CouponDto>>;

public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, Result<CouponDto>>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCouponCommandHandler(ICouponRepository couponRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CouponDto>> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByIdAsync(request.Id, cancellationToken);
        if (coupon == null)
            return new Result<CouponDto>.Failure($"Cupom {request.Id} não encontrado.");

        // Update core fields (only the ones that are changing)
        var minOrderValue = request.MinOrderValue.HasValue
            ? new Money(request.MinOrderValue.Value)
            : coupon.MinOrderValue;

        coupon.Update(
            request.Description ?? coupon.Description,
            request.DiscountValue ?? coupon.DiscountValue,
            minOrderValue,
            request.MaxUsages ?? coupon.MaxUsages,
            request.ValidFrom ?? coupon.ValidFrom,
            request.ValidTo ?? coupon.ValidTo
        );

        // Handle activation separately
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                coupon.Activate();
            else
                coupon.Deactivate();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new Result<CouponDto>.Success(_mapper.Map<CouponDto>(coupon));
    }
}
