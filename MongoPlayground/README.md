# Repository Demo

This project demonstrates a repository pattern with selective caching, cache invalidation, and a MongoDB-powered search index.

## Requirements

- Docker and Docker Compose
- .NET 8 SDK (only if running outside Docker)

## Running with Docker

```bash
docker compose up --build
```

The `app` service will run the demo and log output to the console.

## Project Structure

```
repository-demo/
├── docker-compose.yml
├── Dockerfile
└── src
    ├── DemoApp.csproj
    ├── Program.cs
    ├── Models
    │   ├── Item.cs
    │   └── SearchEntry.cs
    └── Repositories
        ├── IItemRepository.cs
        └── ItemRepository.cs
```

## NuGet Packages

- MongoDB.Driver 2.25.2
- Microsoft.Extensions.Hosting 8.0
- Microsoft.Extensions.Caching.Memory 8.0
