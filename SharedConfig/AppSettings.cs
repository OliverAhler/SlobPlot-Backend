namespace SharedConfig;

public class AppSettings
{
    public DatabaseSettings Database { get; set; } = new();
}

public class DatabaseSettings
{
    public string Host { get; set; } = string.Empty;
    public string Port { get; set; } = "5432";
    public string DatabaseName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool SslMode { get; set; } = true;
    
    public string ConnectionString => 
        $"Host={Host};Port={Port};Database={DatabaseName};Username={UserName};Password={Password};SSL Mode={(SslMode ? "Require" : "Disable")};";
}
