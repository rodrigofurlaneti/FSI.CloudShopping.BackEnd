namespace FSI.CloudShopping.Application.Commands.Customer;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;

public record RegisterGuestCommand(string Email) : IRequest<Result<CustomerDto>>;
