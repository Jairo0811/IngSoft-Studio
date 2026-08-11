# Arquitectura de IngSoft Studio

IngSoft Studio utiliza una **Clean Architecture pragmática** organizada como **monolito modular full stack**. La Fase 1 establece los límites fundamentales entre dominio, casos de uso, infraestructura, API y frontend; los módulos funcionales siguientes deben crecer respetando esas dependencias.

## Vista general

```mermaid
flowchart LR
    User["Usuario"] --> Web["React 19 · TypeScript · Vite"]
    Web --> Router["React Router / TanStack Query"]
    Router --> API["ASP.NET Core Web API"]

    API --> Application["Application · Casos de uso / DTO / Validación"]
    Application --> Domain["Domain · Entidades / Reglas"]

    Infrastructure["Infrastructure"] --> Application
    Infrastructure --> Domain
    Infrastructure --> EF["Entity Framework Core"]
    EF --> SQL[("SQL Server")]

    API --> OpenAPI["Swagger / OpenAPI"]
    API --> Logging["Serilog"]
```

La regla principal es que **Domain no depende de infraestructura ni presentación**. Application define los casos de uso y contratos; Infrastructure implementa persistencia y servicios técnicos; API compone la aplicación; React consume únicamente contratos HTTP.

## Regla de dependencias

```mermaid
flowchart TD
    Frontend["React Frontend"] --> API["IngSoftStudio.Api"]
    API --> Application["IngSoftStudio.Application"]
    Application --> Domain["IngSoftStudio.Domain"]
    Infrastructure["IngSoftStudio.Infrastructure"] --> Application
    Infrastructure --> Domain
```

```text
Domain ← Application ← Infrastructure ← API
```

La representación textual resume la dirección conceptual; Infrastructure implementa contratos definidos hacia dentro y API actúa como composition root.

## Ensamblados

| Proyecto | Responsabilidad |
|---|---|
| `IngSoftStudio.Domain` | Entidades, invariantes, enumeraciones y reglas de negocio |
| `IngSoftStudio.Application` | Casos de uso, DTO, contratos, validación y mapeo |
| `IngSoftStudio.Infrastructure` | EF Core, SQL Server, repositorios y servicios técnicos |
| `IngSoftStudio.Api` | Endpoints, middleware, composición, OpenAPI y configuración |
| `ingsoft-studio-web` | Experiencia React, rutas, componentes y consumo de API |

## Módulos funcionales previstos

```mermaid
flowchart TB
    Platform["IngSoft Studio"]
    Platform --> Identity["Identidad / Roles / Permisos"]
    Platform --> Projects["Proyectos"]
    Platform --> Requirements["Requisitos / Historias / Casos de uso"]
    Platform --> Design["Análisis / Diseño / Arquitectura"]
    Platform --> Risks["Riesgos / Estimaciones"]
    Platform --> Testing["Pruebas / Defectos / Evidencias"]
    Platform --> Traceability["Trazabilidad"]
    Platform --> Quality["Calidad / Revisiones"]
    Platform --> Releases["Versiones / Despliegue / Mantenimiento"]
    Platform --> Simulation["Simulador de decisiones"]
    Platform --> Learning["Centro de aprendizaje"]
    Platform --> Reporting["Dashboard / PDF / Excel"]
```

Los módulos posteriores deben incorporarse como capacidades verticales sin romper la regla de dependencias ni convertir la API en un contenedor de lógica de negocio.

## Flujo de una operación

```mermaid
sequenceDiagram
    participant U as Usuario
    participant W as React
    participant A as API
    participant UC as Application
    participant D as Domain
    participant I as Infrastructure
    participant DB as SQL Server

    U->>W: acción
    W->>A: request HTTP
    A->>UC: comando / consulta
    UC->>D: aplicar reglas e invariantes
    UC->>I: usar contrato de persistencia
    I->>DB: EF Core / SQL
    DB-->>I: datos
    I-->>UC: resultado
    UC-->>A: DTO
    A-->>W: respuesta
    W-->>U: actualizar interfaz
```

## Persistencia

```mermaid
flowchart LR
    Application["Application"] --> Contract["Contratos / Repositorios"]
    Infrastructure["Infrastructure"] --> Contract
    Infrastructure --> DbContext["IngSoftStudioDbContext"]
    DbContext --> Migrations["EF Core Migrations"]
    DbContext --> SQL[("SQL Server / LocalDB")]
```

Entity Framework Core y SQL Server pertenecen a Infrastructure. Domain no debe referenciar `DbContext`, migraciones ni detalles de almacenamiento.

## Estado actual de la arquitectura

La **Fase 1 — Fundación técnica** ya dispone de:

- proyectos Domain, Application, Infrastructure y API;
- frontend React + TypeScript + Vite;
- primer módulo básico de proyectos;
- `DbContext` y migración inicial para SQL Server;
- Swagger/OpenAPI;
- logging con Serilog;
- pruebas de dominio;
- pipeline de GitHub Actions para backend y frontend.

La autenticación y la mayoría de módulos de negocio pertenecen a fases posteriores y no deben considerarse implementados por el mero hecho de aparecer en el diagrama de evolución.

## Calidad y CI

```mermaid
flowchart LR
    Backend["Backend .NET"] --> Build["dotnet build"]
    Tests["xUnit / FluentAssertions"] --> CI["GitHub Actions"]
    Build --> CI
    Frontend["React / TypeScript"] --> FrontBuild["npm build"]
    FrontBuild --> CI
    CI --> Gate["Fase validada"]
```

## Criterio de evolución

IngSoft Studio debe continuar como monolito modular mientras los módulos compartan el mismo contexto transaccional y equipo. La prioridad es reforzar límites internos, pruebas y contratos; microservicios no aportan valor en la etapa actual.
