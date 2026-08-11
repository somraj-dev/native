using AxioVital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AxioVital.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");

        builder.Property(t => t.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Identifier)
            .HasColumnName("identifier")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(t => t.Identifier).IsUnique();

        builder.Property(t => t.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(t => t.CreatedAtUtc).HasColumnName("created_at_utc");
        builder.Property(t => t.CreatedBy).HasColumnName("created_by");
        builder.Property(t => t.ModifiedAtUtc).HasColumnName("modified_at_utc");
        builder.Property(t => t.ModifiedBy).HasColumnName("modified_by");
        builder.Property(t => t.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(t => t.DeletedAtUtc).HasColumnName("deleted_at_utc");
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        builder.Property(u => u.TenantId).HasColumnName("tenant_id").IsRequired();
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(320).IsRequired();
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(500).IsRequired();
        builder.Property(u => u.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
        builder.Property(u => u.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(u => u.LastLoginAtUtc).HasColumnName("last_login_at_utc");
        builder.Property(u => u.RefreshTokenHash).HasColumnName("refresh_token_hash").HasMaxLength(500);
        builder.Property(u => u.RefreshTokenExpiryUtc).HasColumnName("refresh_token_expiry_utc");

        builder.Property(u => u.CreatedAtUtc).HasColumnName("created_at_utc");
        builder.Property(u => u.CreatedBy).HasColumnName("created_by");
        builder.Property(u => u.ModifiedAtUtc).HasColumnName("modified_at_utc");
        builder.Property(u => u.ModifiedBy).HasColumnName("modified_by");
        builder.Property(u => u.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(u => u.DeletedAtUtc).HasColumnName("deleted_at_utc");

        builder.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();
        builder.HasIndex(u => u.TenantId);

        builder.HasOne(u => u.Tenant)
            .WithMany(t => t.Users)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");

        builder.Property(r => r.TenantId).HasColumnName("tenant_id");
        builder.Property(r => r.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(r => r.Description).HasColumnName("description").HasMaxLength(500);
        builder.Property(r => r.IsSystemRole).HasColumnName("is_system_role").HasDefaultValue(false);

        builder.Property(r => r.CreatedAtUtc).HasColumnName("created_at_utc");
        builder.Property(r => r.CreatedBy).HasColumnName("created_by");
        builder.Property(r => r.ModifiedAtUtc).HasColumnName("modified_at_utc");
        builder.Property(r => r.ModifiedBy).HasColumnName("modified_by");
        builder.Property(r => r.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(r => r.DeletedAtUtc).HasColumnName("deleted_at_utc");

        builder.HasIndex(r => new { r.TenantId, r.Name }).IsUnique();
    }
}

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasColumnName("description").HasMaxLength(500);
        builder.Property(p => p.Category).HasColumnName("category").HasMaxLength(100);

        builder.HasIndex(p => p.Name).IsUnique();
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");

        builder.HasKey(ur => ur.Id);
        builder.Property(ur => ur.Id).HasColumnName("id");

        builder.Property(ur => ur.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(ur => ur.RoleId).HasColumnName("role_id").IsRequired();
        builder.Property(ur => ur.AssignedAtUtc).HasColumnName("assigned_at_utc");
        builder.Property(ur => ur.AssignedBy).HasColumnName("assigned_by");

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();

        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(rp => rp.Id);
        builder.Property(rp => rp.Id).HasColumnName("id");

        builder.Property(rp => rp.RoleId).HasColumnName("role_id").IsRequired();
        builder.Property(rp => rp.PermissionId).HasColumnName("permission_id").IsRequired();

        builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();

        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
