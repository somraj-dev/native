# AxioVital Native — Root Directory Architecture & Structure

## Master Root Structure

```text
AxioVital/
├── .github/
│   └── workflows/
│       └── ci-cd.yml
├── axiovital-backend/
│   ├── AxioVital.sln
│   ├── Directory.Build.props
│   ├── Directory.Packages.props
│   ├── Directory.Build.targets
│   ├── AxioVital.Api/
│   ├── AxioVital.Application/
│   ├── AxioVital.Domain/
│   ├── AxioVital.Infrastructure/
│   └── AxioVital.Contracts/
├── axiovital-frontend/
│   └── AxioVital.Desktop/
├── database/
│   ├── migrations/
│   └── scripts/
├── docs/
├── infrastructure/
│   ├── docker/
│   ├── kubernetes/
│   ├── terraform/
│   └── nginx/
├── interoperability/
│   ├── fhir/
│   ├── hl7/
│   └── dicom/
├── packages/
├── security/
├── storage/
├── tests/
│   ├── AxioVital.UnitTests/
│   ├── AxioVital.IntegrationTests/
│   └── AxioVital.ApiTests/
├── tools/
├── .env
├── .env.example
├── .gitignore
├── .prettierrc
├── docker-compose.yml
├── package-lock.json
├── package.json
├── README.md
└── tsconfig.base.json
```
