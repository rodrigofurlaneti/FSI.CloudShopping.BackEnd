namespace FSI.CloudShopping.Application.Commands.Auth;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
using BCrypt.Net;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(ICustomerRepository customerRepository, IJwtService jwtService, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var customer = await _customerRepository.GetByEmailAsync(email, cancellationToken);

        if (customer is null || customer.Password is null)
            return new Result<LoginResponse>.Failure("Invalid email or password");

        var isPasswordValid = BCrypt.Verify(request.Password, customer.Password.Hash);
        if (!isPasswordValid)
            return new Result<LoginResponse>.Failure("Invalid email or password");

        if (!customer.IsActive)
            return new Result<LoginResponse>.Failure("Customer account is inactive");

        var token = _jwtService.GenerateToken(customer.Id, customer.Email.Value, customer.Type);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var expiryDate = DateTime.UtcNow.AddDays(30);

        customer.UpdateRefreshToken(refreshToken, expiryDate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new LoginResponse(
            customer.Id,
            customer.Email.Value,
            token,
            refreshToken,
            DateTime.UtcNow.AddHours(1)
        );

        return new Result<LoginResponse>.Success(response);
    }
}
