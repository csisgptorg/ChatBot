using System;

namespace ChatBot.Application.Common.Configuration;

public class CorsOptions
{
    public const string SectionName = nameof(CorsOptions);

    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
}
