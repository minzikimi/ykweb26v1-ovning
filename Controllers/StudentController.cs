using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using modell.Models;
using modell.Requests;

namespace modell.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private static List<Student> students = new()
    {
        new Student(123, "minji", "minji@gmail.com"),
        new Student(456, "somin", "somin@gmail.com")
    };


    [HttpGet]
    public ActionResult<List<Student>> GetStudents()
    {
        return Ok(students);
    }

    // GET /api/student/{id}
    [HttpGet("{id}")]
    public ActionResult<Student> GetStudent(int id)
    {
        var s = students.FirstOrDefault(x => x.Id == id);
        return s is null ? NotFound() : Ok(s);
    }
    // POST /api/student
    [HttpPost]
    public ActionResult<Student> Create(CreateStudentRequest req)
    {
        if (string.IsNullOrEmpty(req.Name)) return BadRequest("Invalid Data");

        int nextId = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;
        var newStudent = new Student(nextId, req.Name, req.Email);
        students.Add(newStudent);

        return CreatedAtAction(nameof(GetStudent), new { id = newStudent.Id }, newStudent);
    }

    // PUT /api/student/{id}
    [HttpPut("{id}")]
    public ActionResult<Student> Update(int id, CreateStudentRequest req)
    {
        var s = students.FirstOrDefault(x => x.Id == id);
        if (s is null) return NotFound();

        s.Name = req.Name;
        s.Email = req.Email;
        return Ok(s);
    }

    // DELETE /api/student/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) // 삭제는 보통 반환 데이터가 없어서 IActionResult 사용
    {
        var s = students.FirstOrDefault(x => x.Id == id);
        if (s is null) return NotFound();

        students.Remove(s);
        return NoContent(); // 204 No Content
    }
}