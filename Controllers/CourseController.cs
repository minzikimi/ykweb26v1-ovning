using Microsoft.AspNetCore.Mvc;

using modell.Models;
using modell.Requests;

namespace modell.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Courseontroller : ControllerBase
{

    private static List<Course> courses = new()
    {
        new Course(101, "C# Basic", "Learn the fundamentals of C#"),
        new Course(102, "Web API", "Build professional APIs with ASP.NET Core")
    };


    [HttpGet]
    public ActionResult<List<Course>> GetCourses()
    {
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public ActionResult<Course> GetCourse(int id)
    {
        var course = courses.FirstOrDefault(c => c.Id == id);
        return course is null ? NotFound() : Ok(course);
    }
    [HttpPost]
    public ActionResult<Course> Create(CreateCourseRequest req)
    {
        try
        {
            int nextId = courses.Count > 0 ? courses.Max(c => c.Id) + 1 : 101;

            var created = new Course(nextId, req.Title, req.Description);
            courses.Add(created);

            return CreatedAtAction(nameof(GetCourse), new { id = nextId }, created);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public ActionResult<Course> Update(int id, CreateCourseRequest req)
    {
        var modifyingCourse = courses.FirstOrDefault(c => c.Id == id);

        if (modifyingCourse is null)
        {
            return NotFound($"Course with ID {id} not found.");
        }

        modifyingCourse.Title = req.Title;
        modifyingCourse.Description = req.Description;

        return Ok(modifyingCourse);
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var targetCourse = courses.FirstOrDefault(c => c.Id == id);

        if (targetCourse is null)
        {
            return NotFound($"Course {id} not found.");
        }

        courses.Remove(targetCourse);
        return NoContent();
    }
}