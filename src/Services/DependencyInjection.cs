using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.Services;

/// <summary>
/// Extension point for registering additional external services.
/// </summary>
public static partial class DependencyInjection
{
    private static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Hook for registering outbound integrations or background services similar to the template repository.
    }
}
