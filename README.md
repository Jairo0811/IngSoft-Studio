# IngSoft Studio

> **Engineering Better Software**

IngSoft Studio es una plataforma web orientada a la gestión, análisis, planificación y simulación del ciclo de vida del desarrollo de software. El proyecto transforma los fundamentos estudiados en la asignatura **Introducción a la Ingeniería en Software (SOF-015)** en una aplicación moderna, profesional y preparada para evolucionar como proyecto de portafolio.

## Objetivo

Centralizar en un solo entorno las principales prácticas de Ingeniería de Software, desde la definición de requisitos hasta el mantenimiento del producto, incorporando trazabilidad, calidad, métricas, pruebas, reportes y simulación de escenarios.

## Ciclo de vida cubierto

1. **Requisitos** — gestión de requisitos funcionales y no funcionales.
2. **Análisis y diseño** — casos de uso, historias de usuario y arquitectura.
3. **Desarrollo** — planificación, tareas y seguimiento del progreso.
4. **Pruebas** — casos de prueba, ejecución, evidencias y cobertura.
5. **Despliegue** — versiones, liberaciones y control de entregas.
6. **Mantenimiento** — incidencias, mejoras y evolución continua.

## Módulos planificados

- Identidad y control de acceso
- Gestión de proyectos
- Ingeniería de requisitos
- Análisis y diseño
- Gestión de riesgos
- Calidad de software
- Casos de prueba y defectos
- Trazabilidad
- Métricas y estimaciones
- Revisiones y auditorías
- Reportes y exportaciones
- Simulador de Ingeniería de Software
- Centro de aprendizaje

## Arquitectura propuesta

IngSoft Studio utilizará una **Clean Architecture pragmática** organizada como **monolito modular**, evitando microservicios prematuros y manteniendo una separación clara de responsabilidades.

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

## Tecnologías previstas

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity
- SQL Server LocalDB o SQL Server Express
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

### Calidad

- xUnit
- FluentAssertions
- NSubstitute
- Vitest
- React Testing Library
- Playwright
- GitHub Actions

## Principios de desarrollo

- Clean Code
- SOLID
- DRY
- KISS
- Separación de responsabilidades
- Arquitectura modular
- Seguridad por diseño
- Código mantenible y escalable

## Origen académico

IngSoft Studio nace como una reinterpretación moderna de los contenidos trabajados en una asignatura principalmente teórica del Instituto Tecnológico de Las Américas (ITLA).

| Campo | Información |
|---|---|
| Institución | Instituto Tecnológico de Las Américas (ITLA) |
| Asignatura | Introducción a la Ingeniería en Software (SOF-015) |
| Profesor | Leandro Eduardo Fondeur Gil |
| Período Académico | 2017-C3 |
| Grupo | #4 |

## Integrantes del grupo original

- Francis Jairo Matías Rosario — 2015-2984
- Franger Ramírez — 2015-3008
- Pedro Arturo De León — 2015-3018
- José Durán — 2015-3035
- Fidel Acosta — 2015-3045

> El trabajo académico original fue de carácter teórico. IngSoft Studio representa una reconstrucción nueva, desarrollada desde cero para convertir esos fundamentos en una plataforma web profesional.

## Roadmap

### Fase 1 — Foundation

- [ ] Solución backend
- [ ] Proyecto frontend
- [ ] Arquitectura base
- [ ] Configuración de SQL Server
- [ ] CI con GitHub Actions

### Fase 2 — Identity

- [ ] Autenticación
- [ ] Roles y permisos
- [ ] Perfil de usuario

### Fase 3 — Projects & Requirements

- [ ] Gestión de proyectos
- [ ] Requisitos funcionales y no funcionales
- [ ] Historias de usuario
- [ ] Casos de uso

### Fase 4 — Quality & Testing

- [ ] Riesgos
- [ ] Métricas
- [ ] Casos de prueba
- [ ] Defectos y evidencias

### Fase 5 — Simulation & Reporting

- [ ] Simulador de decisiones
- [ ] Dashboard ejecutivo
- [ ] Reportes PDF y Excel
- [ ] Centro de aprendizaje

## Estado actual

**Versión inicial en construcción.** La siguiente etapa es crear el scaffolding funcional del backend y del frontend.

## Licencia

Licencia pendiente de definición.
