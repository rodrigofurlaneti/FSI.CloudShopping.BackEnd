using FSI.CloudShopping.Domain.Entities;

namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IAuthenticationRepository : IRepository<Authentication>
    {
        Task<int> InsertAsync(string email, bool isAuthorized);
       Task<bool> GetAccessAsync(string email, string password);
    }
}