using Microsoft.EntityFrameworkCore;

namespace G_NET_84_EF03;

class Program
{
    static void Main(string[] args)
    {
        using var context = CreateContext();
        context.Database.Migrate();
        SeedStudentsAndEnrollments(context);
        Console.WriteLine("Online Learning Platform database is ready.");
    }

    private static LearningPlatformContext CreateContext()
    {
        const string defaultConnectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=OnlineLearningPlatform;Trusted_Connection=True;TrustServerCertificate=True;";
        var connectionString = Environment.GetEnvironmentVariable("LEARNING_PLATFORM_CONNECTION")
            ?? defaultConnectionString;

        return new LearningPlatformContext(
            new DbContextOptionsBuilder<LearningPlatformContext>()
                .UseSqlServer(connectionString)
                .Options);
    }

    private static void SeedStudentsAndEnrollments(LearningPlatformContext context)
    {
        if (context.Students.Any())
        {
            return;
        }

        var students = new[]
        {
            new Student { FullName = "Ahmed Samir", Email = "ahmed@student.example", DateOfBirth = new DateTime(2000, 1, 10) },
            new Student { FullName = "Nour Mahmoud", Email = "nour@student.example", DateOfBirth = new DateTime(1999, 2, 20) },
            new Student { FullName = "Youssef Ali", Email = "youssef@student.example", DateOfBirth = new DateTime(2001, 3, 15) },
            new Student { FullName = "Laila Hassan", Email = "laila@student.example", DateOfBirth = new DateTime(1998, 4, 25) },
            new Student { FullName = "Karim Adel", Email = "karim@student.example", DateOfBirth = new DateTime(2002, 5, 5) }
        };

        context.Students.AddRange(students);
        context.SaveChanges();

        context.Enrollments.AddRange(
            new Enrollment { StudentId = students[0].Id, CourseId = 1, EnrollmentDate = DateTime.UtcNow.AddDays(-30), ProgressPercentage = 45m, Status = EnrollmentStatus.Active },
            new Enrollment { StudentId = students[0].Id, CourseId = 2, EnrollmentDate = DateTime.UtcNow.AddDays(-20), ProgressPercentage = 100m, Status = EnrollmentStatus.Completed, FinalGrade = 92m },
            new Enrollment { StudentId = students[1].Id, CourseId = 1, EnrollmentDate = DateTime.UtcNow.AddDays(-15), ProgressPercentage = 20m, Status = EnrollmentStatus.Active },
            new Enrollment { StudentId = students[1].Id, CourseId = 4, EnrollmentDate = DateTime.UtcNow.AddDays(-10), ProgressPercentage = 0m, Status = EnrollmentStatus.Dropped },
            new Enrollment { StudentId = students[2].Id, CourseId = 2, EnrollmentDate = DateTime.UtcNow.AddDays(-25), ProgressPercentage = 100m, Status = EnrollmentStatus.Completed, FinalGrade = 88m },
            new Enrollment { StudentId = students[2].Id, CourseId = 6, EnrollmentDate = DateTime.UtcNow.AddDays(-12), ProgressPercentage = 60m, Status = EnrollmentStatus.Active },
            new Enrollment { StudentId = students[3].Id, CourseId = 4, EnrollmentDate = DateTime.UtcNow.AddDays(-8), ProgressPercentage = 100m, Status = EnrollmentStatus.Completed, FinalGrade = 95m },
            new Enrollment { StudentId = students[4].Id, CourseId = 6, EnrollmentDate = DateTime.UtcNow.AddDays(-5), ProgressPercentage = 0m, Status = EnrollmentStatus.Dropped });

        context.SaveChanges();
    }
}
