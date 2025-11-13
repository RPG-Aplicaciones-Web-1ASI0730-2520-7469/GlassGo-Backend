using GlassGo.API.Shared.Domain.Repositories;
using GlassGo.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using GlassGo.API.Tracking.Infrastructure.Persistence.EFC.Contexts;
using GlassGo.API.Tracking.Application.Internal.CommandServices;
using GlassGo.API.Tracking.Application.Internal.QueryServices;
using GlassGo.API.Tracking.Domain.Repositories;
using GlassGo.API.Tracking.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// -------------------------------
//  Add services to the container
// -------------------------------

// Localization Configuration
builder.Services.AddLocalization();

builder.Services.AddControllers(options =>
        options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

// Swagger & OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());
builder.Services.AddOpenApi();

//  Authentication (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
        options.RequireHttpsMetadata = false;
    });

// ------------------------------------
// Database Connections Configuration
// ------------------------------------

// Default (Shared) Context
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null)
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
}
else if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionStringTemplate = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionStringTemplate))
            throw new Exception("Database connection string template is not set in the configuration.");

        var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Database connection string is not set in the configuration.");

        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
    });
}

// Add Tracking Context (Bounded Context)
builder.Services.AddDbContext<TrackingDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySQL(connectionString);
});

// ------------------------------------
// Dependency Injection Configuration
// ------------------------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<DeliveryCommandService>();
builder.Services.AddScoped<DeliveryQueryService>();
// ------------------------------------
// Build Application
// ------------------------------------
var app = builder.Build();

// ------------------------------------
// Middleware Pipeline
// ------------------------------------
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// 🗄️ Database verification
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Localization configuration
var supportedCultures = new[] { "en", "en-US", "es", "es-PE" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

app.UseRequestLocalization(localizationOptions);

//  Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

// Redirect root to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});


app.Run();
