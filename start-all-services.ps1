# Start All Services Script

Write-Host "🚀 Starting all Electronic Shop services..." -ForegroundColor Green

# Array to store job objects
$jobs = @()

# Start Product Service
Write-Host "`nStarting Product Service on port 5001..." -ForegroundColor Yellow
$jobs += Start-Job -ScriptBlock {
    Set-Location "src\Services\ElectronicShop.ProductService"
    dotnet run
} -Name "ProductService"

# Wait a moment between service starts
Start-Sleep -Seconds 2

# Start Customer Service
Write-Host "Starting Customer Service on port 5002..." -ForegroundColor Yellow
$jobs += Start-Job -ScriptBlock {
    Set-Location "src\Services\ElectronicShop.CustomerService"
    dotnet run
} -Name "CustomerService"

# Wait a moment between service starts
Start-Sleep -Seconds 2

# Start Inventory Service
Write-Host "Starting Inventory Service on port 5003..." -ForegroundColor Yellow
$jobs += Start-Job -ScriptBlock {
    Set-Location "src\Services\ElectronicShop.InventoryService"
    dotnet run
} -Name "InventoryService"

Write-Host "`n✅ All services are starting in background jobs" -ForegroundColor Green
Write-Host "Wait a few seconds for services to start, then press any key to start the Console Client..." -ForegroundColor Cyan

# Wait for user input
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# Start Console Client in foreground
Write-Host "`n🎮 Starting Console Client..." -ForegroundColor Yellow
Set-Location "src\Client\ElectronicShop.ConsoleClient"
dotnet run

# Cleanup: Stop all background jobs when console client exits
Write-Host "`n🛑 Stopping all services..." -ForegroundColor Yellow
$jobs | Stop-Job
$jobs | Remove-Job

Write-Host "✅ All services stopped" -ForegroundColor Green