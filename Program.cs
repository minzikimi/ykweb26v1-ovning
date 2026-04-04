using modell.Models;

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
app.MapGet("/students", () => students);
app.MapGet("/courses", () => courses);
app.MapGet("/courseInstances", () => courseInstances);

app.Run();






