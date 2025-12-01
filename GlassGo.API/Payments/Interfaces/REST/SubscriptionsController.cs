using System.Net.Mime;
using GlassGo.API.Payments.Domain.Model.Queries;
using GlassGo.API.Payments.Domain.Services;
using GlassGo.API.Payments.Interfaces.REST.Resources;
using GlassGo.API.Payments.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using GlassGo.API.Payments.Domain.Model.Commands;

namespace GlassGo.API.Payments.Interfaces.REST;

[Authorize(Policy = "AdminOnly")]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Subscription endpoints for Admins")]
public class SubscriptionsController(ISubscriptionQueryService subscriptionQueryService, ISubscriptionCommandService subscriptionCommandService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all subscriptions",
        Description = "Get all subscriptions",
        OperationId = "GetAllSubscriptions")]
    [SwaggerResponse(200, "The subscriptions were retrieved", typeof(IEnumerable<SubscriptionResource>))]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        var getAllSubscriptionsQuery = new GetAllSubscriptionsQuery();
        var subscriptions = await subscriptionQueryService.Handle(getAllSubscriptionsQuery);
        var subscriptionResources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(subscriptionResources);
    }
    
    [HttpPatch("{subscriptionId:int}")]
    [SwaggerOperation(
        Summary = "Cancels a subscription",
        Description = "Cancels a subscription by subscription id.",
        OperationId = "CancelSubscription")]
    [SwaggerResponse(200, "The subscription was cancelled", typeof(SubscriptionResource))]
    public async Task<IActionResult> CancelSubscription(int subscriptionId)
    {
        var command = new CancelSubscriptionAsAdminCommand(subscriptionId);
        await subscriptionCommandService.Handle(command);
        
        var subscription = await subscriptionQueryService.Handle(new GetSubscriptionByIdQuery(subscriptionId));
        var subscriptionResource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription!);
        return Ok(subscriptionResource);
    }
}