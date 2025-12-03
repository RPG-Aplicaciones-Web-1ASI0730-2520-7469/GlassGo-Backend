using System.Net.Mime;
using GlassGo.API.Payments.Domain.Model.Commands;
using GlassGo.API.Payments.Domain.Model.Queries;
using GlassGo.API.Payments.Domain.Services;
using GlassGo.API.Payments.Interfaces.REST.Resources;
using GlassGo.API.Payments.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlassGo.API.Payments.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Payments")]
public class PaymentsController(
    IPaymentCommandService paymentCommandService,
    IPaymentQueryService paymentQueryService,
    ISubscriptionQueryService subscriptionQueryService,
    ISubscriptionCommandService subscriptionCommandService) : ControllerBase
{
    // --- Endpoints for Payments ---

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    [SwaggerOperation(
        Summary = "Get all payments (Admin only)",
        Description = "Get all payments",
        OperationId = "GetAllPayments")]
    [SwaggerResponse(200, "The payments were retrieved", typeof(IEnumerable<PaymentResource>))]
    public async Task<IActionResult> GetAllPayments()
    {
        var getAllPaymentsQuery = new GetAllPaymentsQuery();
        var payments = await paymentQueryService.Handle(getAllPaymentsQuery);
        var paymentResources = payments.Select(PaymentResourceFromEntityAssembler.ToResource);
        return Ok(paymentResources);
    }

    [HttpGet("user/{userId:int}")]
    [SwaggerOperation(
        Summary = "Get payments for a specific user",
        Description = "Get payments for a specific user",
        OperationId = "GetPaymentsByUserId")]
    [SwaggerResponse(200, "The payments were retrieved", typeof(IEnumerable<PaymentResource>))]
    public async Task<IActionResult> GetPaymentsByUserId(int userId)
    {
        var getPaymentsByUserIdQuery = new GetPaymentsByUserIdQuery(userId);
        var payments = await paymentQueryService.Handle(getPaymentsByUserIdQuery);
        var paymentResources = payments.Select(PaymentResourceFromEntityAssembler.ToResource);
        return Ok(paymentResources);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new payment",
        Description = "Create a new payment",
        OperationId = "CreatePayment")]
    [SwaggerResponse(201, "The payment was created", typeof(PaymentResource))]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentResource resource)
    {
        var createPaymentCommand = CreatePaymentCommandFromResourceAssembler.ToCommand(resource);
        await paymentCommandService.Handle(createPaymentCommand);
        return Created();
    }

    // --- Endpoints for Subscriptions ---

    [HttpGet("subscriptions")]
    [Authorize(Policy = "AdminOnly")]
    [SwaggerOperation(
        Summary = "Get all subscriptions (Admin only)",
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
    
    [HttpPatch("subscriptions/{subscriptionId:int}")]
    [Authorize(Policy = "AdminOnly")]
    [SwaggerOperation(
        Summary = "Cancels a subscription (Admin only)",
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