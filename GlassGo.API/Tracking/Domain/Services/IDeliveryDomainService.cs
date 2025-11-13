using System.Collections.Generic;
using System.Threading.Tasks;
using GlassGo.API.Tracking.Domain.Model.Aggregates;

namespace GlassGo.API.Tracking.Domain.Services
{
    /// <summary>
    /// Define la lógica de negocio principal para la gestión de entregas.
    /// </summary>
    public interface IDeliveryDomainService
    {
        Task<IEnumerable<Delivery>> ListAllAsync();
        Task<Delivery?> FindByIdAsync(string deliveryId);
        Task<Delivery> CreateAsync(Delivery delivery);
        Task<Delivery> UpdateStatusAsync(string deliveryId, string newStatus);
        Task<Delivery> UpdateLocationAsync(string deliveryId, string newLocation);
        Task<Delivery> CompleteAsync(string deliveryId);
    }
}