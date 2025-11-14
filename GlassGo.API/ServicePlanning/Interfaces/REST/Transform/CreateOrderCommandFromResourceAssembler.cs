using GlassGo.API.ServicePlanning.Domain.Model.Commands;
using GlassGo.API.ServicePlanning.Interfaces.REST.Resources;

namespace GlassGo.API.ServicePlanning.Interfaces.REST.Transform;

public static class CreateOrderCommandFromResourceAssembler
{
    public static CreateOrderCommand ToCommandFromResource(CreateOrderResource resource)
    {
        return new CreateOrderCommand(
            resource.CustomerName,
            resource.CustomerEmail,
            resource.CustomerPhone,
            resource.ServiceType,
            resource.Description,
            resource.PreferredDate,
            resource.Notes
        );
    }
}

