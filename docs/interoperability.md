# AxioVital Native — Healthcare Interoperability Strategy

AxioVital Native incorporates modular abstractions for healthcare standards in `AxioVital.Infrastructure.Interoperability`:

## 1. FHIR R4 (`IFhirService`)
- Resource CRUD operations (`ReadAsync`, `CreateAsync`, `UpdateAsync`, `SearchAsync`)
- Schema & profile validation (`ValidateAsync`)
- Configured via `FhirSettings`

## 2. HL7 v2.x (`IHl7Service`)
- Message parsing (`ParseAsync`)
- Message building (`CreateMessageAsync`)
- MLLP transmission & acknowledgment (`SendAsync`)
- Configured via `Hl7Settings`

## 3. DICOM (`IDicomService`)
- Storage & Retrieval (`StoreAsync`, `RetrieveAsync`)
- C-FIND Query (`QueryAsync`)
- Configured via `DicomSettings`
