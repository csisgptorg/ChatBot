using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace ChatBot.Application.Mapping
{
    public static class ReflectionMappingConfig
    {
        public static IMapper BuildMapper(params Assembly[] assemblies)
        {
            var allAssemblies = assemblies != null && assemblies.Length > 0
                ? assemblies
                : new[] { Assembly.GetExecutingAssembly() };

            var typesWithAttribute = allAssemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttributes(typeof(AutoMapAttribute), true).Any())
                .ToList();

            var configuration = new MapperConfiguration(cfg =>
            {
                foreach (var type in typesWithAttribute)
                {
                    var attributes = type.GetCustomAttributes<AutoMapAttribute>();
                    foreach (var attribute in attributes)
                    {
                        RegisterMap(cfg, type, attribute);
                    }
                }
            });

            configuration.AssertConfigurationIsValid();
            return configuration.CreateMapper();
        }

        private static void RegisterMap(IMapperConfigurationExpression cfg, Type type, AutoMapAttribute attribute)
        {
            switch (attribute.Direction)
            {
                case AutoMapDirection.To:
                    cfg.CreateMap(type, attribute.TargetType);
                    break;
                case AutoMapDirection.From:
                    cfg.CreateMap(attribute.TargetType, type);
                    break;
                default:
                    cfg.CreateMap(type, attribute.TargetType);
                    cfg.CreateMap(attribute.TargetType, type);
                    break;
            }
        }
    }
}
