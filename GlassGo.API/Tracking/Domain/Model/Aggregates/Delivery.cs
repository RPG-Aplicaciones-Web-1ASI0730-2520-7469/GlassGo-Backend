using GlassGo.API.Tracking.Domain.Model.Commands;
namespace GlassGo.API.Tracking.Domain.Model.Aggregates;
using GlassGo.API.Tracking.Domain.Model.ValueObjects;


public partial class Delivery
{
    public Delivery()
    {

        Id = new DeliveryId();
        Status = new DeliveryStatus();
        location = new DeliveryLocation();
        timestamp = new DeliveryTimestamp();
        
    }
    
    public Delivery(string _id, string _status, string _location, DateTime _timestamp)
    {
        Id = new DeliveryId(_id);
        Status = new DeliveryStatus(_status);
        location = new DeliveryLocation(_location);
        timestamp = new DeliveryTimestamp(_timestamp);
    }
    
    public Delivery(CreateDeliveryCommand command)
    {
        Id = new DeliveryId(command.Id);
        Status = new DeliveryStatus(command.Status);
        location = new DeliveryLocation(command.Location);
        timestamp = new DeliveryTimestamp(command.Timestamp);
    }
    
    public DeliveryId Id { get; }
    public DeliveryStatus Status { get; }
    public DeliveryLocation location { get; }
    public DeliveryTimestamp timestamp { get; }
}
