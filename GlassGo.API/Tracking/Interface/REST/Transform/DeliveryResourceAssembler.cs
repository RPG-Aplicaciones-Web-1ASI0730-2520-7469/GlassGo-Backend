using GlassGo.API.Tracking.Domain.Model.Aggregates;
using GlassGo.API.Tracking.Interface.REST.Resources;

namespace GlassGo.API.Tracking.Interface.REST.Transform
{
    public static class DeliveryResourceAssembler
    {
        public static DeliveryResource ToResourceFromEntity(Delivery entity)
        {
            return new DeliveryResource
            {
                Id = entity.Id.ToString(),
                Status = entity.Status.ToString(),
                Location = entity.location.ToString(),
                Timestamp = entity.timestamp.Value
            };
        }

        public static Delivery ToEntityFromResource(SaveDeliveryResource resource)
        {
            return new Delivery(
                _id: Guid.NewGuid().ToString(), // genera un ID aleatorio para la entrega
                _status: resource.Status,
                _location: resource.Location,
                _timestamp: resource.Timestamp
            );
        }
    }
}


