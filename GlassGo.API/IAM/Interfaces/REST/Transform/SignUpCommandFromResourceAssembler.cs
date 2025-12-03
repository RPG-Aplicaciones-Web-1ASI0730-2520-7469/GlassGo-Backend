using GlassGo.API.IAM.Domain.Model.Commands;
using GlassGo.API.IAM.Interfaces.REST.Resources;

namespace GlassGo.API.IAM.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(
            resource.Username, 
            resource.Password,
            resource.Email,
            resource.FirstName,
            resource.LastName,
            resource.Role,
            resource.Phone,
            resource.Company,
            resource.BusinessName,
            resource.TaxId,
            resource.Address,
            resource.PreferredCurrency
        );
    }
}