# Testing and Continuous Integration

## Backend Tests

The backend test project uses xUnit and EF Core's in-memory provider to exercise the Project Search application service without requiring a SQL Server instance.

Current tests verify:

- Search text and active-state filtering
- Server-side pagination
- Server-side sorting

Run locally:

```bash
cd server
dotnet test EnterpriseFullStackReference.Api.Tests/EnterpriseFullStackReference.Api.Tests.csproj
```

## Angular Tests

The Angular client includes a focused `ProjectService` test using `HttpTestingController`. It verifies that the typed request is posted to the correct endpoint and that a typed paged response is consumed.

Run locally:

```bash
cd client
npm test
```

## Continuous Integration

`.github/workflows/ci.yml` runs on pushes and pull requests targeting `main`.

The workflow independently validates the two application layers:

```text
.NET API
  -> restore
  -> build
  -> xUnit tests

Angular Client
  -> install
  -> build
  -> Jasmine/Karma tests in headless Chrome
```

A CI failure prevents a change from being considered validated even if it works on one developer workstation.
