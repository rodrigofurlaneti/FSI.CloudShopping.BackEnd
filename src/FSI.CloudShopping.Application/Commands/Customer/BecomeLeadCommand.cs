namespace FSI.CloudShopping.Application.Commands.Customer;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public record BecomeLeadCommand(Guid SessionToken, string Email, string Password) : IRequest<Result<CustomerDto>>;

public class BecomeLeadCommandHandler : IRequestHandler<BecomeLeadCommand, Result<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BecomeLeadCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CustomerDto>> Handle(BecomeLeadCommand request, CancellationToken cancellationToken)
    {
        // Find customer by session token
        var customer = await _customerRepository.GetBySessionTokenAsync(request.SessionToken, cancellationToken);
        if (customer == null)
        {
            return new Result<CustomerDto>.Failure("Customer not found");
        }

        try
        {
            // Convert to lead
            customer.BecomeLead();

            // Update email if provided (if different from current)
            if (!string.IsNullOrEmpty(request.Email) && request.Email != customer.Email.Value)
            {
                var newEmail = new Email(request.Email);
                var existingCustomer = await _customerRepository.GetByEmailAsync(newEmail, cancellationToken);
                if (existingCustomer != null)
                {
                    return new Result<CustomerDto>.Failure("Email already in use");
                }
            }

            // Set password
            var password = new Password(request.Password);
            customer.SetPassword(password);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return new Result<CustomerDto>.Success(customerDto);
        }
        catch (Domain.Core.DomainException ex)
        {
            return new Result<CustomerDto>.Failure(ex.Message);
        }
    }
}
