using System.Text;
using GlassGo.API.Shared.Domain.Repositories;
using GlassGo.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using GlassGo.API.Analytics.Domain.Entities;
using GlassGo.API.Analytics.Domain.Interfaces;
using GlassGo.API.Analytics.Domain.Services;
using GlassGo.API.Analytics.Infrastructure.Data;
using GlassGo.API.Analytics.Infrastructure.Repositories;
using GlassGo.API.IAM.Application.Internal.CommandServices;
using GlassGo.API.IAM.Application.Internal.OutboundServices;
using GlassGo.API.IAM.Application.Internal.QueryServices;
using GlassGo.API.IAM.Domain.Repositories;
using GlassGo.API.IAM.Domain.Services;
using GlassGo.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using GlassGo.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using GlassGo.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using GlassGo.API.IAM.Infrastructure.Tokens.JWT.Services;
using GlassGo.API.Payments.Application.Internal.ComandServices;
using GlassGo.API.Payments.Application.Internal.QueryServices;
using GlassGo.API.Payments.Domain.Repositories;
using GlassGo.API.Payments.Domain.Services;
using GlassGo.API.Payments.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Use InMemory database for development (no MySQL required)
    options.UseInMemoryDatabase("GlassGoDb")
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});

builder.Services.AddDbContext<DashboardAnalyticsContext>(options =>
{
    // Use InMemory database for development (no MySQL required)
    options.UseInMemoryDatabase("GlassGoAnalyticsDb")
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});


// Configure JWT Settings
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

// Configure Dependency Injection
// Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAM
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();

// Payments
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IPaymentCommandService, PaymentComandService>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<IPaymentQueryService, PaymentQueryService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();


// Analytics
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ReportService>();


// Configure Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>();
        if (tokenSettings == null) throw new InvalidOperationException("TokenSettings is not configured");
        var key = Encoding.ASCII.GetBytes(tokenSettings.Secret);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = "role"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// Verify Database Objects Creation
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     dbContext.Database.EnsureCreated();
// }

// Localization Configuration
var supportedCultures = new[] { "en", "en-US", "es", "es-PE" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", context => {
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.Run();