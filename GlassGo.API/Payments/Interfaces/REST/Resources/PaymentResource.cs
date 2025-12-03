using System;

namespace GlassGo.API.Payments.Interfaces.REST.Resources;

public class PaymentResource
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "PEN";
    public string Status { get; set; } = "Pending";
    public DateTime PaidAt { get; set; }
}