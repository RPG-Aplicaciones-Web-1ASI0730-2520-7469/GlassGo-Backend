namespace GlassGo.API.IAM.Interfaces.REST.Resources;

public record UserResource(
    int Id, 
    string Username, 
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string Company,
    string? BusinessName,
    string? TaxId,
    string? Address,
    string Phone,
    string PreferredCurrency,
    int LoyaltyPoints,
    bool IsActive,
    DateTime CreatedAt
);