using Application.Common;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddDbContext<ContextSlobPlot>((options) =>
{
    var dbSettings = config?.Database;
    if (string .IsNullOrEmpty(dbSettings?.ConnectionString))
        throw new InvalidOperationException("Database configuration is missing or invalid");

    options.UseNpgsql(dbSettings.ConnectionString);
});

builder.Services.AddConfigureInfrastructure();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IHandler>()
    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IHandler>()
    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddScoped<IDispatcher, Dispatcher>();

// =====================================================
// ===================== Authentication ================
// =====================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = config.Authentik.Authority;
        options.RequireHttpsMetadata = true;
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
