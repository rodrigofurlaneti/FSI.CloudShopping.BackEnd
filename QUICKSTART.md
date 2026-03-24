# Quick Start Guide - FSI.CloudShopping v2

## 5-Minute Local Setup

### Prerequisites
- .NET 9.0 SDK installed
- Docker & Docker Compose (optional)
- MySQL 8.0+ OR Docker

### Option A: Docker (Recommended - 1 minute)

```bash
cd /sessions/epic-confident-tesla/mnt/CloudShopping
docker-compose up -d
```

API will be available at: `http://localhost:5000`
Swagger documentation: `http://localhost:5000/swagger`

**Default Credentials:**
- MySQL: user=`cloudshopping_user`, password=`cloudshopping_password`
- Database: `CloudShoppingDB`

### Option B: Local Development

```bash
# Install MySQL 8.0
# Create database CloudShoppingDB with user/password

# Navigate to project
cd /sessions/epic-confident-tesla/mnt/CloudShopping/src

# Build solution
dotnet build FSI.CloudShopping.sln

# Update appsettings.json with your MySQL credentials

# Apply migrations (once you implement them)
cd FSI.CloudShopping.WebAPI
dotnet ef database update

# Run the API
dotnet run
```

## Testing the API

### 1. Register as Guest
```bash
curl -X POST http://localhost:5000/api/v1/auth/register/guest \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com"}'
```

### 2. Login
```bash
curl -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password123"}'
```

### 3. Get Products
```bash
curl http://localhost:5000/api/v1/catalog/products?pageNumber=1&pageSize=10 \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Project Structure

```
src/
  ├── FSI.CloudShopping.Domain/        Domain entities & value objects
  ├── FSI.CloudShopping.Application/   Commands, queries, DTOs
  ├── FSI.CloudShopping.Infrastructure/ EF Core, repositories, services
  └── FSI.CloudShopping.WebAPI/        HTTP controllers & configuration

tests/
  ├── FSI.CloudShopping.UnitTests.Domain/
  └── FSI.CloudShopping.UnitTests.Application/
```

## Key Files to Modify

### Database Configuration
- **File**: `src/FSI.CloudShopping.WebAPI/appsettings.json`
- **Update**: `ConnectionStrings.DefaultConnection`

### JWT Secret Key
- **File**: `src/FSI.CloudShopping.WebAPI/appsettings.json`
- **Update**: `JwtSettings.Secret` (use a strong random key)

### Allowed CORS Origins
- **File**: `src/FSI.CloudShopping.WebAPI/appsettings.json`
- **Update**: `AllowedOrigins` array

## Most Important Command Handlers to Implement Next

1. **Cart Operations**
   - `AddToCartCommand`
   - `UpdateCartItemCommand`
   - `RemoveFromCartCommand`

2. **Order Processing**
   - `PlaceOrderCommand` (triggers SAGA)
   - `ConfirmOrderCommand`
   - `CancelOrderCommand`

3. **Payment**
   - `ProcessPaymentCommand`
   - `RefundPaymentCommand`

4. **Product Management** (BackOffice)
   - `CreateProductCommand`
   - `UpdateProductCommand`
   - `UpdateStockCommand`

## Database Migrations (When Ready)

```bash
cd src/FSI.CloudShopping.WebAPI

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration
dotnet ef database update

# Create subsequent migrations
dotnet ef migrations add YourMigrationName
```

## Troubleshooting

### Issue: Cannot connect to MySQL
- Check MySQL is running: `docker-compose ps`
- Verify connection string in appsettings.json
- For Docker: `docker-compose logs mysql`

### Issue: JWT token invalid
- Regenerate secret key in appsettings.json
- Restart the application
- Get new token via login endpoint

### Issue: CORS errors
- Add your frontend origin to `AllowedOrigins` in appsettings.json
- Restart the application

### Issue: Build fails
- Ensure .NET 9.0 SDK is installed: `dotnet --version`
- Clean and rebuild: `dotnet clean && dotnet build`

## Common Development Commands

```bash
# Build solution
dotnet build src/FSI.CloudShopping.sln

# Run tests
dotnet test src/FSI.CloudShopping.sln

# Run API
cd src/FSI.CloudShopping.WebAPI
dotnet run

# Clean build artifacts
dotnet clean src/FSI.CloudShopping.sln

# List project references
dotnet list src/FSI.CloudShopping.WebAPI/FSI.CloudShopping.WebAPI.csproj reference
```

## What's Already Working

✅ Project structure & dependencies
✅ Database schema & configurations
✅ JWT authentication
✅ Guest registration
✅ Login with token/refresh token
✅ Product listing (paginated)
✅ AutoMapper configurations
✅ FluentValidation setup
✅ MediatR pipeline
✅ Error handling
✅ Logging infrastructure
✅ CORS configuration
✅ Swagger documentation

## What Needs Implementation

📋 Command handlers for Cart, Order, Payment
📋 Query handlers for reporting
📋 SAGA orchestration
📋 Payment gateway integration
📋 Email service
📋 BackOffice controllers
📋 Stock reservation logic

## Architecture Pattern Summary

- **DDD**: Domain aggregates, value objects, domain events
- **CQRS**: Separate read/write models via MediatR
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Constructor injection throughout
- **Validation**: FluentValidation + Domain rules
- **Error Handling**: Result<T> pattern
- **Async/Await**: Non-blocking I/O

## Documentation Files

- `README.md` - Complete project documentation
- `IMPLEMENTATION_SUMMARY.md` - Detailed implementation status
- This file - Quick start guide

## Support Resources

1. Check existing handlers: `src/FSI.CloudShopping.Application/Commands/Auth/`
2. Review domain entities: `src/FSI.CloudShopping.Domain/Entities/`
3. Study configurations: `src/FSI.CloudShopping.Infrastructure/Configurations/`
4. Browse DTOs: `src/FSI.CloudShopping.Application/DTOs/`

## Next Immediate Steps

1. ✅ Get the API running (Docker or local)
2. ✅ Test authentication endpoints
3. ✅ Review domain entities to understand the model
4. ✅ Implement remaining command handlers
5. ✅ Implement SAGA for order processing
6. ✅ Integrate payment gateway
7. ✅ Add email notifications
8. ✅ Complete BackOffice controllers

Good luck! The foundation is solid and ready for full implementation. 🚀
