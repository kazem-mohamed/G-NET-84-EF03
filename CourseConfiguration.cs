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
    }
}
