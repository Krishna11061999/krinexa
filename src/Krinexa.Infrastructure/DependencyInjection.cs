using Krinexa.Application.Interfaces;
using Krinexa.Infrastructure.Persistence;
using Krinexa.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krinexa.Infrastructure;

// [ADDED 2026-09-03] DI wiring — registers all infrastructure services
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core + Npgsql — DbContext is scoped (never singleton)
        services.AddDbContext<KrinexaDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Default"),
                npgsql => npgsql.EnableRetryOnFailure(maxRetryCount: 3)
            ));

        // [ADDED 2026-09-03] Register all service implementations
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
