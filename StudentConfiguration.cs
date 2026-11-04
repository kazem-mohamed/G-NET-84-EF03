using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET_84_EF03;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.Property(student => student.FullName).IsRequired().HasMaxLength(150);
        builder.Property(student => student.Email).IsRequired().HasMaxLength(200);
        builder.Property(student => student.DateOfBirth).IsRequired();
    }
}
