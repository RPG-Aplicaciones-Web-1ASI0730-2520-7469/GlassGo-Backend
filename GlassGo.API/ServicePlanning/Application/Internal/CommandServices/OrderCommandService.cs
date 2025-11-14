using GlassGo.API.ServicePlanning.Domain.Model.Aggregates;
using GlassGo.API.ServicePlanning.Domain.Model.Commands;
using GlassGo.API.ServicePlanning.Domain.Repositories;
using GlassGo.API.ServicePlanning.Domain.Services;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.ServicePlanning.Application.Internal.CommandServices;

public class OrderCommandService(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : IOrderCommandService
{
    public async Task<Order?> Handle(CreateOrderCommand command)
    {
        var order = new Order
        {
            CustomerName = command.CustomerName,
            CustomerEmail = command.CustomerEmail,
            CustomerPhone = command.CustomerPhone,
            ServiceType = command.ServiceType,
            Description = command.Description,
            PreferredDate = command.PreferredDate,
            Notes = command.Notes,
            Status = "Pending"
        };

        try
        {
            await orderRepository.AddAsync(order);
            await unitOfWork.CompleteAsync();
            return order;
        }
        catch (Exception)
        {
            return null;
        }
    }
}

