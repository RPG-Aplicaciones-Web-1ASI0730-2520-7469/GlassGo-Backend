using System.Net.Mime;
using GlassGo.API.ServicePlanning.Domain.Model.Queries;
using GlassGo.API.ServicePlanning.Domain.Services;
using GlassGo.API.ServicePlanning.Interfaces.REST.Resources;
using GlassGo.API.ServicePlanning.Interfaces.REST.Transform;
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
    [SwaggerOperation(
        Summary = "Get all orders",
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
}

