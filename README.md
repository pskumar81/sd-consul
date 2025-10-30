# Electronic Shop Order Management System

A microservices-based order management system for an electronic shop built with .NET 9 and Consul for service discovery.

## 🏗️ Architecture

This solution demonstrates a microservices architecture with the following components:

### Microservices
- **Product Service** (Port 5001) - Manages electronic products catalog
- **Customer Service** (Port 5002) - Handles customer information
- **Inventory Service** (Port 5003) - Tracks product stock levels
- **Console Client** - Interactive client that discovers and communicates with services

### Shared Libraries
- **ElectronicShop.Shared.Models** - Common data models and DTOs
- **ElectronicShop.Shared.ServiceDiscovery** - Consul service discovery functionality

### Infrastructure
- **Consul** - Service discovery and health monitoring

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- Docker and Docker Compose
- Visual Studio 2022 or VS Code

### 1. Start Consul

First, start the Consul service discovery server using Docker Compose:

```powershell
docker-compose up -d
```

This will start Consul on `http://localhost:8500`. You can access the Consul UI at this address.

### 2. Build the Solution

```powershell
dotnet build
```

### 3. Start the Services

Open multiple terminal windows and start each service:

**Terminal 1 - Product Service:**
```powershell
cd src\Services\ElectronicShop.ProductService
dotnet run
```

**Terminal 2 - Customer Service:**
```powershell
cd src\Services\ElectronicShop.CustomerService
dotnet run
```

**Terminal 3 - Inventory Service:**
```powershell
cd src\Services\ElectronicShop.InventoryService
dotnet run
```

### 4. Run the Console Client

**Terminal 4 - Console Client:**
```powershell
cd src\Client\ElectronicShop.ConsoleClient
dotnet run
```

## 📊 Service URLs

When running locally, the services are available at:

- **Product Service**: http://localhost:5001 (Swagger UI available)
- **Customer Service**: http://localhost:5002 (Swagger UI available)  
- **Inventory Service**: http://localhost:5003 (Swagger UI available)
- **Consul UI**: http://localhost:8500

## 🎯 Features Demonstrated

### Service Discovery
- Automatic service registration with Consul
- Health check monitoring
- Service discovery for inter-service communication
- Load balancing (round-robin) for multiple service instances

### Product Service
- CRUD operations for electronic products
- Product search and filtering by category
- Sample data includes smartphones, laptops, and audio equipment

### Customer Service
- Customer management with validation
- Email-based customer lookup
- Address management

### Inventory Service
- Stock level tracking
- Stock movement history
- Availability checking
- Low stock warnings

### Console Client
- Interactive menu-driven interface
- Service discovery demonstration
- Real-time communication with all services
- Sample order creation workflow

## 🛠️ API Examples

### Product Service

**Get all products:**
```http
GET http://localhost:5001/api/products
```

**Search products:**
```http
GET http://localhost:5001/api/products/search?q=iPhone
```

**Get products by category:**
```http
GET http://localhost:5001/api/products/category/Smartphones
```

### Customer Service

**Get all customers:**
```http
GET http://localhost:5002/api/customers
```

**Find customer by email:**
```http
GET http://localhost:5002/api/customers/email/john.doe@email.com
```

### Inventory Service

**Check product inventory:**
```http
GET http://localhost:5003/api/inventory/product/1
```

**Check stock availability:**
```http
GET http://localhost:5003/api/inventory/check-availability?productId=1&quantity=5
```

## 🔧 Configuration

### Service Discovery Configuration

Each service can be configured via `appsettings.json`:

```json
{
  "ServiceDiscovery": {
    "ConsulAddress": "http://localhost:8500",
    "DataCenter": "dc1"
  }
}
```

### Service Registration

Services automatically register with Consul using the pattern:
- **Service ID**: `{service-name}-{machine-name}-{process-id}`
- **Service Name**: `{service-name}`
- **Health Check**: Automatic HTTP health checks every 30 seconds

## 📁 Project Structure

```
sd-consul/
├── src/
│   ├── Shared/
│   │   ├── ElectronicShop.Shared.Models/          # Common data models
│   │   └── ElectronicShop.Shared.ServiceDiscovery/ # Consul integration
│   ├── Services/
│   │   ├── ElectronicShop.ProductService/         # Product management API
│   │   ├── ElectronicShop.CustomerService/        # Customer management API
│   │   └── ElectronicShop.InventoryService/       # Inventory management API
│   └── Client/
│       └── ElectronicShop.ConsoleClient/          # Interactive console client
├── docker-compose.yml                             # Consul setup
└── ElectronicShop.OrderManagement.sln            # Solution file
```

## 🎮 Using the Console Client

The console client provides an interactive menu to:

1. **Show Available Services** - Display all registered services
2. **Product Management** - Browse products, search, filter by category
3. **Customer Management** - View customers, search by email
4. **Inventory Management** - Check stock levels
5. **Create Sample Order** - Demonstrate order workflow with validation

## 🔍 Monitoring

- **Consul UI**: http://localhost:8500 - View service health and registrations
- **Service Health Endpoints**: Each service exposes `/api/health` for health checks
- **Swagger UI**: Available on each service for API exploration

## 🏗️ Extending the System

To add a new service:

1. Create a new Web API project
2. Add references to shared libraries
3. Configure Consul service discovery in `Program.cs`
4. Implement health check endpoint
5. Update service ports and configuration

## 🐳 Docker Support

The system includes Docker Compose configuration for Consul. To run the entire system in containers, you can extend the `docker-compose.yml` file to include the .NET services.

## 📝 Sample Data

The system includes sample data for demonstration:

### Products
- iPhone 15 Pro ($999.99)
- Samsung Galaxy S24 ($849.99)
- MacBook Pro 14" ($1999.99)
- Sony WH-1000XM5 ($399.99)

### Customers
- John Doe (john.doe@email.com)
- Jane Smith (jane.smith@email.com)

### Inventory
- All products have initial stock levels with configurable reorder points

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## 📄 License

This project is for educational purposes and demonstrates microservices patterns with service discovery.