using GlassGo.API.Tracking.Application.Internal.CommandServices;
using GlassGo.API.Tracking.Application.Internal.QueryServices;
using GlassGo.API.Tracking.Domain.Model.Commands;
using Microsoft.AspNetCore.Mvc;

namespace GlassGo.API.Tracking.Interfaces.REST;

[ApiController]
[Route("api/v1/deliveries")]
public class DeliveriesController : ControllerBase
{
    private readonly DeliveryCommandService _commandService;
    private readonly DeliveryQueryService _queryService;

    public DeliveriesController(DeliveryCommandService commandService, DeliveryQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _queryService.Handle();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDeliveryCommand command)
    {
        var delivery = await _commandService.Handle(command);
        return CreatedAtAction(nameof(GetAll), new { id = delivery.Id.Value }, delivery);
    }
}