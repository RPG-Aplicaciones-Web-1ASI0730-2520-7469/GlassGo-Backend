using GlassGo.API.IAM.Domain.Model.Aggregates;
using GlassGo.API.IAM.Interfaces.REST.Resources;

namespace GlassGo.API.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(user.Id, user.Username);
    }
}