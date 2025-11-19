# eAppointment Server - AI Agent Instructions

## Project Overview

This is an **ASP.NET Core appointment management system** built with **.NET 9.0** using **Domain-Driven Design (DDD)**, **CQRS with MediatR**, and **Clean Architecture**. The system manages doctors, patients, and appointments with ASP.NET Core Identity for authentication.

**Complete Project Structure:**
```
src/
├── eAppointmentServer.Domain/          # Core business entities & interfaces
│   ├── Entities/                       # Aggregates (Doctor, Patient, Appointment, AppUser)
│   ├── Repositories/                   # Repository interfaces
│   └── Enums/                          # SmartEnums (e.g., DepartmentEnum)
├── eAppointmentServer.Application/     # Use cases & business logic
│   ├── Features/Auth/                  # Feature-based organization (CQRS commands/queries)
│   └── Services/                       # Application service interfaces (e.g., IJwtProvider)
├── eAppointmentServer.Infrastructure/  # Data access & external services
│   ├── Context/                        # EF Core DbContext
│   ├── Configurations/                 # EF Core entity configurations
│   ├── Repositories/                   # Repository implementations
│   └── Services/                       # Service implementations (e.g., JwtProvider)
└── eAppointmentServer.WebAPI/          # HTTP API layer (minimal setup currently)
```

**Key Dependencies:**
- `TS.MediatR` (9.0.6) - CQRS pattern for commands/queries
- `ErginWebDev.Result` (1.0.1) - Railway-oriented programming for error handling
- `TS.EntityFrameworkCore.GenericRepository` (9.0.4) - Repository pattern implementation
- `Ardalis.SmartEnum` (8.2.0) - Type-safe enums with behavior

## Critical Architecture Patterns

### 1. Domain Entity Design (DDD Tactical Patterns)

All domain entities follow a **rich domain model** with encapsulation:

```csharp
// ✅ CORRECT - Encapsulated entity from Patient.cs
public sealed class Patient
{
    public Patient(FirstName firstName, LastName lastName, IdentityNumber identityNumber, Address address)
    {
        Id = new PatientId(Guid.CreateVersion7());
        SetFirstName(firstName);
        SetLastName(lastName);
        SetIdentityNumber(identityNumber);
        SetAddress(address);
    }

    public PatientId Id { get; private set; }
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public string FullName => $"{FirstName.Value} {LastName.Value}";
    
    private void SetFirstName(FirstName firstName) => FirstName = firstName;
    private void SetLastName(LastName lastName) => LastName = lastName;
}
```

**Entity Rules:**
- `sealed class` - Prevent inheritance (all entities are sealed)
- **Private setters** - Enforce encapsulation (no `set` from outside)
- **Constructor initialization** - Use `Guid.CreateVersion7()` for IDs (better DB indexing)
- **Private setter methods** - Centralize validation logic (e.g., `SetFirstName()`)
- **Computed properties** - Use `=>` for derived values like `FullName`

**Identity Entities Exception:**
`AppUser` and `AppRole` inherit from `IdentityUser<Guid>` / `IdentityRole<Guid>` for ASP.NET Core Identity integration, but still follow encapsulation.

### 2. Value Objects Pattern

Use **readonly record struct** for value objects - minimal, no base class needed:

```csharp
// Simple value objects (Entities/Shared/ValueObjects/)
public readonly record struct FirstName(string Value);
public readonly record struct LastName(string Value);

// Entity IDs (Entities/Patient/ValueObjects/PatientId.cs)
public readonly record struct PatientId(Guid Value);

// Composite value objects (Entities/Patient/ValueObjects/Address.cs)
public readonly record struct Address(
    City City,
    Town Town,
    FullAddress FullAddress);
```

**Value Object Organization:**
- **Shared value objects** → `Entities/Shared/ValueObjects/` (FirstName, LastName used by multiple entities)
- **Entity-specific value objects** → `Entities/<EntityName>/ValueObjects/` (PatientId, DoctorId, Address)
- **NO `using` statements or inheritance** - Convention-based detection only
- Access underlying value via `.Value` property (e.g., `firstName.Value`)

### 3. SmartEnum Pattern (Not Regular Enums)

Use **Ardalis.SmartEnum** for type-safe, behavior-rich enumerations:

```csharp
// From Domain/Enums/DepartmentEnum.cs
public sealed class DepartmentEnum : SmartEnum<DepartmentEnum>
{
    public static readonly DepartmentEnum Acil = new("Acil", 1);
    public static readonly DepartmentEnum Cocuk = new("Cocuk", 2);
    public static readonly DepartmentEnum Dahiliye = new("Dahiliye", 3);
    
    public DepartmentEnum(string name, int value) : base(name, value) { }
}

// Usage in entities
public DepartmentId Department { get; private set; }  // DepartmentId is a value object wrapping int
```

