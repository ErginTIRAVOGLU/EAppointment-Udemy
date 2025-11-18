# eAppointment Server - AI Agent Instructions

## Project Overview

This is an **ASP.NET Core appointment management system** built with **.NET 9.0** using a Domain-Driven Design (DDD) approach. The project is currently in its early stages with only the Domain layer implemented.

**Key Project Structure:**
- `src/eAppointmentServer.Domain/` - Domain entities and core business models
  - `Entities/AppUser/` - User aggregate with ASP.NET Core Identity
    - `AppUser.cs` - User entity
    - `ValueObjects/FirstName.cs`, `LastName.cs` - User-specific value objects
  - `Entities/Patient/` - Patient aggregate
    - `Patient.cs` - Patient entity
    - `ValueObjects/PatientId.cs`, `FirstName.cs`, `LastName.cs` - Patient-specific types
- `src/StronglyTypedIds-main/` - Local project reference to the StronglyTypedIds library (for development)
- Solution follows a modular architecture (DDD layers expected: Domain, Application, Infrastructure, WebAPI)

**Organization Pattern**: Each aggregate has its own folder with nested `ValueObjects/` subdirectory for strongly-typed IDs and value objects

## Critical Architecture Patterns

### 1. Strongly-Typed IDs Pattern
**This project uses the `ErginWebDev.StronglyTypedIds` library for type-safe entity identifiers.**

#### Why Strongly-Typed IDs?
- ✅ **Type Safety**: Prevents mixing different entity IDs at compile-time
- ✅ **Code Clarity**: `FindCustomer(CustomerId id)` is clearer than `FindCustomer(Guid id)`
- ✅ **DDD Compliance**: Follows Domain-Driven Design value object pattern
- ✅ **Zero Boilerplate**: Automatic EF Core, JSON, and OpenAPI integration

#### Defining Strongly-Typed IDs
Use simple `readonly record struct` with a `Value` property:

```csharp
// Guid-based IDs (recommended for entities)
public readonly record struct CustomerId(Guid Value);
public readonly record struct OrderId(Guid Value);

// Other value types for domain-specific properties (see FirstName.cs, LastName.cs)
public readonly record struct FirstName(string Value);
public readonly record struct OrderNumber(int Value);
public readonly record struct Price(decimal Value);
```

> **CRITICAL**: Do NOT inherit from `StronglyTypedId<T>` or add any `using` statements for ValueObjects. C# record structs cannot inherit from other record structs. The library uses convention-based detection (any `readonly record struct` with a `Value` property). Keep definitions minimal - see `FirstName.cs` for the correct pattern.

#### Usage in Entities
```csharp
// ✅ CORRECT - Using strongly-typed IDs
public sealed class AppUser : IdentityUser<Guid>
{
    public FirstName FirstName { get; set; } = default!;
    public LastName LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
}

// ❌ WRONG - Don't use primitive types for domain values
public string FirstName { get; set; } = default!;
```

#### Supported Value Types
- `Guid` - Entity identifiers (use `Guid.CreateVersion7()` in .NET 9 for better DB performance)
- `int`, `long` - Numeric IDs or counters
- `string` - Codes, names, or text-based identifiers
- `decimal`, `double` - Financial or measurement values
- `DateTime`, `DateTimeOffset` - Temporal values
- `Enum` - Status or category types

### 2. ASP.NET Core Identity Integration
- Entities inherit from `IdentityUser<Guid>` for authentication/authorization
- Primary keys use `Guid` instead of default `string` keys
- Located in `eAppointmentServer.Domain.Entities` namespace

### 3. Entity Conventions
- **Sealed classes**: Domain entities are `sealed` to prevent inheritance
- **Non-nullable by default**: Project uses `<Nullable>enable</Nullable>`
- **Null-forgiving operator**: Use `= default!;` for required properties that will be initialized by EF Core
- **Computed properties**: Use expression-bodied members (e.g., `FullName => $"{FirstName} {LastName}"`)

## Project Configuration

