# AxioVital Native — Database & EF Core Documentation

## Provider & Database

- Database: **PostgreSQL 16**
- ORM: **Entity Framework Core 9** (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- Naming convention: `snake_case` column and table names.

## Entities
- `tenants`: Organization records
- `users`: User accounts bound to a tenant
- `roles`: Role definitions (system or tenant-scoped)
- `permissions`: Fine-grained permission definitions
- `user_roles`: User to Role mapping
- `role_permissions`: Role to Permission mapping

## Query Filters & Auditing
- Soft delete (`IsDeleted`) filter applied automatically.
- Multi-tenancy (`TenantId`) filter automatically scoped to `ITenantProvider`.
- Automatic audit fields (`CreatedAtUtc`, `ModifiedAtUtc`) updated on `SaveChangesAsync`.