**Why SmartEnum?**
- String names for display (`DepartmentEnum.Acil.Name` → "Acil")
- Integer values for storage (`DepartmentEnum.Acil.Value` → 1)
- Type-safe comparisons (`dept == DepartmentEnum.Kardioloji`)
- Can add methods/behavior to enum types

### 4. CQRS with MediatR (Feature-Based Organization)

Commands/queries organized by feature in `Application/Features/<Feature>/`:

```csharp
// From Application/Features/Auth/LoginCommand.cs
public sealed record LoginCommand(
    string UsernameOrEmail,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse(string Token);

internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    IJwtProvider jwtProvider
) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(u =>
            u.UserName == request.UsernameOrEmail ||
            u.Email == request.UsernameOrEmail, cancellationToken);
        
        if (user is null)
            return Result<LoginCommandResponse>.Fail("Invalid username or password.", statusCode: System.Net.HttpStatusCode.NotFound);
        
        bool isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            return Result<LoginCommandResponse>.Fail("Invalid username or password.");
        
        var token = jwtProvider.GenerateToken(user);
        return Result<LoginCommandResponse>.Success(new LoginCommandResponse(Token: token));
    }
}
```

**CQRS Conventions:**
- **Commands** - `<Action>Command` (e.g., `CreatePatientCommand`, `UpdateDoctorCommand`)
- **Queries** - `<Entity>Query` (e.g., `GetDoctorByIdQuery`, `GetAllPatientsQuery`)
- **Handlers** - `internal sealed class` in same file as command/query
- **Primary constructor** - Use C# 12 syntax for dependency injection
- **Return type** - Always `Result<T>` for error handling (railway-oriented programming)

### 5. Result Pattern (Railway-Oriented Programming)

Use `ErginWebDev.Result` for explicit error handling:

```csharp
// ✅ Success case
return Result<LoginCommandResponse>.Success(new LoginCommandResponse(Token: token));

// ✅ Failure with message
return Result<LoginCommandResponse>.Fail("Invalid username or password.");

// ✅ Failure with status code
return Result<LoginCommandResponse>.Fail("Invalid username or password.", statusCode: System.Net.HttpStatusCode.NotFound);
```

**Benefits:**
- No exceptions for business logic failures
- Explicit success/failure states
- HTTP status code mapping built-in
- Forces error handling at call sites

### 6. Repository Pattern with Generic Repository

Define interfaces in Domain, implement in Infrastructure:

```csharp
// Domain/Repositories/IPatientRepository.cs
public interface IPatientRepository : IRepository<Patient> { }

// Infrastructure/Repositories/PatientRepository.cs
internal sealed class PatientRepository : Repository<Patient, ApplicationDbContext>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context) : base(context) { }
}
```

**Repository Conventions:**
- Interface in `Domain/Repositories/` (public)
- Implementation in `Infrastructure/Repositories/` (`internal sealed`)
- Inherit from `Repository<TEntity, TContext>` from `TS.EntityFrameworkCore.GenericRepository`
- Add custom methods to interface when generic methods insufficient
- Use `IUnitOfWork` (implemented by DbContext) for transactions

## Dependency Injection & Layering

Each layer has a `DependencyInjection.cs` extension method:

```csharp
// Application/DependencyInjection.cs
public static IServiceCollection AddApplicationDI(this IServiceCollection services)
{
    services.AddMediatR(configuration =>
    {
        configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
    });
    return services;
}

// Infrastructure/DependencyInjection.cs
public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    
    services.AddIdentity<AppUser, AppRole>(action =>
    {
        action.Password.RequireDigit = true;
        action.Password.RequireLowercase = true;
        action.Password.RequireUppercase = true;
        action.Password.RequireNonAlphanumeric = false;
        action.Password.RequiredLength = 6;
    }).AddEntityFrameworkStores<ApplicationDbContext>();
    
    services.AddScoped<IPatientRepository, PatientRepository>();
    services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
    services.AddScoped<IJwtProvider, JwtProvider>();
    
    return services;
}
```

**Layering Rules:**
- **Domain** - No dependencies on other layers, only third-party libraries
- **Application** - References Domain only, defines service interfaces
- **Infrastructure** - References Application & Domain, implements interfaces
- **WebAPI** - References all layers for composition root

## EF Core Configuration Patterns

Entity configurations in `Infrastructure/Configurations/`:

```csharp
// From Configurations/DoctorConfiguration.cs
public sealed class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.Property(d => d.FirstName).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(d => d.LastName).HasColumnType("nvarchar(100)").IsRequired();
    }
}
```

**DbContext Setup:**
```csharp
// From Context/ApplicationDbContext.cs
internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IUnitOfWork
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Ignore unused Identity tables
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
        
        // Auto-apply configurations from assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
```

