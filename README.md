# LendWise

LendWise is a ground-up mortgage relationship and loan pipeline application. It is built as a modern ASP.NET Core Razor Pages app with Entity Framework Core, SQLite-backed local demo data, and automated tests.

## Project Layout

- `src/LendWise.Web/` - Razor Pages application, EF Core data model, services, seeded demo data, and static assets
- `tests/LendWise.Tests/` - xUnit coverage for seeded data, customer search, dashboard metrics, and work queue behavior
- `docs/` - stack, development, and operating notes

## Quick Start

```powershell
cd E:\LendWise
dotnet restore .\LendWise.slnx
dotnet build .\LendWise.slnx
dotnet test .\LendWise.slnx --no-build
dotnet run --project .\src\LendWise.Web\LendWise.Web.csproj --urls http://localhost:5088
```

Open `http://localhost:5088` after the app starts.

If Windows Defender or Application Control blocks the generated executable, run the app through the framework host:

```powershell
dotnet .\src\LendWise.Web\bin\Debug\net10.0\LendWise.Web.dll --urls http://localhost:5088
```

## Demo Data

The local database is generated at `src/LendWise.Web/data/lendwise-demo.db`. The seeded records are synthetic and exist only to exercise the product workflows.

To reset the demo data:

```powershell
Remove-Item .\src\LendWise.Web\data\lendwise-demo.db -ErrorAction SilentlyContinue
dotnet run --project .\src\LendWise.Web\LendWise.Web.csproj --urls http://localhost:5088
```

## More Detail

See `docs\USER_GUIDE.md` for non-technical product usage instructions.

See `docs\STACK.md` for the complete stack guide, development workflow, and naming/data-handling rules.
