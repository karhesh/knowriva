using Knowriva.Api.ExceptionHandling;
using Knowriva.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();
