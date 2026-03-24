namespace FSI.CloudShopping.Application.Queries.Coupon;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record GetCouponsPagedQuery(int Page, int PageSize) : IRequest<Result<PagedResult<CouponDto>>>;

public class GetCouponsPagedQueryHandler : IRequestHandler<GetCouponsPagedQuery, Result<PagedResult<CouponDto>>>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;

    public GetCouponsPagedQueryHandler(ICouponRepository couponRepository, IMapper mapper)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<CouponDto>>> Handle(
        GetCouponsPagedQuery request, CancellationToken cancellationToken)
    {
        var (coupons, total) = await _couponRepository.GetPagedAsync(request.Page, request.PageSize, cancellationToken);
        var dtos = _mapper.Map<IEnumerable<CouponDto>>(coupons);
        var paged = new PagedResult<CouponDto>(dtos, total, request.Page, request.PageSize);
        return new Result<PagedResult<CouponDto>>.Success(paged);
    }
}
