using Microsoft.AspNetCore.Mvc;
using modell.Models;
using modell.Requests;

namespace modell.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseInstanceController : ControllerBase
{
    private static List<CourseInstance> courseInstances = new()
    {
        new CourseInstance(
            1,
            new DateTime(2026, 1, 15),
            new DateTime(2026, 3, 20),
            new Course(101, "C# Basic", "Intro"),
            new List<Student>()
        )
    };

    private static List<Course> courses = new()
    {
        new Course(101, "C# Basic", "Intro"),
        new Course(102, "Web API", "Advanced")
    };


    [HttpGet]
    public ActionResult<List<CourseInstance>> GetInstances()
    {
        return Ok(courseInstances);
    }


    [HttpGet("{id}")]
    public ActionResult<CourseInstance> GetInstance(int id)
    {
        var instance = courseInstances.FirstOrDefault(ci => ci.Id == id);
        return instance is null ? NotFound($"cant find {id}") : Ok(instance);
    }

    [HttpPost]
    public ActionResult<CourseInstance> Create(CreateCourseInstanceRequest req)
    {
        var foundCourse = courses.FirstOrDefault(c => c.Id == req.CourseId);
        if (foundCourse is null) return BadRequest("cant find the course id");

        int nextId = courseInstances.Count > 0 ? courseInstances.Max(ci => ci.Id) + 1 : 1;

        var newInstance = new CourseInstance(
            nextId,
            req.StartDate,
            req.EndDate,
            foundCourse,
            new List<Student>()
        );

        courseInstances.Add(newInstance);

        return CreatedAtAction(nameof(GetInstance), new { id = nextId }, newInstance);
    }


    [HttpPut("{id}")]
    public ActionResult<CourseInstance> Update(int id, CreateCourseInstanceRequest req)
    {
        var instance = courseInstances.FirstOrDefault(ci => ci.Id == id);
        if (instance is null) return NotFound();


        if (req.EndDate <= req.StartDate) return BadRequest("End date must be after start date");


        var foundCourse = courses.FirstOrDefault(c => c.Id == req.CourseId);
        if (foundCourse is null) return BadRequest("Course not found");


        instance.StartDate = req.StartDate;
        instance.EndDate = req.EndDate;
        instance.Course = foundCourse;

        return Ok(instance);
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var instance = courseInstances.FirstOrDefault(ci => ci.Id == id);
        if (instance is null) return NotFound();

        courseInstances.Remove(instance);
        return NoContent();
    }
}