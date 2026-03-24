namespace FSI.CloudShopping.Application.Commands.Customer;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public class RegisterGuestCommandHandler : IRequestHandler<RegisterGuestCommand, Result<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterGuestCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CustomerDto>> Handle(RegisterGuestCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);

        var emailExists = await _customerRepository.EmailExistsAsync(email, cancellationToken);
        if (emailExists)
            return new Result<CustomerDto>.Failure("Email already registered");

        var customer = Domain.Entities.Customer.CreateGuest(email);
        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var customerDto = _mapper.Map<CustomerDto>(customer);
        return new Result<CustomerDto>.Success(customerDto);
    }
}
