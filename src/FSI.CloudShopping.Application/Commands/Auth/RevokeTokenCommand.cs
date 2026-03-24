namespace FSI.CloudShopping.Application.Commands.Auth;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Domain.Interfaces;

public record RevokeTokenCommand(Guid CustomerId) : IRequest<Result<bool>>;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Result<bool>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RevokeTokenCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            return new Result<bool>.Failure("Cliente não encontrado.");

        customer.RevokeRefreshToken();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new Result<bool>.Success(true);
    }
}
