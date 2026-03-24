namespace FSI.CloudShopping.Application.Commands.Customer;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public record RegisterIndividualCommand(
    Guid CustomerId,
    string TaxId,
    string FullName,
    DateTime? BirthDate = null
) : IRequest<Result<CustomerDto>>;

public class RegisterIndividualCommandHandler : IRequestHandler<RegisterIndividualCommand, Result<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterIndividualCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CustomerDto>> Handle(RegisterIndividualCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            return new Result<CustomerDto>.Failure("Customer not found");
        }

        try
        {
            var taxId = new TaxId(request.TaxId);
            var personName = PersonName.Create(request.FullName);
            var birthDate = request.BirthDate ?? DateTime.UtcNow;

            customer.BecomeIndividual(personName, taxId, birthDate);
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
