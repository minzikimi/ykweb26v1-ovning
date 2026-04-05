namespace modell.Models;

public class Grade
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;

    public Student Student { get; set; } = new();

    public CourseInstance CourseInstance { get; set; } = new();
}