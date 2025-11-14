namespace GlassGo.API.Payments.Domain.Model.Commands;

public record CreatePaymentCommand(
    int UserId,
    decimal Amount,
    string Currency
);