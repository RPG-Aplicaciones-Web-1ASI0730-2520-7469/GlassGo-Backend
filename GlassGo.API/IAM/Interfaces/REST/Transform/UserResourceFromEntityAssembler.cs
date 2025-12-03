using GlassGo.API.IAM.Domain.Model.Aggregates;
using GlassGo.API.IAM.Interfaces.REST.Resources;

namespace GlassGo.API.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(
            user.Id, 
            user.Username, 
            user.Email,
            user.FirstName,
            user.LastName,
            user.Role,
            user.Company,
            user.BusinessName,
            user.TaxId,
            user.Address,
            user.Phone,
            user.PreferredCurrency,
            user.LoyaltyPoints,
            user.IsActive,
            user.CreatedAt
        );
    }
}