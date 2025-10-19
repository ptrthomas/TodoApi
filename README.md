# Todo API

A minimal REST API for managing todos, built with .NET for training workshops.

## Features

- Full CRUD operations for todos
- Bearer token authentication (DELETE operations only)
- In-memory storage (no database required)
- Swagger UI for API testing
- Cross-platform (Mac/Windows/Linux)
- Just 2 files!

## Getting Started

### Step 1: Install .NET SDK (if not already installed)

Check if .NET is installed:

```bash
dotnet --version
```

If you see a version number (9.0 or higher), you're good to go! Skip to Step 2.

If not installed, download and install the .NET SDK:

- **Windows/Mac/Linux**: [Download .NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- After installation, close and reopen your terminal/command prompt
- Verify installation: `dotnet --version`

### Step 2: Clone the Repository

```bash
git clone https://github.com/ptrthomas/TodoApi.git
cd TodoApi
```

### Step 3: Run the API

```bash
dotnet run
```

That's it! The API will start at `http://localhost:5000`.

You should see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

### Step 4: Test the API

Open your browser and go to:
- **Swagger UI**: http://localhost:5000/swagger

Or use curl from another terminal:
```bash
curl http://localhost:5000/todos
```

## Testing with Xplorer

A complete Postman collection is included for easy testing: [TodoApi.postman_collection.json](TodoApi.postman_collection.json)

**To use it:**
1. Download and install [Xplorer](https://xplorer.karatelabs.io/) - quick install for Windows/Mac/Linux
2. Drag and drop the `TodoApi.postman_collection.json` file into Xplorer, or use the UI to open the collection
3. The collection includes all 7 endpoints in order
4. Run "1. Login" first - it automatically saves the bearer token
5. Then try the other requests - "6. Delete Todo" will use the saved token automatically

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

## Example Usage with curl

**Note for Windows users**: Use PowerShell or install [Git Bash](https://git-scm.com/downloads) for these curl commands. Alternatively, use Postman (see above) or the Swagger UI.

```bash
# 1. Create a todo
curl -X POST http://localhost:5000/todos \
  -H "Content-Type: application/json" \
  -d '{"title":"Learn REST APIs"}'

# 2. Get all todos
curl http://localhost:5000/todos

# 3. Update a todo (mark as complete)
curl -X PUT http://localhost:5000/todos/1 \
  -H "Content-Type: application/json" \
  -d '{"title":"Learn REST APIs","isComplete":true}'

# 4. Login to get bearer token (needed for delete)
curl -X POST http://localhost:5000/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password123"}'

# Response will contain an access_token - copy it!
# Example: {"tokenType":"Bearer","accessToken":"CfDJ8...","expiresIn":3600}

# 5. Delete a todo (requires authentication - replace YOUR_TOKEN with the access_token from step 4)
curl -X DELETE http://localhost:5000/todos/1 \
  -H "Authorization: Bearer YOUR_TOKEN"

# 6. Reset all data (helpful for starting fresh)
curl http://localhost:5000/reset
```

## Authentication

The DELETE endpoint requires Bearer token authentication. Use these hardcoded credentials:

- Username: `admin`
- Password: `password123`

The token is valid for 1 hour after login.

## Troubleshooting

**"dotnet: command not found"**
- Make sure you've installed the .NET SDK (see Step 1 above)
- Close and reopen your terminal after installation

**Port 5000 already in use**
- Stop any other application using port 5000, or
- Edit `Program.cs` to change the port

**Cannot access from another machine**
- By default, the API only listens on localhost
- To allow external access, modify the URL in `Program.cs`

## Workshop Tips

- Use the `/reset` endpoint between exercises to clear all data
- The Swagger UI (`/swagger`) is the easiest way to test endpoints interactively
- All data is stored in-memory and will be lost when the application stops
- Credentials are hardcoded (`admin`/`password123`) for workshop simplicity
- Only DELETE operations require authentication
