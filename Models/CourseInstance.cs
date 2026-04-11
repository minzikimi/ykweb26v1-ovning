namespace modell.Models;

public class CourseInstance(int id, DateTime startDate, DateTime endDate, Course course, List<Student> students)
{
    public int Id { get; set; } = id;
    public DateTime StartDate { get; set; } = startDate;
    public DateTime EndDate { get; set; } = endDate;
    public Course Course { get; set; } = course;
    public List<Student> Students { get; set; } = students;
}