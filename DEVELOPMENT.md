# This file keeps track of the StockPilot Backend project structure and conventions

## Architecture

The project follows a Clean Architecture pattern with:

- **Controllers**: API endpoints, HTTP communication
- **Services**: Business logic, data transformations
- **Models**: Database entities, EF Core configuration
- **DTOs**: Data transfer objects for API requests/responses
- **Data**: Entity Framework Core context and database access
- **Middleware**: Custom request/response handling

## Conventions

1. **Naming**: PascalCase for classes, camelCase for properties in DTOs
2. **Async/Await**: All database operations use async methods
3. **Error Handling**: Consistent error responses with appropriate HTTP status codes
4. **Authentication**: JWT Bearer tokens in Authorization header
5. **User Isolation**: All data queries filtered by UserId from JWT claims

## Adding New Endpoints

1. Create DTOs in `DTOs/` folder
2. Create interface in `Services/IServiceName.cs`
3. Create implementation in `Services/ServiceName.cs`
4. Register service in `Program.cs`
5. Create controller in `Controllers/` folder
6. Add methods following the existing patterns

## Database Migrations

```bash
# Create migration after model changes
dotnet ef migrations add MigrationName

# Apply to database
dotnet ef database update

# Remove last migration if needed
dotnet ef migrations remove
```

## Testing

API endpoints can be tested using:
- Swagger UI: `https://localhost:5001/swagger`
- Postman: Import endpoints and use JWT tokens from login response
- cURL: Use Bearer token in Authorization header

## Deployment

For production deployment:

1. Update `appsettings.json` with production connection string
2. Change JWT secret key to a strong random string
3. Enable HTTPS everywhere
4. Configure CORS for your frontend domain
5. Set up proper logging to persistent storage
6. Use environment variables for sensitive configuration

## Performance Considerations

- Database queries include `.Include()` for related data to avoid N+1 queries
- Indexes on frequently queried columns (UserId, SKU, Email)
- Pagination should be added for large datasets in future versions
- Consider caching for reports and dashboard data

## Security

- All endpoints except /auth/register and /auth/login require [Authorize]
- Passwords hashed with SHA-256
- JWT tokens expire after configured time
- CORS restricted to known origins in production
- Sensitive configuration in environment variables

## Future Enhancements

- [ ] Pagination for list endpoints
- [ ] Advanced filtering and sorting
- [ ] Batch operations for inventory
- [ ] Email notifications for alerts
- [ ] PDF receipt generation
- [ ] Advanced analytics and forecasting
- [ ] Multi-user/role support
- [ ] API rate limiting
- [ ] Audit logging
