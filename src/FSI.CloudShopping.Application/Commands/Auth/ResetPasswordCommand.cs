namespace FSI.CloudShopping.Application.Commands.Auth;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<Result<bool>>;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<bool>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByRefreshTokenAsync(request.Token, cancellationToken);
        if (customer == null)
            return new Result<bool>.Failure("Token de redefinição inválido ou expirado.");

        var newPassword = new Password(request.NewPassword);
        customer.ResetPassword(newPassword);
        customer.RevokeRefreshToken();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new Result<bool>.Success(true);
    }
}
