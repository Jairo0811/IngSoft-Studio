<div align="center">

<img src="./docs/images/ingsoft-studio-banner.png" alt="Portada de IngSoft Studio" width="100%" />

<br/>

<img src="https://img.shields.io/badge/ITLA-2017--C3-0057B8?style=for-the-badge" alt="ITLA 2017-C3" />

<br/><br/>

![Estado](https://img.shields.io/badge/Estado-Fase%205%20implementada-22C55E?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-19-61DAFB?style=for-the-badge&logo=react&logoColor=0B1220)
![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Architecture](https://img.shields.io/badge/Architecture-Clean-14B8A6?style=for-the-badge)
![Identity](https://img.shields.io/badge/ASP.NET_Core-Identity-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

> Plataforma web para gestionar, analizar y simular el ciclo de vida del desarrollo de software, desde los requisitos y el diseño hasta las pruebas, la calidad y el mantenimiento.

</div>

## 📌 Descripción

**IngSoft Studio** es una plataforma web orientada a la gestión integral de proyectos de software. Centraliza requisitos, análisis, diseño, riesgos, métricas, pruebas, calidad, trazabilidad, mantenimiento y simulación de escenarios dentro de un único espacio de trabajo.

El proyecto nace como una reconstrucción moderna de los contenidos estudiados en **Introducción a la Ingeniería en Software (SOF-015)** del Instituto Tecnológico de Las Américas. La asignatura fue principalmente teórica; esta nueva implementación convierte aquellos fundamentos en una aplicación real, modular y preparada para crecer como proyecto profesional de portafolio.

> 💡 La idea de transformar el trabajo académico en una plataforma de software fue concebida por **Francis Jairo Matías Rosario**.

---

## 🎯 Objetivo general

Desarrollar un entorno profesional que permita planificar, documentar, controlar y evaluar el ciclo de vida de proyectos de software, aplicando principios de Ingeniería de Software, trazabilidad, aseguramiento de calidad, estimación, gestión de riesgos y mejora continua.

---

## 🔄 Ciclo de vida cubierto

| Fase | Alcance dentro de IngSoft Studio |
|---|---|
| 📋 Requisitos | Requisitos funcionales y no funcionales, historias de usuario, casos de uso, criterios de aceptación y priorización MoSCoW |
| 📐 Análisis y diseño | Casos de uso, arquitectura, componentes y decisiones técnicas |
| 💻 Desarrollo | Planificación, tareas, estados y seguimiento del progreso |
| 🧪 Pruebas | Casos de prueba, ejecuciones, evidencias, defectos y cobertura |
| 🚀 Despliegue | Versiones, entregas, liberaciones y control de cambios |
| 🔄 Mantenimiento | Incidencias, solicitudes de mejora y evolución continua |

---

## 🚀 Funcionalidades previstas

- 📊 Dashboard ejecutivo
- 📁 Gestión de proyectos
- 👥 Usuarios, roles y permisos
- 📋 Ingeniería de requisitos
- 🧭 Historias de usuario y casos de uso
- ✅ Criterios de aceptación y fuente del requisito
- 🏗️ Análisis y diseño de arquitectura
- ⚠️ Gestión de riesgos
- 📐 Métricas y estimaciones
- 🧪 Gestión de pruebas y defectos
- 🔗 Matriz de trazabilidad
- ✅ Revisiones y aseguramiento de calidad
- 📄 Reportes PDF y Excel
- 🎮 Simulador de decisiones de Ingeniería de Software
- 🎓 Centro de aprendizaje
- 🔔 Notificaciones y seguimiento

---

## 🛠️ Stack tecnológico

### Backend

<p>
  <img src="https://skillicons.dev/icons?i=dotnet,cs" alt=".NET y C#" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/swagger/swagger-original.svg" alt="Swagger" width="48" height="48" />
</p>

- C#
- .NET 10
- ASP.NET Core Web API
- ASP.NET Core Identity
- JWT Bearer Authentication
- Entity Framework Core 10.0.8
- SQL Server LocalDB / SQL Server Express
- Swagger / OpenAPI
- Serilog
- QuestPDF 2026.7.2
- ClosedXML 0.105.1

### Frontend

<p>
  <img src="https://skillicons.dev/icons?i=react,ts,vite" alt="React, TypeScript y Vite" />
</p>

- React 19
- TypeScript
- Vite
- React Router
- TanStack Query
- CSS responsive
- Lucide React

### Base de datos

<p>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/microsoftsqlserver/microsoftsqlserver-plain.svg" alt="SQL Server" width="48" height="48" />
</p>

- Microsoft SQL Server
- Entity Framework Core Migrations
- LocalDB para desarrollo local
- Tablas de ASP.NET Core Identity integradas al mismo DbContext
- Persistencia relacional de proyectos, requisitos, calidad y resultados del simulador

### Calidad y automatización

<p>
  <img src="https://skillicons.dev/icons?i=git,github,githubactions" alt="Git, GitHub y GitHub Actions" />
</p>

<p>
  <img src="https://img.shields.io/badge/GitHub%20Actions-CI-2088FF?style=flat-square&logo=githubactions&logoColor=white" alt="GitHub Actions" />
  <img src="https://img.shields.io/badge/xUnit-Tests-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt="xUnit" />
</p>

- xUnit
- FluentAssertions
- ASP.NET Core integration tests
- GitHub Actions

---

## 🏗️ Arquitectura

IngSoft Studio utiliza una **Clean Architecture pragmática** organizada como **monolito modular**.

```text
IngSoft-Studio/
├── backend/
│   ├── src/
│   │   ├── IngSoftStudio.Domain/
│   │   ├── IngSoftStudio.Application/
│   │   ├── IngSoftStudio.Infrastructure/
│   │   └── IngSoftStudio.Api/
│   └── tests/
├── frontend/
├── docs/
├── .github/workflows/
└── README.md
```

```text
Domain ← Application ← Infrastructure ← API
```

- **Domain:** entidades y reglas de negocio.
- **Application:** casos de uso, contratos y DTOs.
- **Infrastructure:** EF Core, SQL Server, Identity, reportes y servicios técnicos.
- **API:** endpoints REST, autenticación JWT, autorización, middleware y OpenAPI.
- **Frontend:** interfaz React modular desacoplada del backend.

---

## 🧱 Principios de desarrollo

- Clean Code
- SOLID
- DRY
- KISS
- Separación de responsabilidades
- Arquitectura modular
- Seguridad por diseño
- Código mantenible y escalable

---

## 🎓 Contexto académico

| Dato | Información |
|---|---|
| 🏫 Institución | Instituto Tecnológico de Las Américas (ITLA) |
| 📖 Asignatura | Introducción a la Ingeniería en Software |
| 🆔 Código | SOF-015 |
| 👨‍🏫 Profesor | Leandro Eduardo Fondeur Gil |
| 📅 Período académico | 2017-C3 |
| 👥 Grupo | #4 |
| 📚 Naturaleza de la materia | Teórica |
| 💡 Idea de reconstrucción | Francis Jairo Matías Rosario |

## 👥 Integrantes del grupo original

| Nombre completo | Matrícula |
|---|---|
| 👨‍💻 **Francis Jairo Matías Rosario** | 2015-2984 |
| 👨‍🎓 **Franger Omar Ramírez Peguero** | 2015-3008 |
| 👨‍🎓 **Pedro Arturo de León Parra** | 2015-3018 |
| 👨‍🎓 **José Andres Durán Diaz** | 2015-3035 |
| 👨‍🎓 **Fidel Ernesto Acosta Morillo** | 2015-3045 |

> El grupo participó en los trabajos académicos originales. La reconstrucción moderna de IngSoft Studio corresponde a una iniciativa posterior desarrollada por Francis Jairo Matías Rosario.

### Reencuentro académico

**Pedro Arturo de León Parra** también formó parte del grupo de **Auditoría Informática (SOF-009)** que posteriormente inspiró el proyecto [AuditCore](https://github.com/Jairo0811/AuditCore). IngSoft Studio representa, por tanto, un nuevo capítulo de continuidad académica entre proyectos nacidos de materias teóricas del ITLA.

---

## 📦 Estado actual

Las **Fases 1, 2, 3, 4 y 5** se encuentran implementadas. IngSoft Studio dispone de una base full stack estable, autenticación y autorización, gestión de proyectos y requisitos, Quality Center, trazabilidad, simulación, reportes y un centro de aprendizaje conectado a datos reales.

| Área | Estado | Detalle |
|---|---|---|
| Backend ASP.NET Core | ✅ | Domain, Application, Infrastructure y API |
| Frontend React | ✅ | React 19, TypeScript, Vite, Projects, Quality Center y Studio Insights |
| SQL Server | ✅ | DbContext, Identity, requisitos, calidad y `SimulationAttempts` |
| Swagger / OpenAPI | ✅ | Configurado con autenticación Bearer |
| GitHub Actions | ✅ | Restore, build, tests de dominio, tests de integración y frontend build |
| Pruebas automatizadas | ✅ | xUnit, FluentAssertions y WebApplicationFactory |
| Registro / Login | ✅ | ASP.NET Core Identity + JWT |
| Roles y permisos | ✅ | Roles `Admin` y `User`; endpoints administrativos protegidos |
| Perfil de usuario | ✅ | Consulta, edición y cambio de contraseña |
| Recuperación de contraseña | ✅ Base | Tokens de Identity; token visible únicamente en Development |
| Gestión de proyectos | ✅ | CRUD y ciclo de vida Draft, Active, Completed y Archived |
| Ingeniería de requisitos | ✅ | Funcionales, no funcionales, historias de usuario y casos de uso |
| MoSCoW | ✅ | Must, Should, Could y Won't |
| Calidad y pruebas | ✅ | Riesgos, métricas, casos de prueba, ejecuciones y defectos |
| Matriz de trazabilidad | ✅ | Requirement → Test Case → Defect |
| Dashboard ejecutivo | ✅ | Portafolio y métricas conectadas a datos reales |
| Indicadores por proyecto | ✅ | Cobertura, pass rate, defectos, riesgos y comparación |
| Simulador | ✅ | 5 escenarios, puntuación, feedback e historial persistente por usuario |
| Centro de aprendizaje | ✅ | Requisitos, SOLID, testing, riesgos, trazabilidad, cambios y releases |
| Reportes | ✅ | PDF con QuestPDF y Excel con ClosedXML |
| Studio Insights | ✅ | Workspace `/studio` con dashboard, reportes, simulación y aprendizaje |
| Hardening y release | 🚧 | Fase 6 |

---

## 🗺️ Roadmap

### ✅ Fase 1 — Fundación técnica
- [x] Arquitectura backend
- [x] Proyecto frontend
- [x] Dashboard inicial
- [x] Primer módulo de proyectos
- [x] Swagger / OpenAPI
- [x] GitHub Actions
- [x] Migración inicial de SQL Server
- [x] Pruebas base

### ✅ Fase 2 — Identidad y acceso
- [x] Registro e inicio de sesión
- [x] JWT Bearer Authentication
- [x] Roles `Admin` y `User`
- [x] Administración básica de roles
- [x] Protección de endpoints
- [x] Recuperación y restablecimiento de contraseña
- [x] Perfil de usuario
- [x] Cambio de contraseña
- [x] UI de login y registro
- [x] UI de perfil y recuperación de contraseña
- [x] Migración SQL Server para Identity

### ✅ Fase 3 — Proyectos y requisitos
- [x] Gestión completa de proyectos
- [x] Ciclo de vida de proyectos
- [x] Requisitos funcionales y no funcionales
- [x] Historias de usuario
- [x] Casos de uso
- [x] Priorización MoSCoW
- [x] Estados de requisitos
- [x] Criterios de aceptación
- [x] Trazabilidad básica por fuente
- [x] Migración EF Core `AddRequirements`
- [x] Workspace React de proyectos y requisitos
- [x] Pruebas de dominio
- [x] Pruebas de integración de API

### ✅ Fase 4 — Calidad y pruebas
- [x] Riesgos
- [x] Métricas
- [x] Casos de prueba
- [x] Ejecución de pruebas
- [x] Defectos
- [x] Matriz de trazabilidad
- [x] Quality Center React
- [x] Migración EF Core `AddQualityManagement`

### ✅ Fase 5 — Simulación y reportes
- [x] Simulador de decisiones
- [x] Historial y puntuación acumulada por usuario
- [x] Dashboard conectado a datos reales
- [x] Dashboard comparativo por proyecto
- [x] Tendencias de requisitos, pruebas, defectos y riesgos
- [x] Reportes PDF con QuestPDF
- [x] Exportación Excel con ClosedXML
- [x] Centro de aprendizaje
- [x] Studio Insights React
- [x] Migración EF Core `AddSimulationAttempts`
- [x] Pruebas de dominio del simulador

### Fase 6 — Hardening y release
- [ ] UX final y accesibilidad
- [ ] Seguridad y hardening
- [ ] Observabilidad y auditoría
- [ ] Documentación final
- [ ] Release `v1.0.0`

---

## ▶️ Ejecución local

### Requisitos

- .NET 10 SDK
- Node.js 22+
- SQL Server LocalDB o SQL Server Express
- Git

### Backend

La clave JWT **no se almacena en Git**. Antes de ejecutar la API, configura una clave local de al menos 32 caracteres:

```powershell
$env:Jwt__SigningKey="TU_CLAVE_LOCAL_SEGURA_DE_AL_MENOS_32_CARACTERES"
```

Opcionalmente puedes crear un administrador inicial mediante variables de entorno:

```powershell
$env:SeedAdmin__Email="admin@ingsoftstudio.local"
$env:SeedAdmin__FullName="Administrador IngSoft Studio"
$env:SeedAdmin__Password="CambiaEstaClave123!"
```

Aplica las migraciones y ejecuta la API:

```powershell
cd backend/src/IngSoftStudio.Api
dotnet restore
dotnet ef database update --project ../IngSoftStudio.Infrastructure/IngSoftStudio.Infrastructure.csproj
dotnet run
```

### Pruebas

```powershell
dotnet test backend/tests/IngSoftStudio.Domain.Tests/IngSoftStudio.Domain.Tests.csproj
dotnet test backend/tests/IngSoftStudio.Api.IntegrationTests/IngSoftStudio.Api.IntegrationTests.csproj
```

### Frontend

```powershell
cd frontend
$env:VITE_API_URL="http://localhost:5000"
npm install
npm run dev
```

> Ajusta `VITE_API_URL` al puerto real expuesto por la API en tu entorno local. Los workspaces principales están en `/projects`, `/quality` y `/studio`.

---

## 🔐 Endpoints principales de identidad

| Método | Endpoint | Acceso |
|---|---|---|
| POST | `/api/auth/register` | Público |
| POST | `/api/auth/login` | Público |
| POST | `/api/auth/forgot-password` | Público |
| POST | `/api/auth/reset-password` | Público |
| GET | `/api/auth/me` | Autenticado |
| PUT | `/api/auth/profile` | Autenticado |
| POST | `/api/auth/change-password` | Autenticado |
| GET | `/api/admin/users` | Admin |
| PUT | `/api/admin/users/{userId}/roles/{roleName}` | Admin |
| DELETE | `/api/admin/users/{userId}/roles/{roleName}` | Admin |

## 📁 Endpoints principales de proyectos y requisitos

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/v1/projects` | Listar proyectos |
| POST | `/api/v1/projects` | Crear proyecto |
| GET | `/api/v1/projects/{id}` | Consultar proyecto |
| PUT | `/api/v1/projects/{id}` | Actualizar proyecto |
| PATCH | `/api/v1/projects/{id}/status` | Cambiar estado |
| DELETE | `/api/v1/projects/{id}` | Eliminar proyecto |
| GET | `/api/v1/projects/{projectId}/requirements` | Listar requisitos |
| POST | `/api/v1/projects/{projectId}/requirements` | Crear requisito |
| PUT | `/api/v1/projects/{projectId}/requirements/{id}` | Actualizar requisito |
| PATCH | `/api/v1/projects/{projectId}/requirements/{id}/status` | Cambiar estado del requisito |
| DELETE | `/api/v1/projects/{projectId}/requirements/{id}` | Eliminar requisito |

## 📊 Endpoints principales de Studio Insights

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/v1/studio/dashboard` | Dashboard global |
| GET | `/api/v1/studio/projects` | Indicadores por proyecto |
| GET | `/api/v1/studio/trends` | Tendencias comparativas |
| GET | `/api/v1/studio/simulation/scenarios` | Escenarios del simulador |
| POST | `/api/v1/studio/simulation/evaluate` | Evaluar y persistir una decisión |
| GET | `/api/v1/studio/simulation/summary` | Historial y puntuación acumulada |
| GET | `/api/v1/studio/learning` | Centro de aprendizaje |
| GET | `/api/v1/studio/reports/pdf` | Descargar reporte PDF |
| GET | `/api/v1/studio/reports/excel` | Descargar reporte Excel |

---

## 📚 Proyecto relacionado

- [AuditCore](https://github.com/Jairo0811/AuditCore) — plataforma de gestión de auditorías de TI inspirada en la asignatura Auditoría Informática (SOF-009).

Ambos proyectos convierten materias teóricas del ITLA en aplicaciones web profesionales, preservando su contexto académico y evolucionándolo hacia soluciones modernas de portafolio.

---

## 📄 Licencia

Licencia pendiente de definición.

---

<div align="center">

### IngSoft Studio

**Planifica. Diseña. Construye. Mejora.**

Desarrollado como reconstrucción moderna de una experiencia académica del ITLA.

</div>