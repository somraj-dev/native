-- AxioVital Database Seed Script

INSERT INTO tenants (id, name, identifier, is_active, created_at, is_deleted)
VALUES ('00000000-0000-0000-0000-000000000001', 'Default System Tenant', 'default', true, CURRENT_TIMESTAMP, false)
ON CONFLICT (id) DO NOTHING;
