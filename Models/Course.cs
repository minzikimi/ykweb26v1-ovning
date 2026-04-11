namespace modell.Models;

public class Course(int id, string title, string description)
{
    public int Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
}