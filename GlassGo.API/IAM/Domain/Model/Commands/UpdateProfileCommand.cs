namespace GlassGo.API.IAM.Domain.Model.Commands;

/// <summary>
/// Command to update user profile information.
/// </summary>
public record UpdateProfileCommand(
    int UserId,
    string FirstName,
    string LastName,
    string Phone,
    string? Company = null,
    string? BusinessName = null,
    string? TaxId = null,
    string? Address = null
);
