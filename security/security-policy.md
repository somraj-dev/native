# Security Policy & Compliance Matrix

- **Password Hashing**: Argon2id (m=65536, t=3, p=1)
- **Token Signing**: HMAC-SHA256 JWT
- **Multi-Tenancy**: Request-scoped isolation & EF Core Query Filters
- **Compliance**: HIPAA / GDPR Audit Interceptors
