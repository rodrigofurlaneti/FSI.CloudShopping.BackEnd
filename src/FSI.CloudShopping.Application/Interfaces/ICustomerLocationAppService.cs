namespace FSI.CloudShopping.Application.Interfaces
{
    public interface ICustomerLocationAppService
    {
        Task RequestCustomerLocationAsync(int customerId, decimal latitude, decimal longitude);
    }
}
