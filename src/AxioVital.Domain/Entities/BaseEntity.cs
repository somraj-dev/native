namespace AxioVital.Domain.Entities;

/// <summary>
/// Base entity providing identity for all domain entities.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