**Key Points:**
- Value objects automatically convert via implicit operators (no manual configuration needed)
- `internal sealed class` - Infrastructure types never exposed
- Implement `IUnitOfWork` from generic repository library
- Use `ApplyConfigurationsFromAssembly()` to auto-discover configurations

## Project Configuration

### Build Settings (All Projects)
- **Target Framework**: .NET 9.0
- **Nullable**: Enabled - All reference types are non-nullable by default
- **Implicit Usings**: Enabled - Common namespaces auto-imported
- **TreatWarningsAsErrors**: `true` - Must fix all compiler warnings

## Development Workflows

### Building the Solution
```bash
# Build entire solution
dotnet build src/eAppointmentServer.sln

# Build specific project
dotnet build src/eAppointmentServer.Domain/eAppointmentServer.Domain.csproj
```

### Adding New Domain Entities

**1. Create Entity Structure** in `Domain/Entities/<EntityName>/`:
```
Entities/
└── Order/
    ├── Order.cs
    └── ValueObjects/
        ├── OrderId.cs
        └── OrderNumber.cs
```

**2. Define Value Objects** (minimal syntax):
```csharp
// Entities/Order/ValueObjects/OrderId.cs
namespace eAppointmentServer.Domain.Entities.Order.ValueObjects;

public readonly record struct OrderId(Guid Value);
```

**3. Create Encapsulated Entity**:
```csharp
// Entities/Order/Order.cs
namespace eAppointmentServer.Domain.Entities.Order;

public sealed class Order
{
    public Order(OrderNumber orderNumber, PatientId patientId)
    {
        Id = new OrderId(Guid.CreateVersion7());
        SetOrderNumber(orderNumber);
        SetPatientId(patientId);
    }
    
    public OrderId Id { get; private set; }
    public OrderNumber OrderNumber { get; private set; }
    public PatientId PatientId { get; private set; }
    
    private void SetOrderNumber(OrderNumber orderNumber) => OrderNumber = orderNumber;
    private void SetPatientId(PatientId patientId) => PatientId = patientId;
}
```

**4. Create Repository Interface**:
```csharp
// Domain/Repositories/IOrderRepository.cs
public interface IOrderRepository : IRepository<Order> { }
```

**5. Implement Repository**:
```csharp
// Infrastructure/Repositories/OrderRepository.cs
internal sealed class OrderRepository : Repository<Order, ApplicationDbContext>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context) { }
}
```

**6. Add EF Core Configuration** (optional, for custom mappings):
```csharp
// Infrastructure/Configurations/OrderConfiguration.cs
public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.OrderNumber).HasColumnType("nvarchar(50)").IsRequired();
    }
}
```

**7. Register in DI** (`Infrastructure/DependencyInjection.cs`):
```csharp
services.AddScoped<IOrderRepository, OrderRepository>();
```

### Adding CQRS Commands/Queries

**1. Create Command** in `Application/Features/<Feature>/`:
```csharp
// Application/Features/Orders/CreateOrderCommand.cs
public sealed record CreateOrderCommand(
    OrderNumber OrderNumber,
    PatientId PatientId) : IRequest<Result<CreateOrderResponse>>;

public sealed record CreateOrderResponse(OrderId Id);

internal sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateOrderCommand, Result<CreateOrderResponse>>
{
    public async Task<Result<CreateOrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = new(request.OrderNumber, request.PatientId);
        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<CreateOrderResponse>.Success(new CreateOrderResponse(order.Id));
    }
}
```

**2. Use in Controller/Endpoint** (WebAPI layer):
```csharp
app.MapPost("/orders", async (CreateOrderCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return result.IsSuccessful ? Results.Ok(result.Data) : Results.BadRequest(result.ErrorMessages);
});
```

## Naming Conventions

- **Project Names**: `eAppointmentServer.<LayerName>` (camelCase prefix)
- **Namespaces**: Follow project structure: `eAppointmentServer.Domain.Entities`
- **Entities**: PascalCase, typically noun-based (`AppUser`, not `User` to avoid conflicts)
- **Commands/Queries**: `<Action>Command` / `<Entity>Query` (e.g., `CreatePatientCommand`, `GetDoctorQuery`)
- **Handlers**: `internal sealed class` in same file as command/query

## Important Notes

- **Zero tolerance for warnings** - Must fix all compiler warnings due to `TreatWarningsAsErrors` setting
- **Database storage** - Value objects stored as underlying types (e.g., `PatientId` → `uniqueidentifier`)
- **Guid generation** - Use `Guid.CreateVersion7()` in .NET 9 for better database performance (time-ordered)
- **No public setters** - Domain entities enforce encapsulation with private setters and setter methods
- **Internal infrastructure** - All Infrastructure implementations are `internal sealed` (never exposed)
- **Railway-oriented error handling** - Use `Result<T>` pattern, not exceptions for business logic failures
