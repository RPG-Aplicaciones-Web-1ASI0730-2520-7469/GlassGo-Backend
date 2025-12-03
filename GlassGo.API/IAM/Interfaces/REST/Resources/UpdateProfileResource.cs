namespace GlassGo.API.IAM.Interfaces.REST.Resources;

public record UpdateProfileResource(
    string FirstName,
    string LastName,
    string Phone,
    string? Company = null,
    string? BusinessName = null,
    string? TaxId = null,
    string? Address = null
);
