namespace FSI.CloudShopping.Application;

using Microsoft.Extensions.DependencyInjection;
using MediatR;
using AutoMapper;
using FluentValidation;
using FSI.CloudShopping.Application.Behaviors;
using FSI.CloudShopping.Application.Sagas;
using FSI.CloudShopping.Application.Sagas.Steps;
using System.Reflection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR — auto-registers all IRequestHandler, INotificationHandler, etc.
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // MediatR Pipeline Behaviors (order matters: logging wraps validation)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // ── SAGA Registration ─────────────────────────────────────────────────
        // Steps are transient so each saga execution gets a fresh step instance.
        services.AddTransient<ReserveStockStep>();
        services.AddTransient<ProcessPaymentStep>();
        services.AddTransient<ConfirmOrderStep>();
        services.AddTransient<SendNotificationStep>();

        // The orchestrator itself is transient — stateless, steps injected per request.
        services.AddTransient<OrderCheckoutSagaOrchestrator>();

        return services;
    }
}
