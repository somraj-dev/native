namespace AxioVital.Domain.Entities;

/// <summary>
/// Extends BaseEntity with audit trail fields for tracking creation and modification.
/// All tenant-aware entities should inherit from this class.
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    /// <summary>
    /// UTC timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Identifier of the user who created this entity.
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was last modified.
    /// </summary>
    public DateTime? ModifiedAtUtc { get; set; }

    /// <summary>
    /// Identifier of the user who last modified this entity.
    /// </summary>
    public Guid? ModifiedBy { get; set; }

    /// <summary>
    /// Soft-delete flag. When true, the entity is considered deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was soft-deleted.
    /// </summary>
    public DateTime? DeletedAtUtc { get; set; }
}
