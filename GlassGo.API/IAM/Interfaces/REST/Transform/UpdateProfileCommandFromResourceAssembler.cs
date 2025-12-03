using GlassGo.API.IAM.Domain.Model.Commands;
using GlassGo.API.IAM.Interfaces.REST.Resources;

namespace GlassGo.API.IAM.Interfaces.REST.Transform;

public static class UpdateProfileCommandFromResourceAssembler
{
    public static UpdateProfileCommand ToCommandFromResource(int userId, UpdateProfileResource resource)
    {
        return new UpdateProfileCommand(
            userId,
            resource.FirstName,
            resource.LastName,
            resource.Phone,
            resource.Company,
            resource.BusinessName,
            resource.TaxId,
            resource.Address
        );
    }
}
