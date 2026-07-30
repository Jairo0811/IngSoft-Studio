# IngSoft Studio

IngSoft Studio is a web-based software engineering workspace designed to manage, analyze, and simulate the software development lifecycle—from requirements and design to testing, quality, metrics, deployment, and maintenance.

## Vision

The project combines professional software engineering workflows with an educational simulation environment. It is intended as a portfolio-grade application that demonstrates architecture, analysis, quality assurance, traceability, reporting, and full-stack development practices.

## Planned modules

- Identity and access management
- Project management
- Requirements engineering
- Analysis and design
- Risk management
- Testing and quality assurance
- Traceability
- Metrics and estimation
- Reviews and audits
- Reports and exports
- Software engineering simulator
- Learning center

## Technology stack

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity
- SQL Server LocalDB / SQL Server Express
- FluentValidation
- Mapster
- Serilog
- QuestPDF
- ClosedXML

### Frontend

- React
- TypeScript
- Vite
- Tailwind CSS
- React Router
- TanStack Query
- React Hook Form
- Zod
- Recharts
- React Flow
- Lucide React

### Quality

- xUnit
- FluentAssertions
- NSubstitute
- Vitest
- React Testing Library
- Playwright
- GitHub Actions

## Architecture

IngSoft Studio will use a pragmatic Clean Architecture approach organized as a modular monolith.

```text
IngSoft-Studio/
├── backend/
│   ├── src/
│   │   ├── IngSoftStudio.Domain/
│   │   ├── IngSoftStudio.Application/
│   │   ├── IngSoftStudio.Infrastructure/
│   │   └── IngSoftStudio.Api/
│   └── tests/
│       ├── IngSoftStudio.UnitTests/
│       └── IngSoftStudio.IntegrationTests/
├── frontend/
│   └── ingsoft-studio-web/
├── docs/
├── .github/workflows/
└── README.md
```

## Status

Initial project scaffolding in progress.

## License

License pending definition.
