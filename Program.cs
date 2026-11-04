using Microsoft.EntityFrameworkCore;

namespace G_NET_84_EF03;

class Program
{
    static void Main(string[] args)
    {
        using var context = CreateContext();
        context.Database.Migrate();
        SeedStudentsAndEnrollments(context);
        AddInstructor(context);
        AddCourse(context);
        AddSeveralStudents(context);
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

    private static void AddInstructor(LearningPlatformContext context)
    {
        const string email = "new.instructor@learn.example";
        var existingInstructor = context.Instructors.FirstOrDefault(instructor => instructor.Email == email);
        if (existingInstructor is not null)
        {
            Console.WriteLine($"Instructor already exists with Id {existingInstructor.Id}.");
            return;
        }

        var instructor = new Instructor
        {
            FullName = "Hana Ibrahim",
            Email = email,
            Bio = "New platform instructor.",
            DateJoined = DateTime.UtcNow
        };

        context.Instructors.Add(instructor);
        context.SaveChanges();
        Console.WriteLine($"Created instructor with Id {instructor.Id}.");
    }

    private static void AddCourse(LearningPlatformContext context)
    {
        var instructor = context.Instructors.FirstOrDefault(item => item.Email == "new.instructor@learn.example");
        var category = context.Categories.Find(3);

        if (instructor is null || category is null)
        {
            Console.WriteLine("Course was not added because the instructor or category does not exist.");
            return;
        }

        const string title = "Entity Framework Core Essentials";
        var existingCourse = context.Courses.FirstOrDefault(course => course.Title == title);
        if (existingCourse is not null)
        {
            Console.WriteLine($"Course already exists with Id {existingCourse.Id}.");
            return;
        }

        var course = new Course
        {
            Title = title,
            Description = "EF Core code-first course.",
            Price = 130m,
            Level = CourseLevel.Intermediate,
            IsPublished = true,
            PublishedDate = DateTime.UtcNow,
            InstructorId = instructor.Id,
            CategoryId = category.Id
        };

        context.Courses.Add(course);
        context.SaveChanges();
        Console.WriteLine($"Created course with Id {course.Id}.");
    }

    private static void AddSeveralStudents(LearningPlatformContext context)
    {
        var emails = new[] { "student.one@learn.example", "student.two@learn.example", "student.three@learn.example" };
        var existingStudents = context.Students.Where(student => emails.Contains(student.Email)).ToList();
        if (existingStudents.Count > 0)
        {
            Console.WriteLine($"Students already exist with Ids: {string.Join(", ", existingStudents.Select(student => student.Id))}.");
            return;
        }

        var students = new[]
        {
            new Student { FullName = "Salma Nabil", Email = emails[0], DateOfBirth = new DateTime(2000, 6, 1) },
            new Student { FullName = "Tarek Fathy", Email = emails[1], DateOfBirth = new DateTime(2001, 7, 2) },
            new Student { FullName = "Mariam Eid", Email = emails[2], DateOfBirth = new DateTime(2002, 8, 3) }
        };

        context.Students.AddRange(students);
        context.SaveChanges();
        Console.WriteLine($"Created students with Ids: {string.Join(", ", students.Select(student => student.Id))}.");
    }
}
