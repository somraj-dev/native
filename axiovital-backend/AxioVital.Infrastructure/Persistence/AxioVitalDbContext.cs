using AxioVital.Domain.Entities;
using AxioVital.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AxioVital.Infrastructure.Persistence;

/// <summary>
/// Main database context for AxioVital with tenant isolation via global query filters.
/// </summary>
public class AxioVitalDbContext : DbContext, IUnitOfWork
{
    private readonly ITenantProvider? _tenantProvider;

    public AxioVitalDbContext(DbContextOptions<AxioVitalDbContext> options) : base(options)
    {
    }

    public AxioVitalDbContext(DbContextOptions<AxioVitalDbContext> options, ITenantProvider tenantProvider) : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    // Core entities
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AxioVitalDbContext).Assembly);

        // Global query filter for tenant isolation on User entity
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted &&
            (_tenantProvider == null || !_tenantProvider.HasTenant || u.TenantId == _tenantProvider.TenantId));

        // Global query filter for soft-delete on tenants
        modelBuilder.Entity<Tenant>().HasQueryFilter(t => !t.IsDeleted);

        // Global query filter for tenant-scoped roles
        modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted &&
            (_tenantProvider == null || !_tenantProvider.HasTenant || r.TenantId == null || r.TenantId == _tenantProvider.TenantId));
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return base.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
        {
            await Database.CommitTransactionAsync(cancellationToken);
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
        {
            await Database.RollbackTransactionAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Automatically sets audit timestamps and tenant ID on tracked entities.
    /// </summary>
    private void ApplyAuditInformation()
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAtUtc = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAtUtc = DateTime.UtcNow;
                    break;
            }
        }
    }
}
