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
- Typed Angular HTTP services and reactive forms
- AG Grid project search interface
- ASP.NET Core / .NET 9 REST APIs
- Entity Framework Core data access
- SQL Server persistence and sample scripts
- Server-side pagination, filtering, and sorting
- Project search and project editing workflows
- DTO-based API contracts
- Dependency injection and service-layer separation
- Client and server validation
- Loading, save, cancel, and API error states
- Automated Angular and .NET tests
- GitHub Actions CI using Node 24 and .NET 9

## Current Reference Features

### Project Search

The Project Search workflow demonstrates a scalable enterprise search pattern:

1. Angular search and AG Grid presentation.
2. A typed `ProjectSearchRequest` contract.
3. `POST /api/projects/search`.
4. A testable `ProjectSearchService` responsible for query composition.
5. EF Core filtering, sorting, and server-side pagination.
6. SQL Server persistence.
7. A typed `PagedResponse<ProjectSummaryDto>` returned to the client.

### Project Editor

Selecting a project from the search grid opens a standalone Angular reactive-form editor. The editor demonstrates:

1. `GET /api/projects/{id}` for loading project details.
2. A typed `ProjectDetailDto` / `ProjectDetail` contract across the API boundary.
3. Required-field and maximum-length validation.
4. Explicit Save and Cancel behavior.
5. `PUT /api/projects/{id}` with a typed `UpdateProjectRequest`.
6. Persistence through a dedicated `ProjectEditorService` and EF Core.
7. Loading, success, validation, and API failure states.
8. Refreshing the search grid after a successful update.

The implementation remains intentionally generic so the architectural patterns can be examined and reused without exposing client-specific business logic.

## Automated Validation

The current CI pipeline restores dependencies, builds both application layers, and executes automated tests on every push and pull request to `main`.

Current coverage includes project-search filtering, pagination and sorting; project-detail loading and persistence; Angular HTTP contracts; editor form loading; required-field validation; Save and Cancel behavior; and load/save failure handling.

## Planned Capabilities

- Customer workflows
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

🚧 **Under active development** — Project Search and Project Editor are implemented and covered by automated CI validation. Additional enterprise capabilities are being added incrementally so architectural decisions and implementation patterns remain easy to follow.

## Author

**Edgar Villegas**  
Senior / Lead Full-Stack Software Engineer

## License

This project is licensed under the MIT License. See `LICENSE` for details.
