namespace FSI.CloudShopping.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Infrastructure.Data;
using FSI.CloudShopping.Infrastructure.Repositories;
using FSI.CloudShopping.Infrastructure.Security;
using FSI.CloudShopping.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        // Security & JWT
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtService, JwtService>();

        // Cache
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();

        // Application Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IViaCepService, ViaCepService>();
        services.AddHttpClient<ViaCepService>();

        // Domain Services
        services.AddScoped<FSI.CloudShopping.Domain.Services.IStockDomainService, StockDomainService>();
        services.AddScoped<FSI.CloudShopping.Domain.Services.ICouponDomainService, CouponDomainService>();

        // Payment Gateway (MercadoPago — configurável via appsettings)
        services.AddHttpClient<MercadoPagoPaymentService>();
        services.AddScoped<IPaymentGatewayService, MercadoPagoPaymentService>();

        return services;
    }
}
