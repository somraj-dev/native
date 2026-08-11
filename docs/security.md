# AxioVital Native — Security & Multi-Tenancy Architecture

## Authentication

### Password Hashing (Argon2id)
Passwords are hashed using **Argon2id** (`Konscious.Security.Cryptography.Argon2`) with:
- Salt size: 16 bytes
- Hash size: 32 bytes
- Parallelism: 4
- Memory: 64 MB
- Iterations: 3

### JWT Tokens
- Access Token expiration: 30–60 minutes
- Refresh Token expiration: 7–30 days
- Includes claims: `sub` (UserId), `tenant_id` (TenantId), `email`, `role`

### WebAuthn (FIDO2)
- Clean abstraction interface `IWebAuthnService` in `AxioVital.Application`.

## Tenant Isolation & RBAC

```text
User ──► Authentication ──► TenantContext ──► Role ──► Permission ──► Resource
```

Server-side tenant isolation is enforced at the database layer via **EF Core Global Query Filters** in `AxioVitalDbContext` and `TenantResolutionMiddleware`.
