using GlassGo.API.Payments.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.Payments.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class PaymentsModelBuilderExtensions
{
    public static void ApplyPaymentsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Payment>().ToTable("Payments");
        builder.Entity<Payment>().HasKey(p => p.Id);
        builder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(10,2)");

        builder.Entity<Subscription>().ToTable("Subscriptions");
        builder.Entity<Subscription>().HasKey(s => s.Id);
    }
}