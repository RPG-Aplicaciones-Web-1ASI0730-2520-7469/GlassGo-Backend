using GlassGo.API.Tracking.Domain.Model.Aggregates;
using GlassGo.API.Tracking.Domain.Model.Commands;
using GlassGo.API.Tracking.Domain.Repositories;

namespace GlassGo.API.Tracking.Application.Internal.CommandServices;

public class DeliveryCommandService
{
    private readonly IDeliveryRepository _deliveryRepository;

    public DeliveryCommandService(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public async Task<Delivery> Handle(CreateDeliveryCommand command)
    {
        var delivery = new Delivery(command);
        await _deliveryRepository.AddAsync(delivery);
        return delivery;
    }
}