using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
/// Factory for creating AppDbContext instances at design time (for migrations).
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Use MySQL for migrations (production)
        var connectionString = "server=localhost;port=3306;user=root;password=root;database=glassgo_dev";
        
        optionsBuilder.UseMySQL(connectionString);
        
        return new AppDbContext(optionsBuilder.Options);
    }
}
