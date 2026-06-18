# LendWise Stack Guide

This guide explains how to work with the LendWise application stack.

## Runtime Stack

- .NET 10
- ASP.NET Core Razor Pages
- Entity Framework Core
- SQLite for local development and demo data
- xUnit for automated tests

The app is intentionally server-rendered. Razor Pages keeps the current product simple, searchable, and easy to refactor while the domain model is still settling.

## Solution Structure

- `E:\LendWise\LendWise.slnx` - solution file
- `E:\LendWise\src\LendWise.Web\` - web app
- `E:\LendWise\tests\LendWise.Tests\` - automated tests
- `E:\LendWise\src\LendWise.Web\data\lendwise-demo.db` - generated local SQLite database

## Daily Commands

Run these from `E:\LendWise`.

Restore dependencies:

```powershell
dotnet restore .\LendWise.slnx
```

Build:

```powershell
dotnet build .\LendWise.slnx
```

Run tests:

```powershell
dotnet test .\LendWise.slnx --no-build
```

Run the web app:

```powershell
dotnet run --project .\src\LendWise.Web\LendWise.Web.csproj --urls http://localhost:5088
```

Open:

```text
http://localhost:5088
```

Health check:

```text
http://localhost:5088/health
```

## Windows Defender Or Application Control

Some Windows configurations block newly generated application executables. If `dotnet run` starts building and then reports that the generated `.exe` is blocked, run through the framework host instead:

```powershell
dotnet .\src\LendWise.Web\bin\Debug\net10.0\LendWise.Web.dll --urls http://localhost:5088
```

This uses the trusted `dotnet` host to load the compiled application DLL.

## Data Model

The EF Core model lives in:

```text
E:\LendWise\src\LendWise.Web\Models\DomainModels.cs
E:\LendWise\src\LendWise.Web\Data\LendWiseDbContext.cs
```

The current domain centers on:

- customers
- contacts
- properties
- loans
- trust deeds
- work items
- pick-list options
- customer relationships
- activity history

Keep the model small and explicit while the product shape is still being validated. Prefer EF Core configuration in `LendWiseDbContext` over provider-specific assumptions in page handlers.

## Demo Data

Seed data lives in:

```text
E:\LendWise\src\LendWise.Web\Data\DemoDataSeeder.cs
```

All seeded people, companies, addresses, emails, and phone numbers must be synthetic. Do not import real customer data into the product repository.

Reset the local database:

```powershell
Remove-Item .\src\LendWise.Web\data\lendwise-demo.db -ErrorAction SilentlyContinue
dotnet run --project .\src\LendWise.Web\LendWise.Web.csproj --urls http://localhost:5088
```

## Adding A Page

1. Add a Razor Page under `src\LendWise.Web\Pages`.
2. Put data access behind `PortfolioService` or a new scoped service.
3. Keep page handlers thin.
4. Add navigation in `Pages\Shared\_Layout.cshtml` when the page is part of the main workflow.
5. Add tests when the page depends on new business logic or query behavior.

## Adding A Data Field

1. Add the property to the relevant entity in `DomainModels.cs`.
2. Configure relationships, precision, required fields, or indexes in `LendWiseDbContext.cs`.
3. Update `DemoDataSeeder.cs` so the field has representative synthetic data.
4. Update service queries and Razor views that should display or filter by the field.
5. Add or update tests in `tests\LendWise.Tests`.

## Naming Rules

- Use `LendWise` for product namespaces, project names, and user-facing product text.
- Use neutral terms such as `LegacyClient`, `LegacySource`, or `HistoricalReference` when copied reference material needs an anonymized placeholder.
- Do not use past client names, surnames, database names, machine paths, or business identifiers in source code, docs, config, generated sample data, or filenames.

## Validation Checklist

Before handing off changes:

```powershell
dotnet build .\LendWise.slnx
dotnet test .\LendWise.slnx --no-build
rg -n --hidden --glob '!**/bin/**' --glob '!**/obj/**' --glob '!**/.vs/**' --glob '!**/.git/**' "restricted-name-or-term" E:\LendWise E:\LendWise.Tools E:\LendWise.Lab
```

Replace `restricted-name-or-term` with any name or client-specific term that must not appear in the project.
