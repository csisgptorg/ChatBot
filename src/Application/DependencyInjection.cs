using System.Reflection;
using AutoMapper;
using ChatBot.Application.Mapping;
using ChatBot.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = new[]
        {
            Assembly.Load("ChatBot.Application"),
            Assembly.Load("ChatBot.Domain")
        };

        services.AddSingleton<IMapper>(_ => ReflectionMappingConfig.BuildMapper(assemblies));
        services.AddScoped<StoryService>();

        return services;
    }
}
