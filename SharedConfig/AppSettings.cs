namespace SharedConfig;

public class AppSettings
{
    public DatabaseSettings Database { get; init; } = new();
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
}
