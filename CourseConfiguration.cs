using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET_84_EF03;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(course => course.Title).IsRequired().HasMaxLength(200);
        builder.Property(course => course.Description).HasMaxLength(2_000);
        builder.Property(course => course.Price).HasPrecision(18, 2);
        builder.Property(course => course.Level).HasConversion<string>().HasMaxLength(20);

        builder.HasOne(course => course.Instructor)
            .WithMany(instructor => instructor.Courses)
            .HasForeignKey(course => course.InstructorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(course => course.Category)
            .WithMany(category => category.Courses)
            .HasForeignKey(course => course.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Course { Id = 1, Title = "C# Fundamentals", Description = "C# for beginners", Price = 50m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 4, 1), InstructorId = 1, CategoryId = 1 },
            new Course { Id = 2, Title = "ASP.NET Core APIs", Description = "Build REST APIs", Price = 120m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedDate = new DateTime(2024, 5, 1), InstructorId = 1, CategoryId = 3 },
            new Course { Id = 3, Title = "Clean Architecture", Description = "Application architecture", Price = 150m, Level = CourseLevel.Advanced, IsPublished = false, PublishedDate = null, InstructorId = 1, CategoryId = 2 },
            new Course { Id = 4, Title = "Modern JavaScript", Description = "Modern frontend basics", Price = 70m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 6, 1), InstructorId = 2, CategoryId = 4 },
            new Course { Id = 5, Title = "React Patterns", Description = "Reusable React patterns", Price = 110m, Level = CourseLevel.Intermediate, IsPublished = false, PublishedDate = null, InstructorId = 2, CategoryId = 4 },
            new Course { Id = 6, Title = "UI Design Systems", Description = "Design systems", Price = 95m, Level = CourseLevel.Advanced, IsPublished = true, PublishedDate = new DateTime(2024, 7, 1), InstructorId = 3, CategoryId = 5 });
    }
}
