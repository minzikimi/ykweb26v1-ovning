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
    new Student(123, "minji", "minji@gmail.com"),
    new Student(456, "somin", "somin@gmail.com")
};
var courses = new List<Course> {
    new Course(101, "C# Programming", "Learn Backend"),
    new Course(102, "Web Design", "HTML/CSS basics")
};

// var courseInstances = new List<CourseInstance>
// {
//     new CourseInstance
//     {
//         Id = 1,
//         Course = courses[0],
//         Students = new List<Student> { students[0], students[1] },
//         StartDate = new DateTime(2026, 4, 1),
//         EndDate = new DateTime(2026, 6, 30)
//     },
//     new CourseInstance
//     {
//         Id=2,
//         Course = courses[1],
//         Students = new List<Student>{students[0]},
//         StartDate = new DateTime(2026, 5, 1),
//         EndDate = new DateTime(2026, 8, 1)
//     }
// };


app.MapGet("/hello", () => "Hello World!");



// Extrauppgift: Skapa en ny endpoint som returnerar alla kurser mellan två givna datum.
// app.MapGet("/courseInstances/between", (DateTime start, DateTime end) =>
// {
//     // 1. 변수 이름을 courseInstances로 일치시킴
//     var filtered = courseInstances
//         .Where(i => i.StartDate >= start && i.EndDate <= end);

//     return Results.Ok(filtered);
// });


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
        // 2. ID 계산
        int nextId = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;

        var newStudent = new Student(nextId, req.Name, req.Email);
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


// Övning 4 – CRUD för Course

app.MapGet("/courses", () => Results.Ok(courses));
app.MapGet("/courses/{id}", (int id) =>
{
    var course = courses.FirstOrDefault(c => c.Id == id);
    return course is null ? Results.NotFound() : Results.Ok(course);
});

// 3.use struct
app.MapPost("/courses", (CreateCourseRequest req) =>
{
    try
    {
        int nextId = courses.Count > 0 ? courses.Max(c => c.Id) + 1 : 101;

        Course created = new Course(nextId, req.Title, req.Description);

        courses.Add(created);
        return Results.Created($"/courses/{nextId}", created);
    }
    catch (Exception ex)
    {
        return Results.InternalServerError(ex.Message);
    }
});
app.MapPut("/courses/{id}", (int id, CreateCourseRequest req) =>
{
    var modifyingCourse = courses.FirstOrDefault(c => c.Id == id);

    if (modifyingCourse is null)
    {
        return Results.NotFound($"Course with ID {id} not found.");
    }
    modifyingCourse.Title = req.Title;
    modifyingCourse.Description = req.Description;

    return Results.Ok(modifyingCourse);
});


app.MapDelete("/courses/{id}", (int id) =>
{
    var targetCourse = courses.FirstOrDefault(c => c.Id == id);
    if (targetCourse is null)
    {
        return Results.NotFound($"Course {id} not found.");
    }
    courses.Remove(targetCourse);
    return Results.NoContent();
});


app.Run();






