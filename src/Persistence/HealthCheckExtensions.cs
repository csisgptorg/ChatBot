using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Persistence;

public static class HealthCheckExtensions
{
    /// <summary>
    /// Simple database connectivity check used by health checks.
    /// </summary>
    public static async Task<bool> SetHealthCheckQuery(this AppDbContext dbContext, CancellationToken cancellationToken)
    {
        return await dbContext.Database.CanConnectAsync(cancellationToken);
    }
}
