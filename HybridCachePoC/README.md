# HybridCache PoC - .NET 9 with PostgreSQL and Grafana

A comprehensive Proof of Concept demonstrating hybrid caching patterns using .NET 9, PostgreSQL, Redis, and Grafana monitoring.

## 🚀 Quick Start

### 1. Start Infrastructure (Docker)
```bash
docker-compose up -d
```

### 2. Start Application
```bash
cd HybridCachePoC
dotnet run --urls "http://localhost:5050"
```

### 3. Access Services

| Service | URL | Credentials |
|---------|-----|-------------|
| **API Health** | http://localhost:5050/health | - |
| **API Swagger** | http://localhost:5050/swagger | - |
| **pgAdmin** | http://localhost:8080 | admin@hybridcache.com / admin123 |
| **Grafana** | http://localhost:3000 | admin / admin123 |
| **Redis Commander** | http://localhost:8081 | - |

## 📊 Architecture

### Infrastructure
- **PostgreSQL 15** - Primary database
- **Redis 7** - Distributed cache
- **pgAdmin 4** - Database management
- **Grafana** - Monitoring dashboard
- **Redis Commander** - Redis management

### Application
- **.NET 9** - Minimal API
- **Entity Framework Core** - ORM with PostgreSQL
- **Hybrid Caching** - Memory + Redis
- **Health Checks** - Redis and PostgreSQL
- **Grafana Integration** - SimpleJSON datasource

## 🔧 Features

### Cache Strategy
- **Memory Cache**: 30 min expiration, 10 min sliding
- **Redis Cache**: 60 min expiration, 15 min sliding
- **Cache-aside Pattern** with invalidation
- **Cache Warming** on startup
- **Performance Tracking** with metrics

### Database
- **PostgreSQL** with proper indexing
- **Customer Entity** with comprehensive fields
- **Automatic Seeding** on startup
- **Access Tracking** (LastAccessed, AccessCount)
- **Unique Email Index** for data integrity

### API Endpoints
- `GET /customers` - Get all customers
- `GET /customers/{id}` - Get specific customer
- `POST /customers` - Create customer
- `PUT /customers/{id}` - Update customer
- `DELETE /customers/{id}` - Delete customer
- `GET /metrics` - Get cache metrics
- `POST /api/grafana/query` - Grafana datasource

## 📈 Grafana Dashboard

The dashboard includes panels for:
- **Cache Hit Rate** - Percentage of cache hits
- **Average Response Time** - Response time metrics
- **Total Requests** - Request count tracking
- **Cache Hits/Misses** - Cache performance metrics
- **Memory Cache Size** - In-memory cache usage
- **Redis Cache Size** - Redis cache usage
- **Response Time by Source** - Performance by cache layer
- **Cache Metrics Summary** - Overview statistics

## 🧪 Testing

### Test Cache Performance
```bash
# Get all customers (first request - database)
curl http://localhost:5050/customers

# Get all customers (second request - cache)
curl http://localhost:5050/customers

# Get specific customer
curl http://localhost:5050/customers/{id}

# Create new customer
curl -X POST http://localhost:5050/customers \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Test","lastName":"User","email":"test@example.com"}'
```

### Monitor in Grafana
1. Open http://localhost:3000
2. Login with admin/admin123
3. Navigate to "HybridCache PoC Dashboard"
4. Watch real-time metrics as you make API calls

## 🔍 Troubleshooting

### Port Already in Use
If port 5050 is in use, try:
```bash
dotnet run --urls "http://localhost:5051"
```

### Database Connection Issues
1. Ensure PostgreSQL container is running:
   ```bash
   docker-compose ps
   ```

2. Check container logs:
   ```bash
   docker-compose logs postgres
   ```

### Redis Connection Issues
1. Ensure Redis container is running:
   ```bash
   docker-compose ps
   ```

2. Check container logs:
   ```bash
   docker-compose logs redis
   ```

## 📁 Project Structure

```
HybridCachePoC/
├── Controllers/
│   ├── CustomerController.cs
│   └── GrafanaController.cs
├── Data/
│   └── AppDbContext.cs
├── HealthChecks/
│   └── RedisHealthCheck.cs
├── Models/
│   └── Customer.cs
├── Services/
│   ├── CustomerService.cs
│   ├── DataSeeder.cs
│   ├── ICustomerService.cs
│   └── MetricsService.cs
├── grafana/
│   ├── dashboard.json
│   └── datasource.json
├── docker-compose.yml
├── Program.cs
└── appsettings.json
```

## 🛠️ Development

### Adding New Features
1. Create models in `Models/`
2. Add DbContext configuration in `Data/AppDbContext.cs`
3. Create services in `Services/`
4. Add controllers in `Controllers/`
5. Update health checks if needed

### Database Migrations
```bash
# Create migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## 📊 Performance Monitoring

The application tracks:
- Cache hit/miss rates
- Response times by cache source
- Memory and Redis cache sizes
- Total request counts
- Customer access patterns

All metrics are available through:
- `/metrics` endpoint
- Grafana dashboard
- Health check endpoints

## 🔐 Security Notes

- Default credentials are for development only
- PostgreSQL data is persisted in Docker volumes
- Redis data is persisted with AOF
- Grafana dashboard is pre-configured

## 📝 License

This is a Proof of Concept for educational purposes. 