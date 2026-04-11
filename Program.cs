using modell.Models;
using modell.Models.Requests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


//make mock data
var students = new List<Student>
{
    new Student {Id=123, Name= "minji", Email="minji@gmail.com"},
    new Student {Id=456, Name="somin", Email="somin@gmail.com"}
};

var courses = new List<Course> {
    new Course { Id = 101, Title = "C# Programming", Description = "Learn Backend" },
    new Course { Id = 102, Title = "Web Design", Description = "HTML/CSS basics" }
};

var courseInstances = new List<CourseInstance>
{
    new CourseInstance
    {
        Id = 1,
        Course = courses[0],
        Students = new List<Student> { students[0], students[1] },
        StartDate = new DateTime(2026, 4, 1),
        EndDate = new DateTime(2026, 6, 30)
    },
    new CourseInstance
    {
        Id=2,
        Course = courses[1],
        Students = new List<Student>{students[0]},
        StartDate = new DateTime(2026, 5, 1),
        EndDate = new DateTime(2026, 8, 1)
    }
};


app.MapGet("/hello", () => "Hello World!");

app.MapGet("/courses", () => courses);
app.MapGet("/courseInstances", () => courseInstances);

// Extrauppgift: Skapa en endpoint som returnerar en specific kurs baserat på kursens id.

app.MapGet("/courses/{id}", (int id) =>
{
    var course = courses.FirstOrDefault(c => c.Id == id);
    if (course == null)
    {
        return Results.NotFound("cant find");
    }
    return Results.Ok(course);
});

// Extrauppgift: Skapa en ny endpoint som returnerar alla kurser som en given student går
// på.

app.MapGet("/students/{studentID}/courses", (int studentID) =>
{
    var studentCourses = courseInstances
    .Where(ci => ci.Students.Any(s => s.Id == studentID))
    .Select(i => i.Course);
    return studentCourses;
});

// Extrauppgift: Skapa en ny endpoint som returnerar alla kurser mellan två givna datum.
app.MapGet("/courseInstances/between", (DateTime start, DateTime end) =>
{
    // 1. 변수 이름을 courseInstances로 일치시킴
    var filtered = courseInstances
        .Where(i => i.StartDate >= start && i.EndDate <= end);

    return Results.Ok(filtered);
});


// --- Övning 2: Student CRUD ---

app.MapGet("/students", () => Results.Ok(students));

app.MapGet("/students/{id}", (int id) =>
{
    try
    {
        var s = students.FirstOrDefault(x => x.Id == id);
        return s is null ? Results.NotFound() : Results.Ok(s);
    }
    catch { return Results.InternalServerError(); }
});

app.MapPost("/students", (CreateStudentRequest req) =>
{
    try
    {
        if (string.IsNullOrEmpty(req.Name)) return Results.BadRequest("Invalid Data");
        var newStudent = new Student
        {
            Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1,
            Name = req.Name,
            Email = req.Email
        };
        students.Add(newStudent);
        return Results.Created($"/students/{newStudent.Id}", newStudent);
    }
    catch { return Results.InternalServerError(); }
});

app.MapPut("/students/{id}", (int id, CreateStudentRequest req) =>
{
    var s = students.FirstOrDefault(x => x.Id == id);
    if (s is null) return Results.NotFound();
    s.Name = req.Name; s.Email = req.Email;
    return Results.Ok(s);
});

app.MapDelete("/students/{id}", (int id) =>
{
    var s = students.FirstOrDefault(x => x.Id == id);
    if (s is null) return Results.NotFound();
    students.Remove(s);
    return Results.NoContent();
});

app.Run();






