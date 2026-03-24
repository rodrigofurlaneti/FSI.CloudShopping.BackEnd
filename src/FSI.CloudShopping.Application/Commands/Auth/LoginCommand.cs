namespace FSI.CloudShopping.Application.Commands.Auth;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;
