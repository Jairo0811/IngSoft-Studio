<div align="center">

<img src="./docs/images/ingsoft-studio-banner.png" alt="Portada de IngSoft Studio" width="100%" />

<br/>

<img src="https://img.shields.io/badge/ITLA-2017--C3-0057B8?style=for-the-badge" alt="ITLA 2017-C3" />

<br/><br/>

![Estado](https://img.shields.io/badge/Estado-Fase%201%20completada-22C55E?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-19-61DAFB?style=for-the-badge&logo=react&logoColor=0B1220)
![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Architecture](https://img.shields.io/badge/Architecture-Clean-14B8A6?style=for-the-badge)

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
| 📋 Requisitos | Requisitos funcionales y no funcionales, historias de usuario y priorización |
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
- Entity Framework Core 10.0.8
- SQL Server LocalDB / SQL Server Express
- Swagger / OpenAPI
- FluentValidation
- Mapster

### Frontend

<p>
  <img src="https://skillicons.dev/icons?i=react,ts,vite,tailwind" alt="React, TypeScript, Vite y Tailwind CSS" />
</p>

- React 19
- TypeScript
- Vite
- React Router
- TanStack Query
- Tailwind CSS
- React Hook Form
- Zod
- Recharts
- React Flow

### Base de datos

<p>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/microsoftsqlserver/microsoftsqlserver-plain.svg" alt="SQL Server" width="48" height="48" />
</p>

- Microsoft SQL Server
- Entity Framework Core Migrations
- LocalDB para desarrollo local

### Calidad y automatización

<p>
  <img src="https://skillicons.dev/icons?i=git,github,githubactions" alt="Git, GitHub y GitHub Actions" />
</p>

<p>
  <img src="https://img.shields.io/badge/GitHub%20Actions-CI-2088FF?style=flat-square&logo=githubactions&logoColor=white" alt="GitHub Actions" />
  <img src="https://img.shields.io/badge/xUnit-Tests-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt="xUnit" />
  <img src="https://img.shields.io/badge/Vitest-Tests-6E9F18?style=flat-square&logo=vitest&logoColor=white" alt="Vitest" />
  <img src="https://img.shields.io/badge/Playwright-E2E-2EAD33?style=flat-square&logo=playwright&logoColor=white" alt="Playwright" />
</p>

- xUnit
- FluentAssertions
- NSubstitute
- Vitest
- React Testing Library
- Playwright
- GitHub Actions

---

## 🏗️ Arquitectura

IngSoft Studio utiliza una **Clean Architecture pragmática** organizada como **monolito modular**.

```text
IngSoft-Studio/
├── backend/src/
│   ├── IngSoftStudio.Domain/
│   ├── IngSoftStudio.Application/
│   ├── IngSoftStudio.Infrastructure/
│   └── IngSoftStudio.Api/
├── frontend/ingsoft-studio-web/
├── docs/
├── .github/workflows/
└── README.md
```

```text
Domain ← Application ← Infrastructure ← API
```

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

---

## 📦 Estado actual

La **Fase 1 — Fundación técnica está completada**. La base full stack se encuentra estable, con persistencia inicial en SQL Server mediante Entity Framework Core, pruebas de dominio y validación automatizada en GitHub Actions.

| Área | Estado | Detalle |
|---|---|---|
| Backend ASP.NET Core | ✅ | Domain, Application, Infrastructure y API |
| Frontend React | ✅ | React, TypeScript y Vite |
| SQL Server | ✅ | DbContext y migración inicial |
| Swagger / OpenAPI | ✅ | Configurado |
| GitHub Actions | ✅ En verde | Restore, build, tests y frontend build |
| Pruebas automatizadas | ✅ | xUnit y FluentAssertions |
| Autenticación | 🚧 | Pendiente |
| Módulos funcionales | 🚧 | Próximas fases |

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

### Fase 2 — Identidad y acceso
- [ ] Registro e inicio de sesión
- [ ] Roles y permisos
- [ ] Recuperación de contraseña
- [ ] Perfil de usuario

### Fase 3 — Proyectos y requisitos
- [ ] Gestión completa de proyectos
- [ ] Requisitos
- [ ] Historias de usuario
- [ ] Casos de uso
- [ ] Priorización MoSCoW

### Fase 4 — Calidad y pruebas
- [ ] Riesgos
- [ ] Métricas
- [ ] Casos de prueba
- [ ] Defectos
- [ ] Matriz de trazabilidad

### Fase 5 — Simulación y reportes
- [ ] Simulador de decisiones
- [ ] Dashboard conectado a datos reales
- [ ] Reportes PDF y Excel
- [ ] Centro de aprendizaje

---

## ▶️ Ejecución local

### Backend

```powershell
cd backend/src/IngSoftStudio.Api
dotnet restore
dotnet run
```

### Pruebas

```powershell
dotnet test backend/tests/IngSoftStudio.Domain.Tests/IngSoftStudio.Domain.Tests.csproj
```

### Frontend

```powershell
cd frontend/ingsoft-studio-web
npm install
npm run dev
```

---

## 📚 Proyecto relacionado

- [AuditCore](https://github.com/Jairo0811/AuditCore) — plataforma de gestión de auditorías de TI inspirada en Auditoría Informática (SOF-009).

---

## 📄 Licencia

Licencia pendiente de definición.

<div align="center">

### IngSoft Studio

**Planifica. Diseña. Construye. Mejora.**

</div>
