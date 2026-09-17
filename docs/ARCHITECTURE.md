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

The `client` directory contains a standalone Angular 19 application responsible for presentation, routing, typed API communication, search/filter state, reactive-form validation, and user workflows.

The application shell uses Angular Router to expose separate `/projects` and `/customers` workspaces. Each workspace owns its search and editor state instead of placing unrelated domain workflows in the root component.

Project Search and Customer Search use AG Grid for tabular results. Project Editor and Customer Editor use Angular Reactive Forms for explicit load, edit, Save, and Cancel behavior. TypeScript interfaces mirror backend DTO contracts, while persistence remains entirely behind the API boundary.

## Server

The `server` directory contains the ASP.NET Core / .NET 9 Web API.

Primary responsibilities include:

- HTTP endpoints and explicit API contracts
- Request validation
- Application/service orchestration
- Query composition
- Data access through Entity Framework Core
- Project and customer persistence
- Extensible boundaries for authentication, authorization, logging, and centralized error handling

Controllers remain thin. Search behavior is delegated to `ProjectSearchService` and `CustomerSearchService`; detail loading and persistence are delegated to `ProjectEditorService` and `CustomerEditorService`. This keeps query and update logic testable independently from controller actions.

## Data

SQL Server is the persistence layer. Entity Framework Core provides application data access while T-SQL scripts document schema, indexes, and sanitized sample data.

Search services perform filtering, sorting, counting, and pagination on the server so large result sets are not unnecessarily transferred to the browser. Editor services load detail DTOs with no-tracking projections and use tracked entities for updates before calling `SaveChangesAsync`.

## Domain

The implemented reference domain currently contains two end-to-end entities:

- `Project`
- `Customer`

The architecture is designed to expand to concepts such as `Service`, `ProjectStatus`, and `ApplicationUser`. These concepts are intentionally generic and are not copies of a client database schema.

## Application Navigation

```text
AppComponent
   -> Angular Router
      -> /projects
         -> ProjectSearchComponent
         -> ProjectEditorComponent
      -> /customers
         -> CustomerSearchComponent
         -> CustomerEditorComponent
```

The root route and unknown routes redirect to `/projects`. The application shell owns only global presentation and navigation; domain workflow state remains inside each routed feature.

## Implemented End-to-End Flows

### Project Search

```text
Project Search UI / AG Grid
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

### Project Editor

```text
Grid Row Selection
   -> ProjectEditorComponent
   -> GET /api/projects/{id}
   -> ProjectEditorService.GetByIdAsync
   -> ProjectDetailDto
   -> Reactive Form
   -> Client Validation
   -> UpdateProjectRequest
   -> PUT /api/projects/{id}
   -> ProjectEditorService.UpdateAsync
   -> SaveChangesAsync
   -> Editor Success State
   -> Refresh Project Grid
```

### Customer Search

```text
Customer Search UI / AG Grid
   -> CustomerSearchRequest
   -> POST /api/customers/search
   -> CustomersController
   -> CustomerSearchService
   -> IQueryable<Customer>
   -> Filter / Sort / Count / Page
   -> PagedResponse<CustomerSummaryDto>
   -> CustomerService
   -> Angular Grid
```

Customer search supports free-text matching across customer number, company, contact, email, and city, plus state and active-state filters. Sorting and pagination remain server-side.

### Customer Editor

```text
Grid Row Selection
   -> CustomerEditorComponent
   -> GET /api/customers/{id}
   -> CustomerEditorService.GetByIdAsync
   -> CustomerDetailDto
   -> Reactive Form
   -> Client Validation
   -> UpdateCustomerRequest
   -> PUT /api/customers/{id}
   -> CustomerEditorService.UpdateAsync
   -> SaveChangesAsync
   -> Editor Success State
   -> Refresh Customer Grid
```

Customer number and creation date remain identity/display data while company, contact, email, phone, city, state, and active status are editable. Cancel restores the last loaded or successfully saved state. Failed saves preserve the user's current edits.

## Validation and Error Handling

Validation exists at both application boundaries. Angular prevents invalid editor submissions, including required fields, maximum lengths, and customer email format. ASP.NET Core validates update DTO data annotations before controller actions proceed.

Missing resources return HTTP 404. The Angular editors expose load and save failure states without silently discarding user-entered changes. Broader centralized API exception handling and structured logging remain planned capabilities.

## Automated Testing and CI

GitHub Actions builds and tests the Angular and .NET applications on pushes and pull requests to `main`.

Current automated coverage includes:

- Project and customer search filtering and active-state filtering
- Server-side pagination and sorting
- Project and customer detail retrieval and not-found behavior
- Project and customer update persistence and not-found behavior
- Angular search, GET-detail, and PUT-update HTTP contracts
- Reactive-form loading and population
- Required-field and customer-email validation
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
7. Clear separation between routed domain features and the application shell.
8. Preserve user-entered state when recoverable API failures occur.
9. Validate behavior through automated CI before advancing an increment.
10. AI-assisted changes receive normal human engineering review.
