namespace ChatBot.Application.Common.Configuration;

public class DatabaseOptions
{
    public const string SectionName = nameof(DatabaseOptions);

    public bool UseInMemoryDatabase { get; set; } = true;

    public bool EnablePooling { get; set; } = false;

    public bool EnableLogging { get; set; } = false;

    public bool EnableSensitiveDataLogging { get; set; } = false;

    public int? MaxPoolSize { get; set; }

    public ConnectionStrings ConnectionStrings { get; set; } = new();
}

public class ConnectionStrings
{
    public string? SqlServer { get; set; }
}
