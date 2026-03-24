namespace FSI.CloudShopping.Application.Commands.Auth;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Interfaces;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<RefreshTokenResponse>>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
        ICustomerRepository customerRepository,
        IJwtService jwtService,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RefreshTokenResponse>> Handle(
        RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (customer == null || customer.RefreshTokenExpiry < DateTime.UtcNow)
            return new Result<RefreshTokenResponse>.Failure("Refresh token inválido ou expirado.");

        var (accessToken, expiry) = _jwtService.GenerateToken(customer);
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(30);

        customer.UpdateRefreshToken(newRefreshToken, refreshExpiry);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new Result<RefreshTokenResponse>.Success(
            new RefreshTokenResponse(accessToken, newRefreshToken, expiry));
    }
}
