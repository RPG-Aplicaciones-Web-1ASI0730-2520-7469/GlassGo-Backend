namespace GlassGo.API.ServicePlanning.Domain.Model.Commands;

public record UpdateOrderStatusCommand(int OrderId, string Status);