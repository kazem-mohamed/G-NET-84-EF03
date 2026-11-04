using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET_84_EF03;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.Property(instructor => instructor.FullName).IsRequired().HasMaxLength(150);
        builder.Property(instructor => instructor.Email).IsRequired().HasMaxLength(200);
        builder.Property(instructor => instructor.Bio).HasMaxLength(1_000);
        builder.Property(instructor => instructor.DateJoined).IsRequired();

        builder.HasData(
            new Instructor { Id = 1, FullName = "Mona Hassan", Email = "mona@learn.example", Bio = "Backend instructor.", DateJoined = new DateTime(2024, 1, 10) },
            new Instructor { Id = 2, FullName = "Omar Adel", Email = "omar@learn.example", Bio = "Frontend instructor.", DateJoined = new DateTime(2024, 2, 15) },
            new Instructor { Id = 3, FullName = "Sara Ali", Email = "sara@learn.example", Bio = "Design instructor.", DateJoined = new DateTime(2024, 3, 20) });
    }
}
