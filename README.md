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
+-- .gitignore
+-- LICENSE
+-- README.md
```

## Planned Capabilities

- Angular standalone application architecture
- ASP.NET Core REST APIs
- Entity Framework Core data access
- SQL Server persistence
- Server-side pagination, filtering, and sorting
- Project and customer search workflows
- DTO-based API contracts
- Dependency injection and service-layer separation
- Validation and consistent API error handling
- Authentication and role-based authorization
- Structured logging
- Automated tests
- PDF/reporting examples
- Optional Electron desktop integration
- AI-assisted engineering documentation and workflow examples

## First Reference Feature

The first end-to-end feature will be **Project Search**.

It will demonstrate a realistic enterprise workflow with:

1. An Angular search/grid interface.
2. A typed paged-search request.
3. An ASP.NET Core API endpoint.
4. A service layer responsible for query composition.
5. EF Core filtering, sorting, and pagination.
6. SQL Server persistence.
7. A typed paged response returned to the client.

The implementation will be intentionally generic so the architectural patterns can be examined and reused without exposing client-specific business logic.

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

🚧 **Under active development** — the repository is being built incrementally so architectural decisions and implementation patterns remain easy to follow.

## Author

**Edgar Villegas**  
Senior / Lead Full-Stack Software Engineer

## License

This project is licensed under the MIT License. See `LICENSE` for details.
