# IngSoft Studio

![Estado](https://img.shields.io/badge/Estado-Fase%201%20completada-22C55E?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-19-61DAFB?style=for-the-badge&logo=react&logoColor=0B1220)
![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-EF_Core-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)

> Plataforma web para gestionar, analizar y simular el ciclo de vida del desarrollo de software, desde los requisitos y el diseño hasta las pruebas, la calidad y el mantenimiento.

## 📌 Descripción

**IngSoft Studio** reconstruye como aplicación profesional los fundamentos estudiados en **Introducción a la Ingeniería en Software (SOF-015)** del ITLA. La solución adopta Clean Architecture pragmática, un monolito modular y separación estricta entre Domain, Application, Infrastructure, API y frontend.

## 🛠️ Stack

**Backend:** C#, .NET 10, ASP.NET Core Web API, Entity Framework Core 10.0.8, SQL Server, Swagger/OpenAPI, Microsoft.OpenApi 2.11.0, Swashbuckle 10.1.0 y Serilog 10.0.0.

**Frontend:** React 19, TypeScript, Vite, React Router, TanStack Query y Tailwind CSS.

**Calidad:** xUnit, FluentAssertions y GitHub Actions.

## 🏗️ Arquitectura

```text
Domain ← Application ← Infrastructure ← API
```

- **Domain:** entidades y reglas de negocio.
- **Application:** casos de uso y contratos.
- **Infrastructure:** EF Core, SQL Server y servicios técnicos.
- **API:** endpoints, middleware, CORS, OpenAPI y logging.
- **Frontend:** aplicación React desacoplada del backend.

## 📦 Estado actual

La **Fase 1 — Fundación técnica está completada**. La solución dispone de base full stack, persistencia inicial, migración de SQL Server, pruebas de dominio y CI automatizado.

| Área | Estado | Detalle |
|---|---|---|
| Backend | ✅ | Domain, Application, Infrastructure y API |
| Frontend | ✅ | React 19 + TypeScript + Vite |
| SQL Server | ✅ | DbContext y migración inicial `InitialCreate` |
| Proyectos | ✅ Base | Entidad, servicio y endpoints iniciales |
| OpenAPI | ✅ | Swagger/OpenAPI estabilizado |
| Logging | ✅ | Serilog con cultura invariante |
| Pruebas | ✅ Base | Invariantes principales de `Project` cubiertas |
| CI | ✅ | Restore, build, tests backend y build frontend |

## 🗺️ Roadmap

### ✅ Fase 1 — Fundación técnica

- [x] Arquitectura backend
- [x] Proyecto frontend
- [x] Dashboard inicial
- [x] Primer módulo de proyectos
- [x] Swagger / OpenAPI
- [x] GitHub Actions
- [x] Dependencias estabilizadas
- [x] Compilación completa del backend en CI
- [x] Migración inicial de SQL Server
- [x] Pruebas base

### Fase 2 — Identidad y acceso

- [ ] Registro e inicio de sesión
- [ ] Roles y permisos
- [ ] Recuperación de contraseña
- [ ] Perfil de usuario

### Fase 3 — Proyectos y requisitos

- [ ] Gestión completa de proyectos
- [ ] Requisitos funcionales y no funcionales
- [ ] Historias de usuario
- [ ] Casos de uso
- [ ] Priorización MoSCoW

### Fase 4 — Calidad y pruebas

- [ ] Riesgos
- [ ] Métricas
- [ ] Casos de prueba
- [ ] Defectos
- [ ] Evidencias
- [ ] Matriz de trazabilidad

### Fase 5 — Simulación y reportes

- [ ] Simulador de decisiones
- [ ] Dashboard conectado a datos reales
- [ ] Reportes PDF y Excel
- [ ] Centro de aprendizaje

## ▶️ Ejecución local

```powershell
cd backend/src/IngSoftStudio.Api
dotnet restore
dotnet run
```

```powershell
cd frontend
npm install
npm run dev
```

Para ejecutar las pruebas de dominio:

```powershell
dotnet test backend/tests/IngSoftStudio.Domain.Tests/IngSoftStudio.Domain.Tests.csproj
```

Para aplicar la base de datos desde el proyecto API:

```powershell
dotnet ef database update --project ../IngSoftStudio.Infrastructure/IngSoftStudio.Infrastructure.csproj
```

## 🎓 Contexto académico

| Dato | Información |
|---|---|
| Institución | Instituto Tecnológico de Las Américas (ITLA) |
| Asignatura | Introducción a la Ingeniería en Software |
| Código | SOF-015 |
| Profesor | Leandro Eduardo Fondeur Gil |
| Período | 2017-C3 |
| Grupo | #4 |

### Integrantes del grupo original

| Nombre | Matrícula |
|---|---|
| Francis Jairo Matías Rosario | 2015-2984 |
| Franger Omar Ramírez Peguero | 2015-3008 |
| Pedro Arturo de León Parra | 2015-3018 |
| José Andres Durán Diaz | 2015-3035 |
| Fidel Ernesto Acosta Morillo | 2015-3045 |

La reconstrucción moderna de IngSoft Studio corresponde a una iniciativa posterior de **Francis Jairo Matías Rosario**.

## 📚 Proyecto relacionado

[AuditCore](https://github.com/Jairo0811/AuditCore) aplica una filosofía similar a los contenidos de Auditoría Informática (SOF-009).

---

**IngSoft Studio — Planifica. Diseña. Construye. Mejora.**
