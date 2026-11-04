using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace G_NET_84_EF03;

public class LearningPlatformContextFactory : IDesignTimeDbContextFactory<LearningPlatformContext>
{
    public LearningPlatformContext CreateDbContext(string[] args)
    {
        const string connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=OnlineLearningPlatform;Trusted_Connection=True;TrustServerCertificate=True;";

        return new LearningPlatformContext(
            new DbContextOptionsBuilder<LearningPlatformContext>()
                .UseSqlServer(connectionString)
                .Options);
    }
}
