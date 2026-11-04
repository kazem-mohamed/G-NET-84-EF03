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
        EnrollStudent(context);
        FindCourse(context);
        FilterCourses(context, 60m, 140m);
        PrintCategories(context, 1);
        PrintCourseRoster(context, 1);
        ChangeCoursePrice(context, 1, 55m);
        UpdateProgress(context, 1, 1, 100m);
        PublishCourse(context, 3);
        UnenrollStudent(context, 4, 4);
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

    private static void EnrollStudent(LearningPlatformContext context)
    {
        var student = context.Students.FirstOrDefault(item => item.Email == "student.one@learn.example");
        var course = context.Courses.FirstOrDefault(item => item.Title == "Entity Framework Core Essentials");

        if (student is null || course is null)
        {
            Console.WriteLine("Enrollment was refused because the student or course was not found.");
            return;
        }

        if (!course.IsPublished)
        {
            Console.WriteLine("Enrollment was refused because the course is not published.");
            return;
        }

        if (context.Enrollments.Find(student.Id, course.Id) is not null)
        {
            Console.WriteLine("Enrollment was refused because the student is already enrolled.");
            return;
        }

        context.Enrollments.Add(new Enrollment
        {
            StudentId = student.Id,
            CourseId = course.Id,
            EnrollmentDate = DateTime.UtcNow,
            ProgressPercentage = 0m,
            Status = EnrollmentStatus.Active
        });
        context.SaveChanges();
        Console.WriteLine($"Student {student.Id} enrolled in course {course.Id}.");
    }

    private static void FindCourse(LearningPlatformContext context)
    {
        var courseById = context.Courses.Find(1);
        Console.WriteLine(courseById is null ? "Course with Id 1: not found." : $"Course by Id: {courseById.Title}");

        var courseByTitle = context.Courses.FirstOrDefault(course => course.Title == "C# Fundamentals");
        Console.WriteLine(courseByTitle is null ? "Course by title: not found." : $"Course by title: {courseByTitle.Id}");
    }

    private static void FilterCourses(LearningPlatformContext context, decimal minimumPrice, decimal maximumPrice)
    {
        var courses = context.Courses
            .Where(course => course.Price >= minimumPrice && course.Price <= maximumPrice)
            .OrderByDescending(course => course.Price)
            .Select(course => new { course.Title, course.Level, course.Price })
            .ToList();

        foreach (var course in courses)
        {
            Console.WriteLine($"{course.Title} | {course.Level} | {course.Price:C}");
        }
    }

    private static void PrintCategories(LearningPlatformContext context, int categoryId)
    {
        var subCategories = context.Categories
            .Where(category => category.ParentCategoryId == categoryId)
            .Select(category => category.Name)
            .ToList();
        Console.WriteLine($"Direct sub-categories: {string.Join(", ", subCategories)}");

        var topLevelCategories = context.Categories
            .Where(category => category.ParentCategoryId == null)
            .Select(category => category.Name)
            .ToList();
        Console.WriteLine($"Top-level categories: {string.Join(", ", topLevelCategories)}");
    }

    private static void PrintCourseRoster(LearningPlatformContext context, int courseId)
    {
        if (context.Courses.Find(courseId) is null)
        {
            Console.WriteLine("Course roster: course not found.");
            return;
        }

        var studentIds = context.Enrollments
            .Where(enrollment => enrollment.CourseId == courseId)
            .Select(enrollment => enrollment.StudentId)
            .ToList();
        var studentNames = context.Students
            .Where(student => studentIds.Contains(student.Id))
            .Select(student => student.FullName)
            .ToList();

        Console.WriteLine($"Course {courseId} has {studentIds.Count} enrolled student(s): {string.Join(", ", studentNames)}");
    }

    private static void ChangeCoursePrice(LearningPlatformContext context, int courseId, decimal newPrice)
    {
        var course = context.Courses.Find(courseId);
        if (course is null)
        {
            Console.WriteLine("Price update: course not found.");
            return;
        }

        course.Price = newPrice;
        context.SaveChanges();
        Console.WriteLine($"Updated {course.Title} price to {course.Price:C}.");
    }

    private static void UpdateProgress(LearningPlatformContext context, int studentId, int courseId, decimal progress)
    {
        if (progress is < 0m or > 100m)
        {
            Console.WriteLine("Progress update was refused: progress must be between 0 and 100.");
            return;
        }

        var enrollment = context.Enrollments.Find(studentId, courseId);
        if (enrollment is null)
        {
            Console.WriteLine("Progress update: enrollment not found.");
            return;
        }

        enrollment.ProgressPercentage = progress;
        if (progress == 100m)
        {
            enrollment.Status = EnrollmentStatus.Completed;
        }

        context.SaveChanges();
        Console.WriteLine($"Updated progress to {progress}% with status {enrollment.Status}.");
    }

    private static void PublishCourse(LearningPlatformContext context, int courseId)
    {
        var course = context.Courses.Find(courseId);
        if (course is null)
        {
            Console.WriteLine("Publish: course not found.");
            return;
        }

        course.IsPublished = true;
        course.PublishedDate ??= DateTime.UtcNow;
        context.SaveChanges();
        Console.WriteLine($"Published {course.Title} on {course.PublishedDate:O}.");
    }

    private static void UnenrollStudent(LearningPlatformContext context, int studentId, int courseId)
    {
        var enrollment = context.Enrollments.Find(studentId, courseId);
        if (enrollment is null)
        {
            Console.WriteLine("Unenroll: enrollment not found.");
            return;
        }

        context.Enrollments.Remove(enrollment);
        context.SaveChanges();
        Console.WriteLine($"Student {studentId} was unenrolled from course {courseId}.");
    }
}
