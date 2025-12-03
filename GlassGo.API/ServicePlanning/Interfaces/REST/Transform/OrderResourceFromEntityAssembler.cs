using GlassGo.API.ServicePlanning.Domain.Model.Aggregates;
using GlassGo.API.ServicePlanning.Interfaces.REST.Resources;

namespace GlassGo.API.ServicePlanning.Interfaces.REST.Transform;

public static class OrderResourceFromEntityAssembler
{
    public static OrderResource ToResourceFromEntity(Order entity)
    {
        return new OrderResource(
            entity.Id,
            entity.CustomerName,
            entity.CustomerEmail,
            entity.CustomerPhone,
            entity.ServiceType,
            entity.Description,
            entity.PreferredDate,
            entity.Status,
            entity.Notes,
            entity.CreatedDate,
            entity.UpdatedDate
        );
    }
}

