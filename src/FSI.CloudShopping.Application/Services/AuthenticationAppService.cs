using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Services
{
    public class AuthenticationAppService : IAuthenticationAppService
    {
        private readonly IAuthenticationRepository _authenticationRepository; 
        private readonly ICustomerRepository _customerRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        public AuthenticationAppService(IAuthenticationRepository authenticationRepository, ICustomerRepository customerRepository,
             IEmailService emailService, IPasswordHasher passwordHasher)
        {
            _authenticationRepository = authenticationRepository;
            _customerRepository = customerRepository;
            _emailService = emailService;
            _passwordHasher = passwordHasher;

        }
        public async Task<int> InsertAsync(string email, bool isAuthorized)
        {
            return await _authenticationRepository.InsertAsync(email, isAuthorized);
        }
        public async Task<bool> GetAccessAsync(string email, string password)
        {
            return await _authenticationRepository.GetAccessAsync(email, password);
        }
        public async Task ResetPasswordAsync(string email)
        {
            var emailAddress = new Email(email);
            var customer = await _customerRepository.GetByEmailAsync(emailAddress);
            if (customer == null) throw new DomainException("Usuário não encontrado.");
            string defaultPassword = "123Mudar@";
            var passwordHash = _passwordHasher.HashPassword(defaultPassword);
            var securePassword = new Password(passwordHash);
            customer.ResetPassword(securePassword);
            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();
            await _emailService.SendResetPasswordEmailAsync(email, defaultPassword);
        }
    }
}