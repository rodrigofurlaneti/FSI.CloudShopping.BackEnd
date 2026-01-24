using AutoMapper;
using FSI.CloudShopping.Application.DTOs.Contact;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Services
{
    public class ContactAppService : BaseAppService<Contact, ContactDTO>, IContactAppService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICustomerRepository _customerRepository;

        public ContactAppService(IContactRepository repository,
                                 ICustomerRepository customerRepository,
                                 IMapper mapper) : base(repository, mapper)
        {
            _contactRepository = repository;
            _customerRepository = customerRepository;
        }

        public override async Task<ContactDTO> AddAsync(ContactDTO dto)
        {
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null) throw new DomainException("Cliente não encontrado.");
            var contact = new Contact(
                customer.Id,
                new PersonName(dto.Name),
                new Email(dto.Email),
                new Phone(dto.Phone),
                dto.Position);
            customer.AddContact(contact);
            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return Mapper.Map<ContactDTO>(contact);
        }

        public async Task<IEnumerable<ContactDTO>> GetByCustomerIdAsync(int customerId)
        {
            var contacts = await _contactRepository.GetByCustomerIdAsync(customerId);
            return Mapper.Map<IEnumerable<ContactDTO>>(contacts);
        }
    }
}