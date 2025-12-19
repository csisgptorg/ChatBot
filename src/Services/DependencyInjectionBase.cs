using ChatBot.Application.Common.Interfaces;
using ChatBot.Persistence;
using ChatBot.Services.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ChatBot.Services;

/// <summary>
/// Dependency registrar for the services layer following the template conventions.
/// </summary>
public static partial class DependencyInjection
{
    /// <summary>
    /// Register cross-cutting services such as current user context, time providers and health checks.
    /// </summary>
    public static void AddServicesLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();

        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>(
                name: "Database",
                failureStatus: HealthStatus.Unhealthy,
                customTestQuery: (db, cancellationToken) => db.SetHealthCheckQuery(cancellationToken));

        services.AddCustomServices(configuration);
    }
}
