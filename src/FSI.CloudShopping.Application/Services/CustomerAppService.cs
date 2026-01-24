using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Customer;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Services
{
    public class CustomerAppService : BaseAppService<Customer, CustomerDTO>, ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerAppService(ICustomerRepository customerRepository, IMapper mapper)
            : base(customerRepository, mapper)
        {
            _customerRepository = customerRepository;
        }

        public override async Task<CustomerDTO> AddAsync(CustomerDTO dto)
        {
            var customer = new Customer(dto.SessionToken);
            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return Mapper.Map<CustomerDTO>(customer);
        }

        public override async Task UpdateAsync(CustomerDTO dto)
        {
            var customer = await _customerRepository.GetByIdAsync(dto.Id);
            if (customer == null) throw new DomainException("Cliente não encontrado.");
            if (dto.IsActive) customer.Activate(); else customer.Deactivate();
            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task<CustomerDTO?> GetByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(new Email(email));
            return Mapper.Map<CustomerDTO>(customer);
        }

        public async Task RegisterLeadAsync(RegisterLeadRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null) throw new DomainException("Cliente não encontrado.");
            customer.BecomeLead(new Email(request.Email), new Password(request.Password));
            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task UpdateToIndividualAsync(RegisterIndividualRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null) throw new DomainException("Cliente não encontrado.");
            customer.BecomeIndividual(request.Cpf, request.FullName, request.BirthDate);
            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task UpdateToCompanyAsync(RegisterCompanyRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null) throw new DomainException("Cliente não encontrado.");
            customer.BecomeCompany(request.Cnpj, request.CompanyName, request.StateTaxId);
            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();
        }
    }
}