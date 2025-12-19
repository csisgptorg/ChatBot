namespace ChatBot.Application.Common.Configuration;

public class SwaggerOptions
{
    public const string SectionName = nameof(SwaggerOptions);

    public string DocumentTitle { get; set; } = "ChatBot API";

    public string Description { get; set; } = "API documentation";

    public string Version { get; set; } = "v1";

    public bool IncludeXmlComments { get; set; } = false;
}
