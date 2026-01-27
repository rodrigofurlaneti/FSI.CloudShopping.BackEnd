using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Brokers;
using FSI.CloudShopping.Domain.Interfaces;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Application.Services
{
    public class CustomerLocationAppService : ICustomerLocationAppService
    {
        private readonly INominatimService _nominatimService;
        private readonly ICustomerLocationRepository _repository;

        public CustomerLocationAppService(
            INominatimService nominatimService,
            ICustomerLocationRepository repository)
        {
            _nominatimService = nominatimService;
            _repository = repository;
        }

        public async Task RequestCustomerLocationAsync(int customerId, decimal latitude, decimal longitude)
        {
            var location = await _nominatimService.GetLocationAsync(latitude, longitude);
            string road = location.Road;
            string suburb = location.Suburb;
            string cityDistrict = location.CityDistrict;
            string city = location.City;
            string state = location.State;
            string country = location.Country;
            await _repository.RequestCustomerLocationAsync(
                customerId,
                latitude,
                longitude,
                road,
                suburb,
                cityDistrict,
                city,
                state,
                country
            );
        }
    }
}
