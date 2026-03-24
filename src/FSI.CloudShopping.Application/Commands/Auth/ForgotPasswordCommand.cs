namespace FSI.CloudShopping.Application.Commands.Auth;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public record ForgotPasswordCommand(string Email) : IRequest<Result<bool>>;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<bool>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;

    public ForgotPasswordCommandHandler(
        ICustomerRepository customerRepository,
        IEmailService emailService,
        IJwtService jwtService)
    {
        _customerRepository = customerRepository;
        _emailService = emailService;
        _jwtService = jwtService;
    }

    public async Task<Result<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var customer = await _customerRepository.GetByEmailAsync(email, cancellationToken);

        // Sempre retorna sucesso para não revelar se o e-mail existe (security best practice)
        if (customer == null)
            return new Result<bool>.Success(true);

        var resetToken = _jwtService.GenerateRefreshToken();

        await _emailService.SendPasswordResetEmailAsync(
            customer.Email.Value, resetToken, cancellationToken);

        return new Result<bool>.Success(true);
    }
}
