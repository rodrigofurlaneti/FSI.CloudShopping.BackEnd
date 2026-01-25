using Microsoft.Extensions.DependencyInjection;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Services;

namespace FSI.CloudShopping.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IOrderAppService, OrderAppService>();
            services.AddScoped<ICartAppService, CartAppService>();
            services.AddScoped<IAddressAppService, AddressAppService>();
            services.AddScoped<IContactAppService, ContactAppService>();
            services.AddScoped<IPaymentAppService, PaymentAppService>();

            return services;
        }
    }
}