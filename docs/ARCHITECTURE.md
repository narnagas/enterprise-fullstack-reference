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

The `client` directory will contain a standalone Angular application responsible for presentation, typed API communication, search/filter state, validation, and user workflows.

The client should not contain database or persistence logic. API contracts are represented by TypeScript interfaces corresponding to backend DTOs.

## Server

The `server` directory will contain the ASP.NET Core Web API.

Primary responsibilities include:

- HTTP endpoints and API contracts
- Request validation
- Authentication and authorization
- Application/service orchestration
- Query composition
- Logging and error handling
- Data access through Entity Framework Core

Controllers should remain thin. Business and query logic belongs in application services rather than controller actions.

## Data

SQL Server is the persistence layer. Entity Framework Core provides application data access while T-SQL scripts document schema, indexes, and sample data where useful.

The reference application will demonstrate server-side filtering, sorting, and pagination so large result sets are not unnecessarily transferred to the browser.

## Initial Domain

The sample domain models a generic project operations system:

- Project
- Customer
- Service
- ProjectStatus
- ApplicationUser

These entities are intentionally generic and are not copies of a client database schema.

## First End-to-End Flow

Project Search will be implemented first:

```text
Search UI
   -> ProjectSearchRequest
   -> POST /api/projects/search
   -> ProjectSearchService
   -> IQueryable<Project>
   -> Filter / Sort / Page
   -> PagedResponse<ProjectSummaryDto>
   -> Angular Grid
```

## Design Principles

1. Explicit typed contracts between application layers.
2. Thin controllers and testable application services.
3. Server-side operations for large datasets.
4. Asynchronous I/O throughout the API/data path.
5. Configuration and secrets kept outside source control.
6. Security and authorization enforced on the server.
7. Clear separation between domain concepts and presentation models.
8. AI-assisted changes receive normal human engineering review.
