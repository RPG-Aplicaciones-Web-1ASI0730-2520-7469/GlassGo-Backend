using GlassGo.API.IAM.Domain.Model.Aggregates;
using GlassGo.API.IAM.Interfaces.REST.Resources;

namespace GlassGo.API.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(
        User user, string token)
    {
        return new AuthenticatedUserResource(
            user.Id, 
            user.Username, 
            user.Email,
            user.FirstName,
            user.LastName,
            user.Role,
            token
        );
    }
}