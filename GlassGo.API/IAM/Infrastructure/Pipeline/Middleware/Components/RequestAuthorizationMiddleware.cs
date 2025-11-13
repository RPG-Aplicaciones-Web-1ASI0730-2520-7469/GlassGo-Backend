using GlassGo.API.IAM.Application.Internal.OutboundServices;
using GlassGo.API.IAM.Domain.Model.Queries;
using GlassGo.API.IAM.Domain.Services;
using GlassGo.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace GlassGo.API.IAM.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
/// Middleware that authorizes incoming HTTP requests by validating a JWT token in the Authorization header.
/// </summary>
/// <remarks>
/// If a valid token is present the corresponding <see cref="ACME.LearningCenterPlatform.API.IAM.Domain.Model.Aggregates.User"/> is loaded
/// and placed into <c>HttpContext.Items["User"]</c>. Endpoints decorated with <see cref="AllowAnonymousAttribute"/> are skipped.
/// </remarks>
public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Invoked by the ASP.NET Core pipeline to authorize the request.
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/>.</param>
    /// <param name="userQueryService">Service used to load users by id.</param>
    /// <param name="tokenService">Service used to validate JWT tokens.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    /// <exception cref="Exception">Thrown when the token is missing or invalid.</exception>
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService)
    {
        Console.WriteLine("Entering InvokeAsync");
        // skip authorization if endpoint is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.Request.HttpContext.GetEndpoint()!.Metadata
            .Any(m => m.GetType() == typeof(AllowAnonymousAttribute));
        Console.WriteLine($"Allow Anonymous is {allowAnonymous}");
        if (allowAnonymous)
        {
            Console.WriteLine("Skipping authorization");
            // [AllowAnonymous] attribute is set, so skip authorization
            await next(context);
            return;
        }

        Console.WriteLine("Entering authorization");
        // get token from request header
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


        // if token is null then throw exception
        if (token == null) throw new Exception("Null or invalid token");

        // validate token
        var userId = await tokenService.ValidateToken(token);

        // if token is invalid then throw exception
        if (userId == null) throw new Exception("Invalid token");

        // get user by id
        var getUserByIdQuery = new GetUserByIdQuery(userId.Value);

        // set user in HttpContext.Items["User"]

        var user = await userQueryService.Handle(getUserByIdQuery);
        Console.WriteLine("Successful authorization. Updating Context...");
        context.Items["User"] = user;
        Console.WriteLine("Continuing with Middleware Pipeline");
        // call next middleware
        await next(context);
    }
}