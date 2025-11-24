using GlassGo.API.Shared.Domain.Repositories;
using GlassGo.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using GlassGo.API.Analytics.Domain.Entities;
using GlassGo.API.Analytics.Domain.Interfaces;
using GlassGo.API.Analytics.Domain.Services;
using GlassGo.API.Analytics.Infrastructure.Data;
using GlassGo.API.Analytics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Localization Configuration
builder.Services.AddLocalization();

builder.Services.AddControllers(options =>
        options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());
builder.Services.AddOpenApi();


// Add Database Connection
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(options => {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null) 
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        var connectionStringTemplate = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionStringTemplate)) 
            // Stop the application if the connection string template is not set.
            throw new Exception("Database connection string template is not set in the configuration.");
        var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
        if (string.IsNullOrEmpty(connectionString))
            // Stop the application if the connection string is not set.
            throw new Exception("Database connection string is not set in the configuration.");
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
    });

// Configure Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// Verify Database Objects Creation
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated(); // Comentado temporalmente para evitar error de conexión a MySQL
}

// Localization Configuration
var supportedCultures = new[] { "en", "en-US", "es", "es-PE" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection(); // Comentado para evitar error por falta de puerto HTTPS

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", context => {
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.Run();