namespace G_NET_84_EF03;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedDate { get; set; }
    public int InstructorId { get; set; }
    public int CategoryId { get; set; }
    public Instructor Instructor { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
