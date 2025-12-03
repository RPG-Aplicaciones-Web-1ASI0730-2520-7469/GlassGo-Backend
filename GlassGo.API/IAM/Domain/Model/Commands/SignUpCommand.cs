namespace GlassGo.API.IAM.Domain.Model.Commands;

/// <summary>
/// Command used to create a new user (sign up).
/// </summary>
public record SignUpCommand(
    string Username, 
    string Password,
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string Phone,
    string? Company = null,
    string? BusinessName = null,
    string? TaxId = null,
    string? Address = null,
    string PreferredCurrency = "PEN"
);