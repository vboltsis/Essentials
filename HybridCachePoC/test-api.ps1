# HybridCache PoC API Test Script
# Run this script after starting the application

Write-Host "🧪 Testing HybridCache PoC API..." -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green

$baseUrl = "http://localhost:5050"

# Test 1: Health Check
Write-Host "`n1. Testing Health Check..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/health" -UseBasicParsing
    Write-Host "✅ Health Check: $($response.Content)" -ForegroundColor Green
} catch {
    Write-Host "❌ Health Check Failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Get All Customers (First Request - Database)
Write-Host "`n2. Testing Get All Customers (First Request - Database)..." -ForegroundColor Yellow
try {
    $startTime = Get-Date
    $response = Invoke-WebRequest -Uri "$baseUrl/customers" -UseBasicParsing
    $endTime = Get-Date
    $duration = ($endTime - $startTime).TotalMilliseconds
    $customers = $response.Content | ConvertFrom-Json
    Write-Host "✅ Get All Customers: Found $($customers.Count) customers in ${duration}ms" -ForegroundColor Green
} catch {
    Write-Host "❌ Get All Customers Failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Get All Customers (Second Request - Cache)
Write-Host "`n3. Testing Get All Customers (Second Request - Cache)..." -ForegroundColor Yellow
try {
    $startTime = Get-Date
    $response = Invoke-WebRequest -Uri "$baseUrl/customers" -UseBasicParsing
    $endTime = Get-Date
    $duration = ($endTime - $startTime).TotalMilliseconds
    $customers = $response.Content | ConvertFrom-Json
    Write-Host "✅ Get All Customers (Cache): Found $($customers.Count) customers in ${duration}ms" -ForegroundColor Green
} catch {
    Write-Host "❌ Get All Customers (Cache) Failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Get Specific Customer
Write-Host "`n4. Testing Get Specific Customer..." -ForegroundColor Yellow
try {
    $startTime = Get-Date
    $response = Invoke-WebRequest -Uri "$baseUrl/customers" -UseBasicParsing
    $customers = $response.Content | ConvertFrom-Json
    if ($customers.Count -gt 0) {
        $customerId = $customers[0].id
        $response = Invoke-WebRequest -Uri "$baseUrl/customers/$customerId" -UseBasicParsing
        $endTime = Get-Date
        $duration = ($endTime - $startTime).TotalMilliseconds
        $customer = $response.Content | ConvertFrom-Json
        Write-Host "✅ Get Specific Customer: $($customer.firstName) $($customer.lastName) in ${duration}ms" -ForegroundColor Green
    } else {
        Write-Host "⚠️ No customers found to test specific customer retrieval" -ForegroundColor Yellow
    }
} catch {
    Write-Host "❌ Get Specific Customer Failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 5: Create New Customer
Write-Host "`n5. Testing Create New Customer..." -ForegroundColor Yellow
try {
    $newCustomer = @{
        firstName = "Test"
        lastName = "User"
        email = "test.user@example.com"
        phoneNumber = "+1-555-0123"
        company = "Test Company"
        address = "123 Test St"
        city = "Test City"
        state = "TS"
        postalCode = "12345"
        country = "USA"
        notes = "Test customer created by script"
    } | ConvertTo-Json

    $headers = @{
        "Content-Type" = "application/json"
    }

    $response = Invoke-WebRequest -Uri "$baseUrl/customers" -Method POST -Body $newCustomer -Headers $headers -UseBasicParsing
    $createdCustomer = $response.Content | ConvertFrom-Json
    Write-Host "✅ Create Customer: $($createdCustomer.firstName) $($createdCustomer.lastName) created with ID $($createdCustomer.id)" -ForegroundColor Green
} catch {
    Write-Host "❌ Create Customer Failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 6: Get Metrics
Write-Host "`n6. Testing Get Metrics..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/metrics" -UseBasicParsing
    $metrics = $response.Content | ConvertFrom-Json
    Write-Host "✅ Metrics Retrieved:" -ForegroundColor Green
    Write-Host "   - Hit Rate: $($metrics.hitRatePercentage)%" -ForegroundColor Cyan
    Write-Host "   - Total Requests: $($metrics.totalRequests)" -ForegroundColor Cyan
    Write-Host "   - Cache Hits: $($metrics.cacheHits)" -ForegroundColor Cyan
    Write-Host "   - Cache Misses: $($metrics.cacheMisses)" -ForegroundColor Cyan
    Write-Host "   - Average Response Time: $($metrics.averageResponseTimeMs)ms" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Get Metrics Failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 7: Swagger UI
Write-Host "`n7. Testing Swagger UI..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/swagger" -UseBasicParsing
    Write-Host "✅ Swagger UI is accessible" -ForegroundColor Green
    Write-Host "   📖 Swagger URL: $baseUrl/swagger" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Swagger UI Failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n🎉 API Testing Complete!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host "`n📊 Next Steps:" -ForegroundColor Yellow
Write-Host "1. Open Grafana: http://localhost:3000 (admin/admin123)" -ForegroundColor Cyan
Write-Host "2. Open pgAdmin: http://localhost:8080 (admin@hybridcache.com/admin123)" -ForegroundColor Cyan
Write-Host "3. Open Redis Commander: http://localhost:8081" -ForegroundColor Cyan
Write-Host "4. Open Swagger UI: $baseUrl/swagger" -ForegroundColor Cyan
Write-Host "`n🚀 Happy Caching!" -ForegroundColor Green 