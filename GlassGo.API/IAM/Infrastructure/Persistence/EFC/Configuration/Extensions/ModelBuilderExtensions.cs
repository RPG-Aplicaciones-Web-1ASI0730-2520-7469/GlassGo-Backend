using GlassGo.API.IAM.Domain.Model.Aggregates;
using GlassGo.API.IAM.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        // IAM Context - User Entity Configuration
        
        builder.Entity<User>().ToTable("Users");
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(100);
        builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
        builder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Entity<User>().Property(u => u.Role).IsRequired().HasMaxLength(50);
        builder.Entity<User>().Property(u => u.Company).HasMaxLength(255);
        builder.Entity<User>().Property(u => u.BusinessName).HasMaxLength(255);
        builder.Entity<User>().Property(u => u.TaxId).HasMaxLength(50);
        builder.Entity<User>().Property(u => u.Address).HasMaxLength(500);
        builder.Entity<User>().Property(u => u.Phone).IsRequired().HasMaxLength(20);
        builder.Entity<User>().Property(u => u.PreferredCurrency).HasMaxLength(3).HasDefaultValue("PEN");
        builder.Entity<User>().Property(u => u.LoyaltyPoints).HasDefaultValue(0);
        builder.Entity<User>().Property(u => u.IsActive).HasDefaultValue(true);
        builder.Entity<User>().Property(u => u.CreatedAt).IsRequired();
        
        // NotificationSettings as owned entity (Complex Type)
        builder.Entity<User>().OwnsOne(u => u.Notifications, n =>
        {
            n.Property(ns => ns.Email).HasColumnName("NotificationEmail").HasDefaultValue(true);
            n.Property(ns => ns.Sms).HasColumnName("NotificationSms").HasDefaultValue(false);
            n.Property(ns => ns.Push).HasColumnName("NotificationPush").HasDefaultValue(true);
        });
        
        // PaymentMethods as owned collection (Complex Type) - TEMPORARILY DISABLED
        // TODO: Re-enable after database schema is fixed
        /*
        builder.Entity<User>().OwnsMany(u => u.PaymentMethods, pm =>
        {
            pm.WithOwner().HasForeignKey("UserId");
            pm.Property<int>("Id").ValueGeneratedOnAdd();
            pm.HasKey("Id");
            pm.Property(p => p.Type).HasMaxLength(50);
            pm.Property(p => p.Bank).HasMaxLength(100);
            pm.Property(p => p.Account).HasMaxLength(100);
        });
        */
        
        // Indexes
        builder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}
