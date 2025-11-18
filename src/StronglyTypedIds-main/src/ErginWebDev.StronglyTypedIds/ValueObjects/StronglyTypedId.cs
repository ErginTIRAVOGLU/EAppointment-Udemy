namespace ErginWebDev.StronglyTypedIds.ValueObjects;

/// <summary>
/// Base record struct for strongly typed IDs with generic value type support.
/// Supported types: Guid, int, long, string, decimal, double, DateTime, DateTimeOffset, Enum
/// </summary>
/// <typeparam name="TValue">The underlying value type for the ID</typeparam>
/// <remarks>
/// This is a marker type that enables convention-based detection by the library.
/// To create strongly typed IDs, simply define a record struct with a Value property:
/// <code>
/// public readonly record struct CustomerId(Guid Value);
/// public readonly record struct OrderNumber(int Value);
/// public readonly record struct ProductCode(string Value);
/// </code>
/// No inheritance or interface implementation is required. The library automatically detects
/// and configures types that follow this pattern.
/// </remarks>
public readonly record struct StronglyTypedId<TValue>(TValue Value)
{
    public override string ToString() => Value?.ToString() ?? string.Empty;
}

/// <summary>
/// Optional specialized Guid-based strongly typed ID with factory methods.
/// This type provides convenience methods for Guid generation but is NOT required for usage.
/// </summary>
/// <remarks>
/// You can use this type directly or create your own Guid-based IDs:
/// <code>
/// // Option 1: Use this built-in type (optional)
/// var id = StronglyTypedId.NewId();
/// 
/// // Option 2: Create your own (recommended for domain clarity)
/// public readonly record struct CustomerId(Guid Value);
/// var customerId = new CustomerId(Guid.CreateVersion7()); // .NET 9+
/// var customerId = new CustomerId(Guid.NewGuid());        // .NET 8+
/// </code>
/// </remarks>
public readonly record struct StronglyTypedId(Guid Value) : IEquatable<StronglyTypedId>
{
#if NET9_0_OR_GREATER
    /// <summary>
    /// Creates a new ID using Guid.CreateVersion7() (time-ordered, recommended for database performance).
    /// Available in .NET 9.0 and later.
    /// </summary>
    public static StronglyTypedId NewId() => new(Guid.CreateVersion7());
#else
    /// <summary>
    /// Creates a new ID using Guid.NewGuid() (random).
    /// Note: Guid.CreateVersion7() is only available in .NET 9.0+. This method uses Guid.NewGuid() for .NET 8.0.
    /// </summary>
    public static StronglyTypedId NewId() => new(Guid.NewGuid());
#endif
    
    /// <summary>
    /// Creates a new ID using Guid.NewGuid() (random, traditional approach)
    /// </summary>
    public static StronglyTypedId NewGuid() => new(Guid.NewGuid());
    
    /// <summary>
    /// Empty Guid value
    /// </summary>
    public static StronglyTypedId Empty => new(Guid.Empty);
    
    public override string ToString() => Value.ToString();
}
