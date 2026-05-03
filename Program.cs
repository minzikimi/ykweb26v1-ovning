using modell.Models;
using modell.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();


var students = new List<Student>
{
    new Student(123, "minji", "minji@gmail.com"),
    new Student(456, "somin", "somin@gmail.com")
};
var courses = new List<Course> {
    new Course(101, "C# Programming", "Learn Backend"),
    new Course(102, "Web Design", "HTML/CSS basics")
};

var courseInstances = new List<CourseInstance>
{
    // 순서: Id, StartDate, EndDate, Course, Students
    new CourseInstance(
        1,
        new DateTime(2026, 4, 1),
        new DateTime(2026, 6, 30),
        courses[0],
        new List<Student> { students[0], students[1] }
    ),

    new CourseInstance(
        2,
        new DateTime(2026, 5, 1),
        new DateTime(2026, 8, 1),
        courses[1],
        new List<Student> { students[0] }
    )
};

var validGrades = new List<string> { "A", "B", "C", "D", "E", "F" };
var gradeRecords = new List<Grade>();

app.MapGet("/hello", () => "Hello World!");



// Extrauppgift: Skapa en ny endpoint som returnerar alla kurser mellan två givna datum.
// app.MapGet("/courseInstances/between", (DateTime start, DateTime end) =>
// {
//     // 1. 변수 이름을 courseInstances로 일치시킴
//     var filtered = courseInstances
//         .Where(i => i.StartDate >= start && i.EndDate <= end);

//     return Results.Ok(filtered);
// });


// --- Övning 5: CourseInstance CRUD ---

app.MapGet("/courseInstances", () => Results.Ok(courseInstances));
app.MapGet("/courseInstances/{id}", (int id) =>
{
    var instance = courseInstances.FirstOrDefault(ci => ci.Id == id);
    return instance is null ? Results.NotFound($"cant find {id}") : Results.Ok(instance);
});

app.MapPost("/courseInstances", (CreateCourseInstanceRequest req) =>
{
    try
    {
        var foundCourse = courses.FirstOrDefault(c => c.Id == req.CourseId);
        if (foundCourse is null)

            return Results.BadRequest("cant find the course id");

        int nextId = courseInstances.Count > 0 ? courseInstances.Max(ci => ci.Id) + 1 : 1;
        var newInstance = new CourseInstance(
                    nextId,
                    req.StartDate,
                    req.EndDate,
                    foundCourse,
                    new List<Student>()
                );
        courseInstances.Add(newInstance);

        return Results.Created($"/courseinstances/{nextId}", newInstance);
    }

    catch (Exception ex)
    {
        return Results.InternalServerError($"An error occurred: {ex.Message}");
    }
});
app.MapPut("/courseInstances/{id}", (int id, CreateCourseInstanceRequest req) =>
{
    try
    {
        var instance = courseInstances.FirstOrDefault(ci => ci.Id == id);
        if (instance is null) return Results.NotFound();

        if (req.EndDate <= req.StartDate) return Results.BadRequest();

        var foundCourse = courses.FirstOrDefault(c => c.Id == req.CourseId);
        if (foundCourse is null) return Results.BadRequest();

        instance.StartDate = req.StartDate;
        instance.EndDate = req.EndDate;
        instance.Course = foundCourse;

        return Results.Ok(instance);
    }
    catch (Exception ex)
    {
        return Results.InternalServerError(ex.Message);
    }

});
app.MapDelete("/courseInstances/{id}", (int id) =>
{
    try
    {
        var instance = courseInstances.FirstOrDefault(ci => ci.Id == id);
        if (instance is null) return Results.NotFound();

        courseInstances.Remove(instance);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.InternalServerError(ex.Message);
    }
});

/* Övning 6 – (Frivillig utmaning) Hantera Grade
• Skapa en endpoint POST /grades som låter läraren sätta ett betyg för en student i ett
kurstillfälle.
• Regler:
o Om både student och kurstillfälle existerar → 201 Created.
o Om student eller kurstillfälle saknas → 404 Not Found.
o Om betyget är ogiltigt (t.ex. inte A–F) → 400 Bad Request. */

app.MapPost("/grades", (CreateGradeRequest req) =>
{
    try
    {
        var foundStudent = students.FirstOrDefault(s => s.Id == req.StudentId);
        var foundCourseInstance = courseInstances.FirstOrDefault(ci => ci.Id == req.CourseInstanceId);

        if (foundStudent is null || foundCourseInstance is null)
        {
            return Results.NotFound("Student or CourseInstance not found.");
        }

        if (string.IsNullOrEmpty(req.Grade) || !validGrades.Contains(req.Grade.ToUpper()))
        {
            return Results.BadRequest("Invalid Grade.");
        }

        int nextId = gradeRecords.Count > 0 ? gradeRecords.Max(g => g.Id) + 1 : 1;

        Grade created = new Grade(nextId, req.Grade.ToUpper(), foundStudent, foundCourseInstance);

        gradeRecords.Add(created);

        return Results.Created($"/grades/{nextId}", created);
    }
    catch (Exception ex)
    {
        return Results.InternalServerError(ex.Message);
    }
});

app.Run();






