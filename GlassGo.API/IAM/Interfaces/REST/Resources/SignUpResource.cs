namespace GlassGo.API.IAM.Interfaces.REST.Resources;

public record SignUpResource(
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