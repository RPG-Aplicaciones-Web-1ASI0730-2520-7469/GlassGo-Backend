using GlassGo.API.IAM.Application.ACL.Services;
using GlassGo.API.IAM.Application.Internal.CommandServices;
using GlassGo.API.IAM.Application.Internal.OutboundServices;
using GlassGo.API.IAM.Application.Internal.QueryServices;
using GlassGo.API.IAM.Domain.Repositories;
using GlassGo.API.IAM.Domain.Services;
using GlassGo.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using GlassGo.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using GlassGo.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using GlassGo.API.IAM.Infrastructure.Tokens.JWT.Services;
using GlassGo.API.IAM.Interfaces.ACL;

namespace GlassGo.API.IAM.Infrastructure.Interfaces.ASP.Configuration.Extensions;

/// <summary>
///     Extension methods for configuring IAM context services in a WebApplicationBuilder.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    ///     Adds the IAM context services to the WebApplicationBuilder.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder to configure.</param>
    public static void AddIamContextServices(this WebApplicationBuilder builder)
    {
        // IAM Bounded Context Injection Configuration

        // TokenSettings Configuration

        builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserCommandService, UserCommandService>();
        builder.Services.AddScoped<IUserQueryService, UserQueryService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IHashingService, HashingService>();
        builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
    }
}