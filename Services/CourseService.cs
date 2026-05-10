using modell.Models;

namespace modell.Services;

public class CourseService
{

    private static List<Course> _courses = new()
    {
        new Course(101, "C# Basic", "Learn the fundamentals of C#"),
        new Course(102, "Web API", "Build professional APIs with ASP.NET Core")
    };

    public List<Course> GetCourses() => _courses;

    public Course? GetCourse(int id)
    {
        return _courses.FirstOrDefault(c => c.Id == id);
    }

    public Course Add(Course course)
    {
        int nextId = _courses.Count > 0 ? _courses.Max(c => c.Id) + 1 : 1;
        var newCourse = new Course(nextId, course.Title, course.Description);
        _courses.Add(newCourse);
        return newCourse;
    }

    public Course? Update(int id, Course updated)
    {
        var course = _courses.FirstOrDefault(c => c.Id == id);
        if (course is null) return null;

        course.Title = updated.Title;
        course.Description = updated.Description;
        return course;
    }

    public bool Delete(int id)
    {
        var course = _courses.FirstOrDefault(c => c.Id == id);
        if (course is null) return false;

        _courses.Remove(course);
        return true;
    }
}