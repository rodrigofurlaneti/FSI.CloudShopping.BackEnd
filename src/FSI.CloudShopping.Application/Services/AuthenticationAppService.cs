using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Interfaces;

namespace FSI.CloudShopping.Application.Services
{
    public class AuthenticationAppService : IAuthenticationAppService
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationAppService(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public async Task<int> InsertAsync(string email, bool isAuthorized)
        {
            return await _authenticationRepository.InsertAsync(email, isAuthorized);
        }

        public async Task<bool> GetAccessAsync(string email, string password)
        {
            return await _authenticationRepository.GetAccessAsync(email, password);
        }
    }
}