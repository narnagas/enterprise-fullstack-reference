# Local Development

## Prerequisites

- .NET 9 SDK
- Node.js 20 or later
- npm
- SQL Server LocalDB or another SQL Server instance
- Angular CLI is optional because the project can use `npx ng`

## 1. Create the database

The default API configuration expects SQL Server LocalDB with database name `EnterpriseFullStackReference`.

From SQL Server Management Studio or another SQL client, run these scripts in order:

```text
database/001-create-projects.sql
database/002-seed-projects.sql
```

If you use another SQL Server instance, update `DefaultConnection` in the API configuration or provide it through environment/user-secret configuration.

## 2. Run the API

From the repository root:

```bash
cd server/EnterpriseFullStackReference.Api
dotnet restore
dotnet run --launch-profile https
```

The development profile exposes:

```text
https://localhost:7001
http://localhost:5081
```

For local HTTPS, trust the ASP.NET Core development certificate if necessary:

```bash
dotnet dev-certs https --trust
```

## 3. Run the Angular client

Open a second terminal:

```bash
cd client
npm install
npm start
```

The client runs at:

```text
http://localhost:4200
```

The development environment calls:

```text
https://localhost:7001/api
```

The API CORS policy permits the Angular development origin `http://localhost:4200`.

## 4. Verify Project Search

When both applications are running, open the Angular client and verify:

- Active projects load on startup.
- Search finds project numbers, names, and customers.
- Customer and status filters can be combined.
- Column sorting sends a new server-side request.
- Page size changes reload the first page.
- Previous and Next request the appropriate page.
- Reset returns to the default active-project view.

## Configuration Guidance

The checked-in connection string and client URL are development defaults only. Production credentials, secrets, certificates, and internal hostnames should never be committed to this public repository.
