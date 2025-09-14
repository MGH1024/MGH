# MGH Public Libraries

MGH is a collection of reusable, modular **.NET/C# libraries** that simplify backend application development.  
It consolidates cross-cutting concerns, infrastructure, and domain essentials into one place — so you don’t need to reinvent the wheel.

---

## 📌 Features

- ✅ Centralized **Logging** (Serilog, MSSQL, etc.)  
- ✅ **Caching** abstractions (In-memory, Redis)  
- ✅ **Health Checks** for services  
- ✅ Reusable **HTTP Clients** with `IHttpClientFactory`  
- ✅ **Identity / Security** helpers  
- ✅ **Persistence** via EF Core & Dapper  
- ✅ **Event Bus** integration (RabbitMQ)  
- ✅ **ElasticSearch** utilities  
- ✅ Standardized **Swagger / API Endpoint** setup  

---

## 🏗️ Project Structure

```
MGH/
├── MGH.Core.Application       # Application layer
├── MGH.Core.Domain            # Domain layer
├── MGH.Core.CrossCutting      # Cross-cutting concerns (logging, caching, etc.)
├── MGH.Core.Infrastructure    # Infrastructure (persistence, security, http, event bus, elastic, health)
├── MGH.Core.Endpoint          # API/Web endpoint helpers (Swagger, HealthChecks)
├── MGH.sln                    # Solution file
└── .github/workflows          # CI/CD workflows
```

Each module is independent, so you can reference only the parts your project requires.

---

## ⚙️ Requirements

- [.NET SDK](https://dotnet.microsoft.com/) (latest LTS recommended)  
- Visual Studio / Rider / VS Code  
- Optional external dependencies:  
  - RabbitMQ (if using Event Bus)  
  - ElasticSearch (if using Elastic integration)  
  - SQL Server or PostgreSQL (if using EF/Dapper persistence)  
  - Redis (if using distributed caching)  

---

## 🚀 Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/MGH1024/MGH.git
   cd MGH
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Build the solution:

   ```bash
   dotnet build
   ```

4. Reference the desired projects in your own solution, e.g.:

   ```xml
   <ProjectReference Include="..\MGH.Core.CrossCutting\MGH.Core.CrossCutting.csproj" />
   ```

---

## 🔧 Example Usage

### Logging
```csharp
var logger = Log.ForContext<Program>();
logger.Information("Application started!");
```

### EF Core Repository
```csharp
public class ProductRepository : EfRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }
}
```

### RabbitMQ Event Bus
```csharp
_eventBus.Publish(new OrderCreatedEvent(orderId));
```

---

## ⚡ Configuration

Most modules use `appsettings.json`. Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=AppDb;User Id=sa;Password=yourStrong(!)Password;"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "UserName": "guest",
    "Password": "guest"
  },
  "ElasticSearch": {
    "Uri": "http://localhost:9200",
    "Index": "mgh-logs"
  }
}
```

---

## 🤝 Contributing

Contributions are welcome!  

1. Fork the repo  
2. Create a feature branch (`git checkout -b feature/my-feature`)  
3. Commit your changes (`git commit -m "Add feature"`)  
4. Push the branch (`git push origin feature/my-feature`)  
5. Open a Pull Request  

---

## 📄 License

This project is licensed under the **MIT License** – see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

- **[MGH1024](https://github.com/MGH1024)** – Maintainer & Developer

---
