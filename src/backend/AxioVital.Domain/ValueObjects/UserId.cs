namespace AxioVital.Domain.ValueObjects;

/// <summary>
/// Strongly-typed user identifier.
/// </summary>
public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
    public static UserId From(Guid value) => new(value);
    public static UserId Empty => new(Guid.Empty);

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserId userId) => userId.Value;
    public static explicit operator UserId(Guid value) => new(value);
}
