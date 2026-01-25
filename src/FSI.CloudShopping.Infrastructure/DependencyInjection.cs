using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Repositories;
using FSI.CloudShopping.Infrastructure.Security; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSI.CloudShopping.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Production");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("A ConnectionString 'Production' não foi encontrada no appsettings.json.");
            services.AddScoped(sp => new SqlDbConnector(connectionString));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            return services;
        }
    }
}