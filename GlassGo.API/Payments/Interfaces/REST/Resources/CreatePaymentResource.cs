namespace GlassGo.API.Payments.Interfaces.REST.Resources;

public class CreatePaymentResource
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "PEN";
}