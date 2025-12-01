namespace GlassGo.API.IAM.Domain.Model.ValueObjects;

/// <summary>
/// Value object representing a payment method.
/// </summary>
public record PaymentMethod
{
    public string Type { get; init; } = string.Empty;
    public string Bank { get; init; } = string.Empty;
    public string Account { get; init; } = string.Empty;

    public PaymentMethod() { }

    public PaymentMethod(string type, string bank, string account)
    {
        Type = type;
        Bank = bank;
        Account = account;
    }
}
