# StockPilot Backend API

ASP.NET Core 8.0 REST API for StockPilot - Inventory & Sales Management System

## Features

- **User Management**: Registration, Login, JWT Authentication
- **Inventory Management**: CRUD operations, barcode scanning, low-stock alerts
- **Sales Management**: Point of Sale, Receipt generation, payment tracking
- **Reporting**: Dashboard analytics, daily sales trends, top-selling items
- **Alerts System**: Real-time notifications for low stock and out-of-stock items

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core 8.0
- **Authentication**: JWT Bearer Tokens
- **Logging**: Serilog
- **API Documentation**: Swagger/OpenAPI

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or any SQL Server instance)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
```bash
git clone https://github.com/vamshikreddy7-debug/StockPilot-Backend.git
cd StockPilot-Backend
```

2. Update the connection string in `appsettings.json`
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=StockPilotDb;Trusted_Connection=true;"
}
```

3. Update JWT settings in `appsettings.json`
```json
"JwtSettings": {
    "SecretKey": "your-secret-key-at-least-32-characters-long",
    "ExpiryMinutes": 60
}
```

4. Apply database migrations
```bash
dotnet ef database update
```

5. Run the application
```bash
dotnet run
```

The API will be available at `https://localhost:5001`
Swagger documentation: `https://localhost:5001/swagger`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `GET /api/auth/profile` - Get user profile (Requires Auth)

### Inventory
- `GET /api/inventory` - Get all items (Requires Auth)
- `GET /api/inventory/{id}` - Get item details
- `POST /api/inventory` - Create new item
- `PUT /api/inventory/{id}` - Update item
- `DELETE /api/inventory/{id}` - Delete item
- `GET /api/inventory/low-stock` - Get low-stock items
- `GET /api/inventory/search?query=` - Search items

### Sales
- `POST /api/sales` - Create sale/receipt
- `GET /api/sales/{id}` - Get sale details
- `GET /api/sales/history?days=30` - Get sales history
- `GET /api/sales/total-sales?startDate=&endDate=` - Get total sales for period
- `GET /api/sales/total-items-sold?startDate=&endDate=` - Get total items sold

### Reports
- `GET /api/reports/dashboard` - Get dashboard report
- `GET /api/reports/daily-sales?days=7` - Get daily sales
- `GET /api/reports/top-items?days=30` - Get top selling items

## Project Structure

```
StockPilot.API/
├── Controllers/         # API endpoints
├── Models/             # Database entities
├── DTOs/               # Data transfer objects
├── Services/           # Business logic
├── Data/               # Database context
├── Middleware/         # Custom middleware
├── Program.cs          # Application configuration
└── appsettings.json    # Configuration
```

## Database Schema

- **Users**: User accounts and profiles
- **InventoryItems**: Product catalog
- **Sales**: Transaction records
- **SaleItems**: Line items in sales
- **Alerts**: Low-stock and out-of-stock alerts

## Security

- JWT Bearer token authentication
- Password hashing with SHA-256
- User-scoped data isolation
- CORS enabled for frontend integration

## Logging

Logs are written to:
- Console (for development)
- File: `logs/stockpilot-YYYY-MM-DD.txt` (rolling daily)

## Android App Integration

The backend API supports:
- RESTful endpoints for mobile clients
- JWT authentication tokens
- Real-time inventory updates
- Receipt generation
- Sales tracking

## Web App Integration (React/TypeScript)

The frontend at `https://github.com/vamshikreddy7-debug/UXDesignForStockPilot` consumes this API with:
- Dashboard analytics
- Inventory management
- Point of Sale interface
- Reporting tools

## Error Handling

All errors return appropriate HTTP status codes:
- `400` - Bad Request
- `401` - Unauthorized
- `404` - Not Found
- `500` - Internal Server Error

## License

MIT License

## Support

For issues and questions, please open an issue on GitHub.
