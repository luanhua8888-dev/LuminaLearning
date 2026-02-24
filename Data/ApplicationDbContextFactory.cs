using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lumina_Learning.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Hardcode connection string for design-time (migrations)
        optionsBuilder.UseNpgsql(
            "postgresql://postgres.jbvtftlooctnfjkkosms:Test123456zaZ2122@aws-0-ap-southeast-1.pooler.supabase.com:6543/postgres?sslmode=require",
            npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
                npgsqlOptions.CommandTimeout(30);
            });

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
