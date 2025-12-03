using System.Net.Mime;
using GlassGo.API.IAM.Application.Internal.OutboundServices;
using GlassGo.API.IAM.Domain.Services;
using GlassGo.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using GlassGo.API.IAM.Interfaces.REST.Resources;
using GlassGo.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlassGo.API.IAM.Interfaces.REST;
[Authorize]
[ApiController]
[Route("api/v1")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Authentication endpoints")]
public class AuthenticationController(
    IUserCommandService userCommandService,
    ITokenService tokenService) : ControllerBase
{
    /// <summary>
    /// Authenticate a user using username and password.
    /// </summary>
    /// <param name="signInResource">The sign-in resource containing credentials.</param>
    /// <returns>Returns an <see cref="IActionResult"/> that contains an <see cref="AuthenticatedUserResource"/> with a JWT token when authentication succeeds.</returns>
    [HttpPost("sessions")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Create session",
        Description = "Create a new user session (sign in)",
        OperationId = "CreateSession")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was authenticated", typeof(AuthenticatedUserResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        try
        {
            var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
            var authenticatedUser = await userCommandService.Handle(signInCommand);
            var resource =
                AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                    authenticatedUser.token);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new user account (sign up).
    /// </summary>
    /// <param name="signUpResource">The sign-up resource containing the new user's credentials.</param>
    /// <returns>Returns an <see cref="IActionResult"/> that indicates the operation result.</returns>
    [HttpPost("users")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Create user",
        Description = "Create a new user account (sign up)",
        OperationId = "CreateUser")]
    [SwaggerResponse(StatusCodes.Status201Created, "The user was created successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request or user already exists")]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        try
        {
            var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
            await userCommandService.Handle(signUpCommand);
            return Created(string.Empty, new { success = true, message = "User created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
    
    /// <summary>
    /// Validate a JWT token.
    /// </summary>
    /// <param name="token">The token to validate.</param>
    /// <returns>Validation result.</returns>
    [HttpPost("tokens/validations")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Create token validation",
        Description = "Validate if a JWT token is valid",
        OperationId = "CreateTokenValidation")]
    [SwaggerResponse(StatusCodes.Status200OK, "Token is valid")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Token is invalid or expired")]
    public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenResource resource)
    {
        try
        {
            var userId = await tokenService.ValidateToken(resource.Token);
            if (userId.HasValue)
            {
                return Ok(new { valid = true, userId = userId.Value });
            }
            return Unauthorized(new { valid = false, message = "Invalid or expired token" });
        }
        catch
        {
            return Unauthorized(new { valid = false, message = "Invalid or expired token" });
        }
    }
    
    /// <summary>
    /// Logout a user (invalidate token).
    /// </summary>
    /// <returns>Logout confirmation.</returns>
    [HttpDelete("sessions")]
    [SwaggerOperation(
        Summary = "Delete session",
        Description = "Delete user session (logout)",
        OperationId = "DeleteSession")]
    [SwaggerResponse(StatusCodes.Status200OK, "Logout successful")]
    public IActionResult Logout()
    {
        // In a stateless JWT implementation, logout is handled client-side by removing the token
        // In a stateful implementation, you would invalidate the token in a blacklist/database
        return Ok(new { success = true, message = "Logout successful" });
    }
    
    /// <summary>
    /// Send password reset email.
    /// </summary>
    /// <param name="resource">The forgot password resource containing the email.</param>
    /// <returns>Password reset email sent confirmation.</returns>
    [HttpPost("passwords/resets")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Create password reset",
        Description = "Request a password reset email",
        OperationId = "CreatePasswordReset")]
    [SwaggerResponse(StatusCodes.Status200OK, "Password reset email sent")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Email not found")]
    public IActionResult ForgotPassword([FromBody] ForgotPasswordResource resource)
    {
        // TODO: Implement actual email sending logic
        // For now, return success response (demo mode)
        return Ok(new { success = true, message = "Password reset email sent (demo mode)" });
    }
}