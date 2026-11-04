using Microsoft.EntityFrameworkCore;

namespace G_NET_84_EF03;

public class LearningPlatformContext(DbContextOptions<LearningPlatformContext> options) : DbContext(options)
{
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearningPlatformContext).Assembly);
    }
}
