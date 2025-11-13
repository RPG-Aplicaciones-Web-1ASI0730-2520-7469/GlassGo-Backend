namespace GlassGo.API.IAM.Domain.Model.Commands;

/// <summary>
/// Command used to create a new user (sign up).
/// </summary>
/// <param name="Username">The username for the new user.</param>
/// <param name="Password">The plain-text password for the new user.</param>
public record SignUpCommand(string Username, string Password);