using GlassGo.API.Tracking.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.Tracking.Infrastructure.Persistence.EFC.Contexts;

public class TrackingDbContext : DbContext
{
    public TrackingDbContext(DbContextOptions<TrackingDbContext> options) : base(options) { }

    public DbSet<Delivery> Deliveries { get; set; } = null!;
}