using FSI.CloudShopping.Domain.Entities;
namespace FSI.CloudShopping.Application.Brokers
{
    public interface INominatimService
    {
        Task<CustomerLocation> GetLocationAsync(decimal latitude, decimal longitude);
    }
}
