using ChatBot.Application.Common.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChatBot.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceAsync(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? configure = null)
    {
        var dbOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>() ?? new();

        void ConfigureDb(DbContextOptionsBuilder options)
        {
            if (configure is not null)
            {
                configure(options);
                return;
            }

            if (dbOptions.UseInMemoryDatabase)
            {
                options.UseInMemoryDatabase("chatbot");
            }
            else if (!string.IsNullOrWhiteSpace(dbOptions.ConnectionStrings.SqlServer))
            {
                options.UseSqlServer(dbOptions.ConnectionStrings.SqlServer,
                    sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "dbo"));
            }

            if (dbOptions.EnableLogging)
            {
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));

                if (dbOptions.EnableSensitiveDataLogging)
                {
                    options.EnableSensitiveDataLogging();
                }
            }
        }

        if (dbOptions.EnablePooling)
        {
            var poolSize = dbOptions.MaxPoolSize.HasValue && dbOptions.MaxPoolSize > 0
                ? dbOptions.MaxPoolSize.Value
                : 1024;

            services.AddDbContextPool<AppDbContext>(ConfigureDb, poolSize);
        }
        else
        {
            services.AddDbContext<AppDbContext>(ConfigureDb);
        }

        return services;
    }
}
