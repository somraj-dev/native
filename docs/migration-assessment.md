# AxioVital Native — Legacy to New Architecture Assessment

## Legacy Overview (`c:\Users\HP\website`)
- **Framework**: Next.js 16 + React 19 + TailwindCSS.
- **Backend**: None (pure frontend website).
- **Database**: None.
- **Auth**: None.

## Architecture Mapping

```text
Legacy Next.js Marketing / Web Portal (c:\Users\HP\website)
        │ (Preserved as public web portal / marketing)
        ▼
New AxioVital Native Desktop Application (c:\Users\HP\native\src\AxioVital.Desktop)
        │ (WinUI 3 + XAML + MVVM)
        ▼
New ASP.NET Core 9 Web API (c:\Users\HP\native\src\AxioVital.Api)
        │
        ▼
New PostgreSQL 16 + Redis + Redpanda + MinIO Infrastructure
```

## Preserved Business Domain Concepts
The following entities and domain models identified in `AXIOVITAL_ENTITY_MAP.md` and `src/app/profile/page.tsx` will inform Phase 2 domain entities:
- Patient Profiles & Medical Histories
- Care Provider & Assistant Doctor assignments
- Appointment Scheduling & Room Allocations
- Prescriptions & Preferred Pharmacy lists
- Diagnostic & Laboratory Reports
- Hardware Credentials (AXIO-ID Token, AXIO Smart Card)
