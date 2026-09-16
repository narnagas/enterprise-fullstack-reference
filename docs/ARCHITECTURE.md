# Architecture

## Overview

Enterprise Full-Stack Reference uses a layered architecture intended to demonstrate practical patterns for business applications without depending on proprietary client code.

```text
Angular Client
    |
    | HTTPS / JSON
    v
ASP.NET Core API
    |
    v
Application Services
    |
    v
Entity Framework Core
    |
    v
SQL Server
```

## Client

The `client` directory contains a standalone Angular 19 application responsible for presentation, typed API communication, search/filter state, reactive-form validation, and user workflows.

The client does not contain database or persistence logic. API contracts are represented by TypeScript interfaces corresponding to backend DTOs. Project Search uses AG Grid for tabular results, while Project Editor uses Angular Reactive Forms for explicit load, edit, Save, and Cancel behavior.

## Server

The `server` directory contains the ASP.NET Core / .NET 9 Web API.

Primary responsibilities include:

- HTTP endpoints and API contracts
- Request validation
- Application/service orchestration
- Query composition
- Data access through Entity Framework Core
- Persistence of project updates
- Extensible boundaries for authentication, authorization, logging, and centralized error handling

Controllers remain thin. Search and editor behavior is delegated to `ProjectSearchService` and `ProjectEditorService`, keeping query and persistence logic testable outside controller actions.

## Data

SQL Server is the persistence layer. Entity Framework Core provides application data access while T-SQL scripts document schema, indexes, and sanitized sample data.

Project Search performs filtering, sorting, counting, and pagination on the server so large result sets are not unnecessarily transferred to the browser. Project Editor loads a single project using a no-tracking DTO projection and uses a tracked entity for updates before calling `SaveChangesAsync`.

## Domain

The sample domain models a generic project operations system. The current implemented entity is `Project`, with the architecture designed to expand to concepts such as:

- Customer
- Service
- ProjectStatus
- ApplicationUser

These concepts are intentionally generic and are not copies of a client database schema.

## Implemented End-to-End Flows

### Project Search

```text
Search UI / AG Grid
   -> ProjectSearchRequest
   -> POST /api/projects/search
   -> ProjectsController
   -> ProjectSearchService
   -> IQueryable<Project>
   -> Filter / Sort / Count / Page
   -> PagedResponse<ProjectSummaryDto>
   -> ProjectService
   -> Angular Grid
```

Search state remains in the Angular component while scalable filtering, sorting, and pagination are performed by the API/data layer.

### Project Editor - Load

```text
Grid Row Selection
   -> selectedProjectId
   -> ProjectEditorComponent
   -> ProjectService.getById(id)
   -> GET /api/projects/{id}
   -> ProjectsController
   -> ProjectEditorService.GetByIdAsync
   -> AsNoTracking + ProjectDetailDto projection
   -> Reactive Form
```

The editor exposes editable fields through a reactive form while project identity and creation information remain display-only.

### Project Editor - Save

```text
Reactive Form
   -> Client Validation
   -> UpdateProjectRequest
   -> ProjectService.update(id, request)
   -> PUT /api/projects/{id}
   -> ProjectsController
   -> ProjectEditorService.UpdateAsync
   -> Tracked Project Entity
   -> SaveChangesAsync
   -> ProjectDetailDto
   -> Editor Success State
   -> Refresh Search Grid
```

Cancel resets the form from the last successfully loaded or saved project. Failed saves preserve the user's current edits and expose an error state rather than silently discarding changes.

## Validation and Error Handling

Validation exists at both application boundaries. Angular validates required fields and maximum lengths before issuing an update request. ASP.NET Core validates the `UpdateProjectRequest` data annotations before the controller action proceeds.

The client handles project-load and project-save failures explicitly. A missing project returns HTTP 404 from the API. Broader centralized API exception handling and structured logging remain planned capabilities.

## Automated Testing and CI

The repository uses GitHub Actions to build and test the Angular and .NET applications on pushes and pull requests to `main`.

Current automated coverage includes:

- Search filtering and active-state filtering
- Server-side pagination
- Server-side sorting
- Project-detail retrieval and not-found behavior
- Project update persistence and not-found behavior
- Angular search HTTP contract
- Angular project-detail GET contract
- Angular project-update PUT contract
- Editor loading and form population
- Required-field validation
- Save and emitted-result behavior
- Cancel/reset behavior
- Load and save failure handling

The frontend CI job uses Node 24 and Chrome Headless. The backend job uses .NET 9 with EF Core InMemory for service-level tests.

## Design Principles

1. Explicit typed contracts between application layers.
2. Thin controllers and testable application services.
3. Server-side operations for large datasets.
4. Asynchronous I/O throughout the API/data path.
5. Configuration and secrets kept outside source control.
6. Security and authorization enforced on the server as those capabilities are introduced.
7. Clear separation between domain concepts and presentation models.
8. Preserve user-entered state when recoverable API failures occur.
9. Validate behavior through automated CI before advancing an increment.
10. AI-assisted changes receive normal human engineering review.
