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
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICustomerLocationAppService _customerLocationAppService;
        public CustomerAppService(ICustomerRepository customerRepository, IMapper mapper, 
            IPasswordHasher passwordHasher, ICustomerLocationAppService customerLocationAppService)
            : base(customerRepository, mapper)
        {
            _customerRepository = customerRepository;
            _passwordHasher = passwordHasher;
            _customerLocationAppService = customerLocationAppService;
        }
        public async Task<CreateGuestResponse> CreateGuestAsync(CreateGuestRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var deviceInfo = new DeviceInfo(
                request.DeviceInfo.UserAgent,
                request.DeviceInfo.Platform,
                request.DeviceInfo.Language,
                request.DeviceInfo.TimeZone
            );

            var customer = new Customer(
                sessionToken: Guid.NewGuid(),
                latitude: request.Latitude.HasValue ? (decimal?)request.Latitude : null,
                longitude: request.Longitude.HasValue ? (decimal?)request.Longitude : null,
                deviceInfo: deviceInfo
            );

            var customerId = await _customerRepository.AddAsync(customer);

            if (request.Latitude.HasValue && request.Longitude.HasValue)
            {
                await _customerLocationAppService.RequestCustomerLocationAsync(
                    customerId,
                    request.Latitude.Value,
                    request.Longitude.Value);
            }

            return new CreateGuestResponse
            {
                SessionToken = customer.SessionToken,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
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
            var existingCustomer = await _customerRepository.GetByEmailAsync(new Email(request.Email));
            if (existingCustomer != null)
            {
                throw new DomainException("Este e-mail já está em uso por outro cliente.");
            }
            var passwordHash = _passwordHasher.HashPassword(request.Password);
            var securePassword = new Password(passwordHash);
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                customer = new Customer(Guid.NewGuid());
                customer.BecomeLead(new Email(request.Email), securePassword);
                await _customerRepository.AddAsync(customer);
            }
            else
            {
                customer.BecomeLead(new Email(request.Email), securePassword);
                await _customerRepository.UpdateAsync(customer);
            }
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