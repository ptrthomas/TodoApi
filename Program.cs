using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add authentication with Bearer tokens
builder.Services.AddAuthentication().AddBearerToken();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

// In-memory storage
var todos = new List<TodoItem>();
var nextId = 1;

// Hardcoded credentials for the workshop
const string Username = "admin";
const string Password = "password123";

// POST /login - Login to get bearer token
app.MapPost("/login", (LoginRequest request) =>
{
    // Simple hardcoded credential check
    if (request.Username != Username || request.Password != Password)
    {
        return Results.Unauthorized();
    }

    // Create claims for the user
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, request.Username)
    };

    var claimsIdentity = new ClaimsIdentity(claims, BearerTokenDefaults.AuthenticationScheme);
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    // Generate bearer token (valid for 1 hour)
    return Results.SignIn(claimsPrincipal, authenticationScheme: BearerTokenDefaults.AuthenticationScheme);
});

// GET /reset - Reset all data
app.MapGet("/reset", () =>
{
    todos.Clear();
    nextId = 1;
    return Results.Ok(new { message = "All data cleared" });
});

// GET /todos - Get all todos
app.MapGet("/todos", () => todos);

// GET /todos/{id} - Get a specific todo
app.MapGet("/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

// POST /todos - Create a new todo
app.MapPost("/todos", (CreateTodoRequest request) =>
{
    var todo = new TodoItem
    {
        Id = nextId++,
        Title = request.Title,
        IsComplete = false
    };
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

// PUT /todos/{id} - Update a todo
app.MapPut("/todos/{id}", (int id, UpdateTodoRequest request) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo is null)
        return Results.NotFound();

    todo.Title = request.Title;
    todo.IsComplete = request.IsComplete;
    return Results.Ok(todo);
});

// DELETE /todos/{id} - Delete a todo (requires authentication)
app.MapDelete("/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo is null)
        return Results.NotFound();

    todos.Remove(todo);
    return Results.Ok();
}).RequireAuthorization();

app.Run();

// Models
record TodoItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool IsComplete { get; set; }
}

record CreateTodoRequest(string Title);
record UpdateTodoRequest(string Title, bool IsComplete);
record LoginRequest(string Username, string Password);
