using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Infrastructure.Repositories
{
    public class CustomerLocationRepository : ICustomerLocationRepository
    {
        private readonly SqlDbConnector _connector;

        public CustomerLocationRepository(SqlDbConnector connector)
        {
            _connector = connector;
        }

        public async Task RequestCustomerLocationAsync(
            int customerId,
            decimal latitude,
            decimal longitude,
            string? road = null,
            string? suburb = null,
            string? cityDistrict = null,
            string? city = null,
            string? state = null,
            string? country = null)
        {
            using var cmd = await _connector.CreateProcedureCommandAsync("Procedure_CustomersLocation_Insert");

            cmd.Parameters.AddWithValue("@IdCustomers", customerId);
            cmd.Parameters.AddWithValue("@Latitude", latitude);
            cmd.Parameters.AddWithValue("@Longitude", longitude);
            cmd.Parameters.AddWithValue("@Road", (object?)road ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Suburb", (object?)suburb ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CityDistrict", (object?)cityDistrict ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@City", (object?)city ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@State", (object?)state ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Country", (object?)country ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
