using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET_84_EF03;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(category => category.Name).IsRequired().HasMaxLength(100);
        builder.Property(category => category.Description).HasMaxLength(500);

        builder.HasOne(category => category.ParentCategory)
            .WithMany(category => category.SubCategories)
            .HasForeignKey(category => category.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Category { Id = 1, Name = "Programming", Description = "Software development", ParentCategoryId = null },
            new Category { Id = 2, Name = "Backend", Description = "Server-side development", ParentCategoryId = 1 },
            new Category { Id = 3, Name = "ASP.NET", Description = "ASP.NET development", ParentCategoryId = 2 },
            new Category { Id = 4, Name = "Frontend", Description = "Client-side development", ParentCategoryId = 1 },
            new Category { Id = 5, Name = "Design", Description = "Visual design", ParentCategoryId = null });
    }
}
