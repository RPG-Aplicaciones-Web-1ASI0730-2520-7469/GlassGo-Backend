using GlassGo.API.Analytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.Analytics.Infrastructure.Data
{
    public class DashboardAnalyticsContext : DbContext
    {
        public DashboardAnalyticsContext(DbContextOptions<DashboardAnalyticsContext> options)
            : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; } = null!;
    }
}