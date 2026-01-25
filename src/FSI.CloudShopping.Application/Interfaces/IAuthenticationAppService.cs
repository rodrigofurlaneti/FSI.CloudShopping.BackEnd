namespace FSI.CloudShopping.Application.Interfaces
{
    public interface IAuthenticationAppService
    {
        Task<int> InsertAsync(string email, bool isAuthorized);
        Task<bool> GetAccessAsync(string email, string password);
    }
}