using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SharedConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration.GetSection("AppSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var config = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

if (config?.Authentik is null)
    throw new InvalidOperationException("Authentik configuration is missing");
if (config?.Database is null)
    throw new InvalidOperationException("Database configuration is missing");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(config);
builder.Services.AddApplication();

// =====================================================
// ===================== Authentication ================
// =====================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = config.Authentik.Authority;
        options.RequireHttpsMetadata = config.Authentik.RequireHttpsMetadata;
        options.MetadataAddress = config.Authentik.MetadataAddress;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = config.Authentik.Issuer,
            ValidAudience = config.Authentik.ClientId,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
        
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context => Task.CompletedTask,
            OnTokenValidated = context => Task.CompletedTask
        };

    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
