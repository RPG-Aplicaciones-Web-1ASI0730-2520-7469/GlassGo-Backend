using GlassGo.API.Payments.Application.Internal.CommandServices;
using GlassGo.API.Payments.Application.Internal.QueryServices;
using GlassGo.API.Payments.Domain.Repositories;
using GlassGo.API.Payments.Domain.Services;
using GlassGo.API.Payments.Infrastructure.Persistence.EFC.Repositories;

namespace GlassGo.API.Payments.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddPaymentsContext(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        // Repositories
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

        // Domain/Application Services
        services.AddScoped<IPaymentCommandService, PaymentCommandService>();
        services.AddScoped<IPaymentQueryService, PaymentQueryService>();

        return builder;
    }
}