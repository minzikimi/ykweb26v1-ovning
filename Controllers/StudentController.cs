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
        return Ok(_studentService.GetStudents());
    }

    // GET /api/student/{id}
    [HttpGet("{id}")]
    public ActionResult<Student> GetStudent(int id)
    {
        var student = _studentService.GetStudent(id);


        if (student is null) return NotFound($"Student with ID {id} not found.");
        return Ok(student);
    }
    // POST /api/student
    [HttpPost]
    public ActionResult<Student> Create(CreateStudentRequest req)
    {
        // 유효성 검사 (안내데스크의 역할)
        if (string.IsNullOrEmpty(req.Name)) return BadRequest("Name is required.");

        // 서비스에게 넘겨줄 깡통 객체 생성
        var tempStudent = new Student(0, req.Name, req.Email);

        // 비서한테 "장부에 추가해" 시키고 결과물 받기
        var createdStudent = _studentService.Add(tempStudent);

        // 201 Created 상자에 포장해서 보냄 (조회할 수 있는 주소 정보도 포함)
        return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
    }


    // PUT /api/student/{id}
    [HttpPut("{id}")]
    public ActionResult<Student> Update(int id, CreateStudentRequest req)
    {
        var tempStudent = new Student(0, req.Name, req.Email);

        // 비서한테 수정 시키기
        var updatedStudent = _studentService.Update(id, tempStudent);

        // 수정할 대상이 없었으면 404, 성공했으면 200 Ok 포장
        if (updatedStudent is null) return NotFound($"Student {id} not found.");
        return Ok(updatedStudent);
    }

    // DELETE /api/student/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) // 삭제는 보통 반환 데이터가 없어서 IActionResult 사용
    {
        var success = _studentService.Delete(id);
        return success ? NoContent() : NotFound($"Student {id} not found.");
    }
}