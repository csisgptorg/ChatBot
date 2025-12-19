using System;
using System.IO;
using ChatBot.Application;
using ChatBot.Application.Common.Configuration;
using ChatBot.Persistence;
using ChatBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ChatBot.WebApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTypedOptions(configuration);
        services.AddApplicationLayer(configuration);
        services.AddPersistenceAsync(configuration);
        services.AddServicesLayer(configuration);
        services.AddPresentationServices(configuration);
        services.AddApiControllers();
        services.AddSwagger(configuration.GetSection(SwaggerOptions.SectionName).Get<SwaggerOptions>() ?? new());
        return services;
    }

    public static void AddTypedOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));
        services.Configure<CorsOptions>(configuration.GetSection(CorsOptions.SectionName));
        services.Configure<SwaggerOptions>(configuration.GetSection(SwaggerOptions.SectionName));
    }

    public static void AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        var cors = configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>();
        if (cors is not null && cors.AllowedOrigins.Length > 0)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins(cors.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
        }
    }

    public static void AddApiControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.ReturnHttpNotAcceptable = true;
            options.Filters.Add(new ProducesAttribute("application/json"));
        })
        .ConfigureApiBehaviorOptions(setup =>
        {
            setup.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Title = "Validation failed.",
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status422UnprocessableEntity
                };

                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            };
        });
    }

    public static void AddSwagger(this IServiceCollection services, SwaggerOptions options)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(setup =>
        {
            setup.CustomSchemaIds(type => type.ToString());

            if (options.IncludeXmlComments)
            {
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                foreach (var xml in xmlFiles)
                {
                    setup.IncludeXmlComments(xml, true);
                }
            }

            var version = string.IsNullOrWhiteSpace(options.Version) ? "v1" : options.Version;
            setup.SwaggerDoc(version, new OpenApiInfo
            {
                Title = string.IsNullOrWhiteSpace(options.DocumentTitle) ? "ChatBot API" : options.DocumentTitle,
                Description = options.Description,
                Version = version
            });
        });
    }
}
