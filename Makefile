# Build and run in development
dotnet run

# Run migrations
dotnet ef database update

# Create migration
dotnet ef migrations add MigrationName

# Publish for production
dotnet publish -c Release
