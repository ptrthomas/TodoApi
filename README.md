# Todo API Simple

A minimal REST API for managing todos, built with .NET for training workshops.

## Features

- Full CRUD operations for todos
- Bearer token authentication (DELETE operations only)
- In-memory storage (no database required)
- Swagger UI for API testing
- Cross-platform (Mac/Windows/Linux)
- Just 2 files!

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)

## Build and Run

```bash
dotnet run
```

The API will start at `http://localhost:5000` (or `https://localhost:5001` for HTTPS).

## Testing with Postman

A complete Postman collection is included: [TodoApi.postman_collection.json](TodoApi.postman_collection.json)

To use it:
1. Import the collection into Postman
2. The collection includes all endpoints in order
3. Run "1. Login" first - it automatically saves the bearer token
4. The "6. Delete Todo" request uses the saved token automatically

## API Endpoints

Access Swagger UI at: `http://localhost:5000/swagger`

- `POST /login` - Login to get bearer token
  ```json
  { "username": "admin", "password": "password123" }
  ```
- `GET /reset` - Clear all todos (helpful for workshop demos)
- `GET /todos` - Get all todos
- `GET /todos/{id}` - Get a specific todo
- `POST /todos` - Create a new todo
  ```json
  { "title": "Learn .NET" }
  ```
- `PUT /todos/{id}` - Update a todo
  ```json
  { "title": "Learn .NET", "isComplete": true }
  ```
- `DELETE /todos/{id}` - Delete a todo (requires authentication)

## Example Usage

```bash
# Login to get bearer token
curl -X POST http://localhost:5000/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password123"}'

# The response will contain an access_token - save it for authenticated requests
# Example response: {"tokenType":"Bearer","accessToken":"CfDJ8...","expiresIn":3600}

# Create a todo
curl -X POST http://localhost:5000/todos \
  -H "Content-Type: application/json" \
  -d '{"title":"Learn REST APIs"}'

# Get all todos
curl http://localhost:5000/todos

# Update a todo (mark as complete)
curl -X PUT http://localhost:5000/todos/1 \
  -H "Content-Type: application/json" \
  -d '{"title":"Learn REST APIs","isComplete":true}'

# Delete a todo (requires authentication)
curl -X DELETE http://localhost:5000/todos/1 \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN_HERE"

# Reset all data
curl http://localhost:5000/reset
```

## Authentication

The DELETE endpoint requires Bearer token authentication. Use these hardcoded credentials:

- Username: `admin`
- Password: `password123`

The token is valid for 1 hour after login.

## Notes

- All data is stored in-memory and will be lost when the application stops
- Bearer authentication only on DELETE operations (simplified for training)
- No database setup required
- Credentials are hardcoded for workshop simplicity
