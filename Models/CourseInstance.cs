using modell.Models;

public class CourseInstance
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Course Course { get; set; }
    public List<Student> Students { get; set; } = new();
}