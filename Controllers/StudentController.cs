using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using modell.Models;
using modell.Requests;
using modell.Services;

namespace modell.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentController(StudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public ActionResult<List<Student>> GetStudents()
    {
        var students = _studentService.GetStudents();
        return Ok(students);
    }

    // GET /api/student/{id}
    [HttpGet("{id}")]
    public ActionResult<Student> GetStudent(int id)
    {

    }
    // POST /api/student
    [HttpPost]
    public ActionResult<Student> Create(CreateStudentRequest req)
    {

    }

    // PUT /api/student/{id}
    [HttpPut("{id}")]
    public ActionResult<Student> Update(int id, CreateStudentRequest req)
    {

    }

    // DELETE /api/student/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) // 삭제는 보통 반환 데이터가 없어서 IActionResult 사용
    {
        var success = _studentService.Delete(id);
        return success ? NoContent() : NotFound($"Student {id} not found.");
    }
}