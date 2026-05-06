// Övning 4 – Flytta logik till en service
// 1. Skapa en mapp Services.
// 2. Skapa StudentService med metoderna:
// o GetAll()
// o GetById(int id)
// o Add(Student student)
// o Update(int id, Student updated)
// o Delete(int id)
// 3. Flytta all logik från StudentController till StudentService.
// 4. Ändra StudentController så att den bara anropar service-metoderna.
// Studenterna ser nu hur controllern blir tunnare.

using modell.Models;

namespace modell.Services;

public class StudentService
{
    private static List<Student> _students = new()
    {
        new Student(123, "minji", "minji@gmail.com"),
        new Student(456, "somin", "somin@gmail.com")
    };


    public List<Student> GetStudents()
    {
        return _students;
    }

    public Student? GetStudent(int id)
    {
        return _students.FirstOrDefault(x => x.Id == id);
    }

    public Student Add(Student student)
    {
        int nextId = _students.Count > 0 ? _students.Max(s => s.Id) + 1 : 1;

        var newStudent = new Student(nextId, student.Name, student.Email);
        _students.Add(newStudent);

        return newStudent;
    }


    public Student? Update(int id, Student updated)
    {
        var student = _students.FirstOrDefault(x => x.Id == id);
        if (student is null) return null;

        student.Name = updated.Name;
        student.Email = updated.Email;
        return student;
    }


    public bool Delete(int id)
    {
        var student = _students.FirstOrDefault(x => x.Id == id);
        if (student is null) return false;

        _students.Remove(student);
        return true;
    }
}