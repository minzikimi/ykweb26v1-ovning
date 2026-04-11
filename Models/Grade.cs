namespace modell.Models;

public class Grade(int id, string value, Student student, CourseInstance courseInstance)
{
    public int Id { get; set; } = id;
    public string Value { get; set; } = value;
    public Student Student { get; set; } = student;
    public CourseInstance CourseInstance { get; set; } = courseInstance;
}