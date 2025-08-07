# HybridCache API with Redis

A .NET 9 Minimal API demonstrating HybridCache with Redis integration.

## Features

- **HybridCache**: Unified in-memory and distributed caching
- **Redis Integration**: Distributed cache backend
- **Swagger UI**: Interactive API documentation
- **Docker Support**: Complete containerization
- **Fallback Support**: Works without Redis for local development

## Running the Application

### Option 1: Local Development (No Redis)

For quick development without Redis:

```bash
dotnet run
```

The application will use in-memory distributed cache as fallback.

### Option 2: Local Development with Redis

1. **Start Redis**:
   ```bash
   docker run -d --name redis -p 6379:6379 redis:latest
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

### Option 3: Docker Compose (Recommended)

1. **Start all services**:
   ```bash
   docker-compose up -d
   ```

2. **Access the application**:
   - API: http://localhost:8080
   - Swagger UI: http://localhost:8080
   - Redis: localhost:6379

### Option 4: Visual Studio Debug Mode

#### Method A: Docker Compose Debugging (Recommended)

1. **In Visual Studio Solution Explorer**:
   - Right-click on `docker-compose.yml`
   - Select **"Set as Startup Project"**
   - You'll see a green play button next to it

2. **Press F5** - Visual Studio will:
   - Start both Redis and application containers
   - Attach the debugger to the application
   - Open the browser automatically

#### Method B: Manual Redis Setup

If you prefer to use the Dockerfile directly:

1. **Start Redis manually**:
   ```bash
   docker run -d --name redis-debug -p 6379:6379 redis:latest
   ```

2. **In Visual Studio**:
   - Right-click on the project
   - Select **"Set as Startup Project"**
   - Press F5 to debug

#### Method C: Docker Compose + Attach Debugger

1. **Start Docker Compose from command line**:
   ```bash
   docker-compose up -d
   ```

2. **In Visual Studio**:
   - Go to **Debug** → **Attach to Process**
   - Select the running container process
   - Or use **Debug** → **Attach to Process** → **Docker Container**

## API Endpoints

### HybridCache Endpoints
- `GET /hybrid/products` - Get all products
- `GET /hybrid/products/{id}` - Get product by ID
- `POST /hybrid/products` - Create new product
- `DELETE /hybrid/products/{id}/cache` - Invalidate cache

### Traditional Cache Endpoints (for comparison)
- `GET /traditional/products` - Get all products
- `GET /traditional/products/{id}` - Get product by ID

### Health Check
- `GET /health` - Application health status

## Testing the API

### Using PowerShell

```powershell
# Health check
Invoke-RestMethod -Uri "http://localhost:8080/health"

# Get all products
Invoke-RestMethod -Uri "http://localhost:8080/hybrid/products"

# Get specific product
Invoke-RestMethod -Uri "http://localhost:8080/hybrid/products/1"

# Create a product
Invoke-RestMethod -Uri "http://localhost:8080/hybrid/products" -Method POST -ContentType "application/json" -Body '{"name":"Test Product","description":"A test product","price":99.99,"isActive":true}'

# Invalidate cache
Invoke-RestMethod -Uri "http://localhost:8080/hybrid/products/1/cache" -Method DELETE
```

### Using Swagger UI

1. Open http://localhost:8080 in your browser
2. Use the interactive interface to test all endpoints

## Redis Verification

To verify Redis is working:

```bash
# Check Redis keys
docker exec minimalredisapi-redis redis-cli KEYS "*"

# Check Redis connection
docker exec minimalredisapi-redis redis-cli PING
```

## Troubleshooting

### Visual Studio Debug Mode Issues

**Problem**: Getting Redis connection errors in Visual Studio debug mode.

**Solutions**: 
1. **Use Docker Compose debugging** (Method A above)
2. **Start Redis manually** before debugging (Method B above)
3. **Use Docker Compose + Attach Debugger** (Method C above)

### Port Conflicts

**Problem**: Port 8080 is already in use.

**Solution**: 
1. Change ports in `docker-compose.yml`
2. Or stop the conflicting service

### Redis Connection Issues

**Problem**: Cannot connect to Redis.

**Solutions**:
1. Ensure Redis is running: `docker ps`
2. Check Redis logs: `docker logs minimalredisapi-redis`
3. Verify network connectivity between containers

## Architecture

- **HybridCache**: Provides unified caching API
- **Redis**: Distributed cache backend
- **In-Memory Cache**: Local cache layer
- **Fallback**: In-memory distributed cache when Redis unavailable

## Configuration

The application automatically detects the environment:
- **Docker Compose**: Uses Redis service
- **Visual Studio Debug**: Uses localhost Redis or fallback
- **Local Development**: Uses localhost Redis or fallback 