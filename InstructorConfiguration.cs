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
    }
}
