namespace FSI.CloudShopping.Application.Queries.Coupon;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record GetCouponByIdQuery(Guid Id) : IRequest<Result<CouponDto>>;

public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQuery, Result<CouponDto>>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;

    public GetCouponByIdQueryHandler(ICouponRepository couponRepository, IMapper mapper)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
    }

    public async Task<Result<CouponDto>> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByIdAsync(request.Id, cancellationToken);
        if (coupon == null)
            return new Result<CouponDto>.Failure($"Cupom {request.Id} não encontrado.");

        return new Result<CouponDto>.Success(_mapper.Map<CouponDto>(coupon));
    }
}
