# Electronic Shop Order Management - Quick Start

Write-Host "🏪 Electronic Shop Order Management System" -ForegroundColor Green
Write-Host "=========================================" -ForegroundColor Green

# Check if Docker is running
Write-Host "`n🔍 Checking Docker..." -ForegroundColor Yellow
try {
    docker --version | Out-Null
    Write-Host "✅ Docker is available" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker is not available or not running" -ForegroundColor Red
    Write-Host "Please install Docker and ensure it's running" -ForegroundColor Red
    exit 1
}

# Start Consul
Write-Host "`n🚀 Starting Consul..." -ForegroundColor Yellow
docker-compose up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Consul started successfully" -ForegroundColor Green
    Write-Host "   Consul UI available at: http://localhost:8500" -ForegroundColor Cyan
} else {
    Write-Host "❌ Failed to start Consul" -ForegroundColor Red
    exit 1
}

# Build the solution
Write-Host "`n🔨 Building solution..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Solution built successfully" -ForegroundColor Green
} else {
    Write-Host "❌ Failed to build solution" -ForegroundColor Red
    exit 1
}

Write-Host "`n🎯 Ready to start services!" -ForegroundColor Green
Write-Host "`nTo start the services, open separate terminal windows and run:" -ForegroundColor Cyan
Write-Host "`n1. Product Service:" -ForegroundColor White
Write-Host "   cd src\Services\ElectronicShop.ProductService" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray

Write-Host "`n2. Customer Service:" -ForegroundColor White
Write-Host "   cd src\Services\ElectronicShop.CustomerService" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray

Write-Host "`n3. Inventory Service:" -ForegroundColor White
Write-Host "   cd src\Services\ElectronicShop.InventoryService" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray

Write-Host "`n4. Console Client:" -ForegroundColor White
Write-Host "   cd src\Client\ElectronicShop.ConsoleClient" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray

Write-Host "`n📊 Service URLs:" -ForegroundColor Cyan
Write-Host "   Product Service:  http://localhost:5001" -ForegroundColor Gray
Write-Host "   Customer Service: http://localhost:5002" -ForegroundColor Gray
Write-Host "   Inventory Service: http://localhost:5003" -ForegroundColor Gray
Write-Host "   Consul UI:        http://localhost:8500" -ForegroundColor Gray

Write-Host "`n🎮 Or run the start-all-services.ps1 script to start all services automatically!" -ForegroundColor Yellow