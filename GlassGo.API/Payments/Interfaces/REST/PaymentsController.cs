using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassGo.API.Payments.Domain.Model.Queries;
using GlassGo.API.Payments.Domain.Services;
using GlassGo.API.Payments.Interfaces.REST.Resources;
using GlassGo.API.Payments.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace GlassGo.API.Payments.Interfaces.REST;

[ApiController]
[Route("api/v1/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentCommandService _paymentCommandService;
    private readonly IPaymentQueryService _paymentQueryService;

    public PaymentsController(
        IPaymentCommandService paymentCommandService,
        IPaymentQueryService paymentQueryService)
    {
        _paymentCommandService = paymentCommandService;
        _paymentQueryService = paymentQueryService;
    }

    /// <summary>
    /// Get all payments.
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<PaymentResource>> GetAllAsync()
    {
        var query = new GetAllPaymentsQuery();
        var payments = await _paymentQueryService.Handle(query);
        return payments.Select(PaymentResourceFromEntityAssembler.ToResource);
    }

    /// <summary>
    /// Get payments for a specific user.
    /// </summary>
    [HttpGet("user/{userId:int}")]
    public async Task<IEnumerable<PaymentResource>> GetByUserIdAsync(int userId)
    {
        var query = new GetPaymentsByUserIdQuery(userId);
        var payments = await _paymentQueryService.Handle(query);
        return payments.Select(PaymentResourceFromEntityAssembler.ToResource);
    }

    /// <summary>
    /// Create a new payment.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PaymentResource>> PostAsync([FromBody] CreatePaymentResource resource)
    {
        var command = CreatePaymentCommandFromResourceAssembler.ToCommand(resource);
        var result = await _paymentCommandService.Handle(command);
        var paymentResource = PaymentResourceFromEntityAssembler.ToResource(result);

        return CreatedAtAction(nameof(GetAllAsync), new { id = paymentResource.Id }, paymentResource);
    }
}
