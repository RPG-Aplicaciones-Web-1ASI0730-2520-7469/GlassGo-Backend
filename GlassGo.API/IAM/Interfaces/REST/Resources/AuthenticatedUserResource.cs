namespace GlassGo.API.IAM.Interfaces.REST.Resources;

public record AuthenticatedUserResource(
    int Id, 
    string Username, 
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string Token
);