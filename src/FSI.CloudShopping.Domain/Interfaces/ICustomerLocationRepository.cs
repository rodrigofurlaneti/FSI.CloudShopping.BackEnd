namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface ICustomerLocationRepository
    {
        Task RequestCustomerLocationAsync(
            int customerId,
            decimal latitude,
            decimal longitude,
            string road = null,
            string suburb = null,
            string cityDistrict = null,
            string city = null,
            string state = null,
            string country = null
        );
    }
}
