using System.Net.Mime;
using GlassGo.API.ServicePlanning.Domain.Model.Commands;
using GlassGo.API.ServicePlanning.Domain.Model.Queries;
using GlassGo.API.ServicePlanning.Domain.Services;
using GlassGo.API.ServicePlanning.Interfaces.REST.Resources;
using GlassGo.API.ServicePlanning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlassGo.API.ServicePlanning.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class OrdersController(
    IOrderCommandService orderCommandService,
    IOrderQueryService orderQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new order",
        Description = "Create a new service order",
        OperationId = "CreateOrder")]
    [SwaggerResponse(201, "The order was created", typeof(OrderResource))]
    [SwaggerResponse(400, "The order was not created")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderResource resource)
    {
        var createOrderCommand = CreateOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
        var order = await orderCommandService.Handle(createOrderCommand);
        
        if (order is null)
            return BadRequest();
        
        var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
        return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, orderResource);
    }

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    [SwaggerOperation(
        Summary = "Get all orders (Admin only)",
        Description = "Get all service orders",
        OperationId = "GetAllOrders")]
    [SwaggerResponse(200, "The orders were retrieved", typeof(IEnumerable<OrderResource>))]
    public async Task<IActionResult> GetAllOrders()
    {
        var getAllOrdersQuery = new GetAllOrdersQuery();
        var orders = await orderQueryService.Handle(getAllOrdersQuery);
        var orderResources = orders.Select(OrderResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(orderResources);
    }

    [HttpGet("{orderId:int}")]
    [SwaggerOperation(
        Summary = "Get order by id",
        Description = "Get a service order by its id",
        OperationId = "GetOrderById")]
    [SwaggerResponse(200, "The order was retrieved", typeof(OrderResource))]
    [SwaggerResponse(404, "The order was not found")]
    public async Task<IActionResult> GetOrderById(int orderId)
    {
        var getOrderByIdQuery = new GetOrderByIdQuery(orderId);
        var order = await orderQueryService.Handle(getOrderByIdQuery);
        
        if (order is null)
            return NotFound();
        
        var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order);
        return Ok(orderResource);
    }
    
    [HttpPatch("{orderId:int}")]
    [Authorize(Policy = "AdminOnly")]
    [SwaggerOperation(
        Summary = "Updates an order's status (Admin only)",
        Description = "Updates an order's status by order id.",
        OperationId = "UpdateOrderStatus")]
    [SwaggerResponse(200, "The order status was updated", typeof(OrderResource))]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusResource resource)
    {
        var command = new UpdateOrderStatusCommand(orderId, resource.Status);
        await orderCommandService.Handle(command);
        
        var order = await orderQueryService.Handle(new GetOrderByIdQuery(orderId));
        var orderResource = OrderResourceFromEntityAssembler.ToResourceFromEntity(order!);
        return Ok(orderResource);
    }
}