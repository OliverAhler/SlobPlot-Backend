using Application.Common;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using SharedConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration.GetSection("AppSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var config = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContextSlobPlot>((options) =>
{
    var dbSettings = config.Database;
    if (string .IsNullOrEmpty(dbSettings.ConnectionString))
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
