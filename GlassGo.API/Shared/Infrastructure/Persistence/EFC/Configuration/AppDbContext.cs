using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using GlassGo.API.ServicePlanning.Domain.Model.Aggregates;

namespace GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    // Service Planning Bounded Context
    public DbSet<Order> Orders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Automatically set CreatedDate and UpdatedDate for entities
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Service Planning Bounded Context
        builder.Entity<Order>().HasKey(o => o.Id);
        builder.Entity<Order>().Property(o => o.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Order>().Property(o => o.CustomerName).IsRequired().HasMaxLength(100);
        builder.Entity<Order>().Property(o => o.CustomerEmail).IsRequired().HasMaxLength(150);
        builder.Entity<Order>().Property(o => o.CustomerPhone).IsRequired().HasMaxLength(20);
        builder.Entity<Order>().Property(o => o.ServiceType).IsRequired().HasMaxLength(50);
        builder.Entity<Order>().Property(o => o.Description).IsRequired().HasMaxLength(500);
        builder.Entity<Order>().Property(o => o.PreferredDate).IsRequired();
        builder.Entity<Order>().Property(o => o.Status).IsRequired().HasMaxLength(20);
        builder.Entity<Order>().Property(o => o.Notes).HasMaxLength(500);
        
        // Apply naming convention to use snake_case for database objects
        builder.UseSnakeCaseNamingConvention();
    }
}
