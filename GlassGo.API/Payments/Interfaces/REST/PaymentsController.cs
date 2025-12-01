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
public class PaymentsController(
    IPaymentCommandService paymentCommandService,
    IPaymentQueryService paymentQueryService) : ControllerBase
{
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
        // Note: This is not ideal, as we don't have a GetPaymentById endpoint to return the created resource.
        // This is a simplification for the purpose of this exercise.
        return Created();
    }
}