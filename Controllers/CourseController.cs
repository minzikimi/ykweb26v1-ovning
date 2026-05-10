using Microsoft.AspNetCore.Mvc;
using modell.Models;
using modell.Requests;
using modell.Services;

namespace modell.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;

    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public ActionResult<List<Course>> GetCourses()
    {
        var courses = _courseService.GetCourses();
        return Ok(_courseService.GetCourses());
    }

    [HttpGet("{id}")]
    public ActionResult<Course> GetCourse(int id)
    {
        var course = _courseService.GetCourse(id);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpPost]
    public ActionResult<Course> Create(CreateCourseRequest req)
    {
        var newCourse = new Course(0, req.Title, req.Description);
        var created = _courseService.Add(newCourse);

        return CreatedAtAction(nameof(GetCourse), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<Course> Update(int id, CreateCourseRequest req)
    {
        var updatedData = new Course(0, req.Title, req.Description);
        var result = _courseService.Update(id, updatedData);

        if (result is null)
        {
            return NotFound($"Course with ID {id} not found.");
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var success = _courseService.Delete(id);

        if (!success)
        {
            return NotFound($"Course {id} not found.");
        }

        return NoContent();
    }
}