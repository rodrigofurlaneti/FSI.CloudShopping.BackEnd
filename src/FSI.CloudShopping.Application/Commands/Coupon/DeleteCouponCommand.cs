namespace FSI.CloudShopping.Application.Commands.Coupon;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Domain.Interfaces;

public record DeleteCouponCommand(Guid Id) : IRequest<Result<bool>>;

public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, Result<bool>>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCouponCommandHandler(ICouponRepository couponRepository, IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByIdAsync(request.Id, cancellationToken);
        if (coupon == null)
            return new Result<bool>.Failure($"Cupom {request.Id} não encontrado.");

        await _couponRepository.DeleteAsync(coupon, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new Result<bool>.Success(true);
    }
}