### Build Settings
- **Target Framework**: .NET 9.0
- **Implicit Usings**: Enabled (common namespaces automatically imported)
- **TreatWarningsAsErrors**: `true` - All warnings must be resolved before building

### Key Dependencies
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (9.0.11) - Identity & EF Core integration
- `ErginWebDev.StronglyTypedIds` (1.0.5) - Custom library for strongly-typed identifiers

## Development Workflows

### Building the Solution
```powershell
# Build from solution root
dotnet build src/eAppointmentServer.sln

# Build specific project
dotnet build src/eAppointmentServer.Domain/eAppointmentServer.Domain.csproj
```

### Adding New Entities
1. **Create aggregate folder structure** in `eAppointmentServer.Domain/Entities/<EntityName>/`:
   ```
   Entities/
   └── Customer/
       ├── Customer.cs
       └── ValueObjects/
           ├── CustomerId.cs
           ├── FirstName.cs
           └── LastName.cs
   ```

2. **Define strongly-typed IDs** in `ValueObjects/` subdirectory:
   ```csharp
   // Entities/Customer/ValueObjects/CustomerId.cs
   namespace eAppointmentServer.Domain.Entities.Customer.ValueObjects;
   
   public readonly record struct CustomerId(Guid Value);
   ```
   **Important**: Do NOT add `using` statements for the StronglyTypedIds library. Keep definitions minimal.

3. **Create entity class** following the pattern in `AppUser.cs`:
   ```csharp
   // Entities/Customer/Customer.cs
   using eAppointmentServer.Domain.Entities.Customer.ValueObjects;
   using Microsoft.AspNetCore.Identity;
   
   namespace eAppointmentServer.Domain.Entities.Customer;
   
   public sealed class Customer : IdentityUser<Guid>
   {
       public FirstName FirstName { get; set; } = default!;
       public LastName LastName { get; set; } = default!;
       public string FullName => $"{FirstName} {LastName}";
   }
   ```

4. **For non-Identity entities**, use constructor initialization:
   ```csharp
   public sealed class Order
   {
       public Order()
       {
           Id = new OrderId(Guid.NewGuid());
       }
       
       public OrderId Id { get; set; }
       public string OrderNumber { get; set; } = default!;
       // Other required properties should use = default! or be initialized in constructor
   }
   ```

3. **Configure EF Core** (when Infrastructure layer is added):
   ```csharp
   protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
   {
       configurationBuilder.ConfigureStronglyTypedIds(); // Automatic conversion
   }
   ```

4. **Use in queries** - Works seamlessly with EF Core:
   ```csharp
   var customer = await context.Customers.FindAsync(customerId);
   var orders = await context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
   ```

### Future Architecture (Expected Layers)
Based on naming conventions, expect these layers to be added:
- `eAppointmentServer.Application` - Use cases, commands, queries (CQRS likely)
- `eAppointmentServer.Infrastructure` - Data access, external services
- `eAppointmentServer.WebAPI` - HTTP endpoints, controllers

## Naming Conventions

- **Project Names**: `eAppointmentServer.<LayerName>` (camelCase prefix)
- **Namespaces**: Follow project structure: `eAppointmentServer.Domain.Entities`
- **Entities**: PascalCase, typically noun-based (`AppUser`, not `User` to avoid conflicts)

## Important Notes

- **Early stage project** - Only Domain layer exists; expect architectural expansion
- **Zero tolerance for warnings** - Must fix all compiler warnings due to `TreatWarningsAsErrors` setting
- **Database storage** - Strongly-typed IDs are stored as their underlying value type (e.g., `CustomerId` → `uniqueidentifier`)
- **JSON serialization** - Automatic conversion to/from simple values (no nested objects)
- **Guid generation** - Use `Guid.CreateVersion7()` in .NET 9 for better database performance (time-ordered)

## Resources

- **StronglyTypedIds Documentation**: https://www.nuget.org/packages/ErginWebDev.StronglyTypedIds
- **Library Features**: Automatic EF Core converters, JSON serialization, OpenAPI/Swagger integration
- **Performance**: Zero runtime reflection overhead (only at startup)
