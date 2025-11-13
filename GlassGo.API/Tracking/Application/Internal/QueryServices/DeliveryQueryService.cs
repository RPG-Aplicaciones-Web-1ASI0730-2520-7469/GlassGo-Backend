using GlassGo.API.Tracking.Domain.Repositories;

namespace GlassGo.API.Tracking.Application.Internal.QueryServices;

public class DeliveryQueryService
{
    private readonly IDeliveryRepository _deliveryRepository;

    public DeliveryQueryService(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public async Task<IEnumerable<object>> Handle()
    {
        var deliveries = await _deliveryRepository.ListAsync();
        return deliveries.Select(d => new
        {
            Id = d.Id.Value,
            Status = d.Status.Value,
            Location = d.location.Value,
            Timestamp = d.timestamp.Value
        });
    }
}