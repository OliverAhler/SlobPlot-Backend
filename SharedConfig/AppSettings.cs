namespace SharedConfig;

public class AppSettings
{
    
    public DatabaseSettings Database { get; init; } = new();
    public AuthentikSettings Authentik { get; init; } = new();
}

public class AuthentikSettings
{
    public string Authority { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string MetadataAddress { get; set; } = string.Empty;
    
    public bool RequireHttpsMetadata { get; set; } = false;
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
}
