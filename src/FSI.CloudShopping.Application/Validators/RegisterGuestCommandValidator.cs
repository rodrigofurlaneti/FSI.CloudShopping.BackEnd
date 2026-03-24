namespace FSI.CloudShopping.Application.Validators;

using FluentValidation;
using FSI.CloudShopping.Application.Commands.Customer;

public class RegisterGuestCommandValidator : AbstractValidator<RegisterGuestCommand>
{
    public RegisterGuestCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is invalid")
            .MaximumLength(254).WithMessage("Email must not exceed 254 characters");
    }
}
