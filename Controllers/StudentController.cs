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

    // get post put delete
}