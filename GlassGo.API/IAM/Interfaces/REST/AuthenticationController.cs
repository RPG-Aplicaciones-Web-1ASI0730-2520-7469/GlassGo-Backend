using System.Net.Mime;
using GlassGo.API.IAM.Domain.Services;
using GlassGo.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using GlassGo.API.IAM.Interfaces.REST.Resources;
using GlassGo.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlassGo.API.IAM.Interfaces.REST;
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Authentication endpoints")]
public class AuthenticationController(IUserCommandService userCommandService) : ControllerBase
{
    /// <summary>
    /// Authenticate a user using username and password.
    /// </summary>
    /// <param name="signInResource">The sign-in resource containing credentials.</param>
    /// <returns>Returns an <see cref="IActionResult"/> that contains an <see cref="AuthenticatedUserResource"/> with a JWT token when authentication succeeds.</returns>
    [HttpPost("sign-in")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Sign in",
        Description = "Sign in a user",
        OperationId = "SignIn")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was authenticated", typeof(AuthenticatedUserResource))]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
        var authenticatedUser = await userCommandService.Handle(signInCommand);
        var resource =
            AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                authenticatedUser.token);
        return Ok(resource);
    }

    /// <summary>
    /// Create a new user account (sign up).
    /// </summary>
    /// <param name="signUpResource">The sign-up resource containing the new user's credentials.</param>
    /// <returns>Returns an <see cref="IActionResult"/> that indicates the operation result.</returns>
    [HttpPost("sign-up")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Sign-up",
        Description = "Sign up a new user",
        OperationId = "SignUp")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was created successfully")]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
        await userCommandService.Handle(signUpCommand);
        return Ok(new { message = "User created successfully" });
    }
}