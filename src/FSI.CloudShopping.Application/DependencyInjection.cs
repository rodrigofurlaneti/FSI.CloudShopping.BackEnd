using FSI.CloudShopping.Application.Brokers;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FSI.CloudShopping.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationAppService, AuthenticationAppService>();
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<ICustomerLocationAppService, CustomerLocationAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IOrderAppService, OrderAppService>();
            services.AddScoped<ICartAppService, CartAppService>();
            services.AddScoped<IAddressAppService, AddressAppService>();
            services.AddScoped<IContactAppService, ContactAppService>();
            services.AddScoped<IPaymentAppService, PaymentAppService>();
            services.AddSingleton<HttpClient>();
            services.AddScoped<INominatimService, NominatimService>();
            return services;
        }
    }
}