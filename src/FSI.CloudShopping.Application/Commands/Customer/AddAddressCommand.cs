namespace FSI.CloudShopping.Application.Commands.Customer;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.Entities;

public record AddAddressCommand(
    Guid CustomerId,
    AddressType AddressType,
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    bool IsDefault = false
) : IRequest<Result<AddressDto>>;

public class AddAddressCommandHandler : IRequestHandler<AddAddressCommand, Result<AddressDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddAddressCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<AddressDto>> Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            return new Result<AddressDto>.Failure("Customer not found");
        }

        try
        {
            var address = new Address(
                Guid.NewGuid(),
                request.CustomerId,
                request.AddressType,
                request.Street,
                request.Number,
                request.Complement,
                request.Neighborhood,
                request.City,
                request.State,
                request.ZipCode,
                "BR",
                request.IsDefault
            );

            customer.AddAddress(address);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var addressDto = _mapper.Map<AddressDto>(address);
            return new Result<AddressDto>.Success(addressDto);
        }
        catch (Domain.Core.DomainException ex)
        {
            return new Result<AddressDto>.Failure(ex.Message);
        }
    }
}
