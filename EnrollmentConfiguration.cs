using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET_84_EF03;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(enrollment => new { enrollment.StudentId, enrollment.CourseId });
        builder.Property(enrollment => enrollment.EnrollmentDate).IsRequired();
        builder.Property(enrollment => enrollment.ProgressPercentage).HasPrecision(5, 2);
        builder.Property(enrollment => enrollment.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(enrollment => enrollment.FinalGrade).HasPrecision(5, 2);
        builder.ToTable(table => table.HasCheckConstraint(
            "CK_Enrollments_ProgressPercentage",
            "[ProgressPercentage] >= 0 AND [ProgressPercentage] <= 100"));

        builder.HasOne(enrollment => enrollment.Student)
            .WithMany(student => student.Enrollments)
            .HasForeignKey(enrollment => enrollment.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(enrollment => enrollment.Course)
            .WithMany(course => course.Enrollments)
            .HasForeignKey(enrollment => enrollment.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
