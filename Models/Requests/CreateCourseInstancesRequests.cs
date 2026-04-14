namespace modell.Models.Requests;

public struct CreateCourseInstanceRequest
{
    public int CourseId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}