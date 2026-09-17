# Enterprise Full-Stack Reference

A portfolio-quality reference application demonstrating patterns used to design, modernize, and deliver enterprise business software with **Angular, TypeScript, ASP.NET Core, Entity Framework Core, and SQL Server**.

> This repository is an independent reference implementation. It contains no proprietary client source code, credentials, production data, internal hostnames, or confidential database schemas.

## Purpose

The project demonstrates how a modern enterprise application can be organized across the full stack while keeping architecture, maintainability, security, testability, and clear separation of concerns at the center of the implementation.

The sample domain is a generic **project operations system** containing projects, customers, services, users, and statuses.

## Architecture

```text
Angular / TypeScript Client
          |
          | HTTPS / JSON
          v
ASP.NET Core REST API
          |
          v
Application / Service Layer
          |
          v
Entity Framework Core
          |
          v
Microsoft SQL Server
```

## Repository Structure

```text
enterprise-fullstack-reference/
|
+-- client/       Angular / TypeScript application
+-- server/       ASP.NET Core REST API
+-- database/     SQL Server schema and sample scripts
+-- docs/         Architecture and engineering documentation
+-- samples/      Example API requests and sanitized sample data
+-- .github/      CI workflow
+-- .gitignore
+-- LICENSE
+-- README.md
```

## Implemented Capabilities

- Angular 19 standalone application architecture
- Routed Projects and Customers workspaces
- Typed Angular HTTP services and reactive forms
- AG Grid project and customer search interfaces
- ASP.NET Core / .NET 9 REST APIs
- Entity Framework Core data access
- SQL Server persistence and sanitized sample scripts
- Server-side pagination, filtering, and sorting
- Project search and project editing workflows
- Customer search and customer editing workflows
- DTO-based API contracts
- Dependency injection and service-layer separation
- Client and server validation
- Loading, save, cancel, and API error states
- Automated Angular and .NET tests
- GitHub Actions CI using Node 24 and .NET 9

## Current Reference Features

### Application Navigation

The Angular application shell separates the reference workflows into routed workspaces:

- `/projects` — project search, selection, editing, save, and refresh.
- `/customers` — customer search, selection, editing, save, and refresh.

The root route and unknown routes redirect to Projects. Active navigation state makes the current workspace explicit while keeping feature components isolated from the application shell.

### Project Search and Editor

The Project workflow demonstrates a scalable enterprise search-and-edit pattern:

1. Angular search and AG Grid presentation.
2. Typed `ProjectSearchRequest` and paged response contracts.
3. `POST /api/projects/search` with EF Core filtering, sorting, and server-side pagination.
4. Row selection loads `GET /api/projects/{id}`.
5. A standalone reactive-form editor validates editable fields.
6. `PUT /api/projects/{id}` persists a typed `UpdateProjectRequest` through `ProjectEditorService`.
7. Save success refreshes the search grid; failed saves preserve the user's edits.

### Customer Search and Editor

The Customer workflow applies the same architectural pattern to a second domain area rather than coupling customer behavior to the Project feature:

1. Free-text customer search across customer number, company, contact, email, and city.
2. State and active/inactive filtering.
3. Server-side sorting and pagination through `POST /api/customers/search`.
4. AG Grid row selection loads `GET /api/customers/{id}`.
5. A standalone Customer Editor uses Angular Reactive Forms for Company, Contact, Email, Phone, City, State, and Active status.
6. Client and API validation protect the update boundary.
7. `PUT /api/customers/{id}` persists changes through a dedicated `CustomerEditorService`.
8. Save and Cancel behavior, loading/error states, and automatic grid refresh mirror the Project workflow.

The implementation remains intentionally generic so the architectural patterns can be examined and reused without exposing client-specific business logic.

## Automated Validation

The CI pipeline restores dependencies, builds both application layers, and executes automated tests on every push and pull request to `main`.

Current coverage includes project and customer search filtering, pagination and sorting; detail loading and persistence; Angular HTTP contracts; reactive-form loading and validation; Save and Cancel behavior; not-found handling; and load/save failure handling.

## Planned Capabilities

- Authentication and role-based authorization
- Structured logging and centralized API error handling
- Additional integration and end-to-end testing
- PDF/reporting examples
- Optional Electron desktop integration
- AI-assisted engineering documentation and workflow examples

## Engineering Principles

- Keep API contracts explicit and typed.
- Separate presentation, application, and persistence concerns.
- Perform pagination and filtering server-side for scalable datasets.
- Prefer readable, maintainable code over unnecessary abstraction.
- Treat security and validation as architectural concerns.
- Keep configuration and secrets outside source control.
- Use AI as an engineering accelerator while retaining human review and accountability.

## AI-Assisted Development

AI may be used during development for architecture analysis, prototyping, debugging, refactoring, test generation, documentation, and technical exploration.

AI-generated or AI-assisted changes remain subject to normal engineering review. Correctness, security, maintainability, and architectural decisions remain human responsibilities.

## Technology Direction

**Frontend:** Angular, TypeScript, HTML, SCSS, AG Grid

**Backend:** C#, .NET, ASP.NET Core, Entity Framework Core, LINQ

**Data:** Microsoft SQL Server, T-SQL

**Desktop / Integration:** Electron (optional reference module)

**Engineering:** REST API design, authentication/authorization, reporting, testing, CI/CD, AI-assisted development

## Status

🚧 **Under active development** — Projects and Customers now have complete routed Search → Select → Load → Edit → Save → Refresh workflows covered by automated CI validation. Additional enterprise capabilities are being added incrementally so architectural decisions and implementation patterns remain easy to follow.

## Author

**Edgar Villegas**  
Senior / Lead Full-Stack Software Engineer

## License

This project is licensed under the MIT License. See `LICENSE` for details.
