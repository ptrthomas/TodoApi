using System.Security.Claims;
using System.Text.Json.Serialization;
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
    return Results.Ok(new { message = "All data cleared" });
});

// GET /api/todos - Get all todos
app.MapGet("/api/todos", () => todos);

// GET /api/todos/{id} - Get a specific todo
app.MapGet("/api/todos/{id}", (Guid id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

// POST /api/todos - Create a new todo
app.MapPost("/api/todos", (CreateTodoRequest request) =>
{
    var todo = new TodoItem
    {
        Id = Guid.NewGuid(),
        Title = request.Title,
        IsComplete = false
    };
    todos.Add(todo);
    return Results.Created($"/api/todos/{todo.Id}", todo);
});

// PUT /api/todos/{id} - Update a todo
app.MapPut("/api/todos/{id}", (Guid id, UpdateTodoRequest request) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo is null)
        return Results.NotFound();

    todo.Title = request.Title;
    todo.IsComplete = request.IsComplete;
    return Results.Ok(todo);
});

// DELETE /api/todos/{id} - Delete a todo (requires authentication)
app.MapDelete("/api/todos/{id}", (Guid id) =>
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
    public Guid Id { get; set; }
    public required string Title { get; set; }

    [JsonPropertyName("complete")]
    public bool IsComplete { get; set; }
}

record CreateTodoRequest(string Title);
record UpdateTodoRequest(string Title, bool IsComplete);
record LoginRequest(string Username, string Password);
