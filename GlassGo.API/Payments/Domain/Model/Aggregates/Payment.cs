using System;

namespace GlassGo.API.Payments.Domain.Model.Aggregates;

public class Payment
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "PEN";
    public string Status { get; private set; } = "Pending"; // Pending, Completed, Failed
    public DateTime PaidAt { get; private set; }

    protected Payment() { }

    public Payment(int userId, decimal amount, string currency)
    {
        UserId = userId;
        Amount = amount;
        Currency = currency;
        Status = "Pending";
        PaidAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = "Completed";
    }

    public void MarkAsFailed()
    {
        Status = "Failed";
    }
}