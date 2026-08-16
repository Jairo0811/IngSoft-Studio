<div align="center">

<img src="./docs/images/ingsoft-studio-banner.png" alt="Portada de IngSoft Studio" width="100%" />

<br/>

<img src="https://img.shields.io/badge/ITLA-2017--C3-0057B8?style=for-the-badge" alt="ITLA 2017-C3" />

<br/><br/>

![Estado](https://img.shields.io/badge/Estado-1.1.0%20%7C%20Terminado-22C55E?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![React](https://img.shields.io/badge/React-19-61DAFB?style=for-the-badge&logo=react&logoColor=0B1220)
![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Architecture](https://img.shields.io/badge/Architecture-Clean-14B8A6?style=for-the-badge)
![Identity](https://img.shields.io/badge/ASP.NET_Core-Identity-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Accessibility](https://img.shields.io/badge/Accessibility-NORTIC_B2-0057B8?style=for-the-badge)

> Plataforma web para gestionar, analizar y simular el ciclo de vida del desarrollo de software, desde los requisitos y el diseño hasta las pruebas, la calidad y el mantenimiento.

</div>

## 📌 Descripción

**IngSoft Studio** es una plataforma web orientada a la gestión integral de proyectos de software. Centraliza requisitos, análisis, diseño, riesgos, métricas, pruebas, calidad, trazabilidad, mantenimiento y simulación de escenarios dentro de un único espacio de trabajo.

El proyecto nace como una reconstrucción moderna de los contenidos estudiados en **Introducción a la Ingeniería en Software (SOF-015)** del Instituto Tecnológico de Las Américas. La asignatura fue principalmente teórica; esta implementación convierte aquellos fundamentos en una aplicación real, modular y preparada como proyecto profesional de portafolio.

### 🛠️ Restaurando Proyectos Finales del ITLA

**IngSoft Studio** forma parte de la iniciativa **“Restaurando Proyectos Finales del ITLA”**, aunque este caso es diferente a los proyectos anteriores: aquí no existía una aplicación que restaurar.

El punto de partida fue un **trabajo académico teórico** realizado durante la etapa estudiantil en la asignatura **Introducción a la Ingeniería en Software (SOF-015)**. Años después, aquellos conceptos fueron retomados y transformados en una aplicación de software completa, moderna y funcional, preservando su contexto académico y llevándolo a un estándar actual de desarrollo profesional.

En este caso, “restaurar” significa **convertir conocimiento académico en software real**: requisitos, riesgos, pruebas, trazabilidad, calidad, métricas, reportes y toma de decisiones integrados dentro de una misma plataforma.

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
| 💻 Desarrollo | Planificación, estados y seguimiento del progreso |
| 🧪 Pruebas | Casos de prueba, ejecuciones, defectos, métricas y cobertura |
| 🚀 Despliegue | Reportes ejecutivos, criterios de liberación y decisión Go/No-Go |
| 🔄 Mantenimiento | Riesgos, incidencias, aprendizaje y mejora continua |

---

## 🚀 Funcionalidades

- 📊 Dashboard ejecutivo conectado a datos reales
- 📁 Gestión completa de proyectos
- 👥 Usuarios, roles y permisos
- 📋 Ingeniería de requisitos
- 🧭 Historias de usuario y casos de uso
- ✅ Criterios de aceptación y fuente del requisito
- ⚠️ Gestión de riesgos con probabilidad, impacto, score, estado y mitigación
- 🧪 Casos de prueba, ejecución y estados terminales
- 🐞 Gestión de defectos y severidad
- 🔗 Matriz de trazabilidad Requirement → Test Case → Defect
- 📐 Quality Center con cobertura, pass rate, defectos, riesgos y hallazgos consolidados
- 📊 Distribución visual de riesgos por nivel
- 📄 Reportes ejecutivos PDF y Excel
- 🚦 Evaluación de liberación `GO`, `REVISAR`, `NO-GO` o `SIN EVIDENCIA`
- 💯 Quality Score ejecutivo de 0 a 100
- 🏷️ Branding corporativo en reportes con logo oficial de IngSoft Studio
- 🕒 Reportes con zona horaria `America/Santo_Domingo`
- 📅 Fechas de reporte en formato `dd-MM-yyyy HH:mm`
- 🎮 Simulador de decisiones de Ingeniería de Software
- 🎓 Centro de aprendizaje con Ingeniería de Requisitos, Historias de Usuario, SOLID, pruebas, caja blanca/caja negra, riesgos, trazabilidad, control de cambios y release
- 📈 Indicadores y tendencias por proyecto
- ♿ Accesibilidad basada en NORTIC B2:2017
- 📱 Diseño responsive para escritorio, tableta y móvil
- 🔐 Hardening de seguridad para API

---

## 🧪 Quality Center

El **Quality Center** concentra la evidencia necesaria para evaluar el estado técnico de un proyecto y su preparación para liberación.

Incluye:

- métricas de requisitos y cobertura;
- pass rate de casos de prueba;
- defectos abiertos y resueltos;
- riesgos abiertos, aceptados y cerrados;
- clasificación de riesgos por exposición;
- consolidación ordenada de hallazgos;
- trazabilidad entre requisitos, pruebas y defectos;
- criterios de liberación basados en evidencia.

Los estados terminales se interpretan de forma consistente en las métricas: los defectos `Resolved`/`Closed` y los riesgos `Accepted`/`Closed` no se contabilizan como pendientes.

---

## 📄 Reportes ejecutivos

IngSoft Studio genera reportes de calidad en **PDF** y **Excel**.

### PDF

El reporte PDF incluye:

- logo corporativo de IngSoft Studio;
- resumen ejecutivo;
- KPIs de proyectos, requisitos, pruebas, cobertura, pass rate y pendientes;
- decisión de liberación;
- Quality Score;
- evidencia de calidad;
- detalle por proyecto;
- criterios de interpretación;
- numeración de páginas.

La fecha se genera con la zona horaria de **Santo Domingo (`America/Santo_Domingo`)** y se presenta en formato:

```text
DD-MM-AAAA HH:mm
```

Ejemplo:

```text
16-08-2026 11:43
```

Los nombres de los archivos siguen el mismo formato:

```text
ingsoft-studio-report-16-08-2026.pdf
ingsoft-studio-report-16-08-2026.xlsx
```

### Criterios de liberación

| Estado | Interpretación |
|---|---|
| 🟢 `GO` | Evidencia suficiente, sin defectos ni riesgos pendientes y criterios actuales satisfechos |
| 🟡 `REVISAR` | Existe cobertura incompleta, pruebas pendientes o riesgos abiertos |
| 🔴 `NO-GO` | Existen defectos abiertos que requieren evaluación antes de liberar |
| ⚪ `SIN EVIDENCIA` | No existen casos de prueba suficientes para sustentar una decisión |

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
- QuestPDF
- ClosedXML

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
- HTML semántico y ARIA puntual

### Base de datos

<p>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/microsoftsqlserver/microsoftsqlserver-plain.svg" alt="SQL Server" width="48" height="48" />
</p>

- Microsoft SQL Server
- Entity Framework Core Migrations
- LocalDB para desarrollo local
- Tablas de ASP.NET Core Identity integradas al mismo DbContext
- Persistencia de proyectos, requisitos, calidad y resultados del simulador

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
├── SECURITY.md
├── CHANGELOG.md
└── README.md
```

```text
Domain ← Application ← Infrastructure ← API
```

- **Domain:** entidades y reglas de negocio.
- **Application:** casos de uso, contratos y DTOs.
- **Infrastructure:** EF Core, SQL Server, Identity, reportes y servicios técnicos.
- **API:** endpoints REST, JWT, autorización, rate limiting, hardening, middleware y OpenAPI.
- **Frontend:** interfaz React modular, responsive y accesible.

---

## 🧱 Principios de desarrollo

- Clean Code
- SOLID
- DRY
- KISS
- Separación de responsabilidades
- Arquitectura modular
- Seguridad por diseño
- Accesibilidad por diseño
- Código mantenible y escalable

---

## ♿ Accesibilidad — NORTIC B2:2017

La interfaz fue adecuada tomando como referencia la **Norma sobre Accesibilidad Web del Estado Dominicano NORTIC B2:2017**, con objetivo técnico de cubrir los criterios **A y AA aplicables** al proyecto.

Entre las medidas implementadas se encuentran:

- skip link para saltar al contenido principal;
- navegación mediante teclado y foco visible;
- labels visibles y nombres accesibles en formularios;
- contraste reforzado y modo de alto contraste;
- escalado de texto hasta 200 %;
- títulos descriptivos por ruta;
- idioma principal `es`;
- mensajes de error textuales y regiones `aria-live`;
- confirmación antes de acciones destructivas;
- responsive/reflow para escritorio, tableta y móvil;
- soporte para `prefers-reduced-motion`;
- HTML semántico y uso limitado de ARIA a casos necesarios.

> La adecuación del proyecto a estos lineamientos no representa una certificación oficial.

Consulta el detalle técnico en [`docs/accessibility/NORTIC-B2-2017.md`](./docs/accessibility/NORTIC-B2-2017.md).

---

## 🔐 Seguridad y hardening

- ASP.NET Core Identity y JWT Bearer
- Política fuerte de contraseñas y lockout
- Roles y autorización
- CORS por origen permitido
- Rate limiting global
- HTTPS y HSTS fuera de Development
- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `Referrer-Policy: no-referrer`
- Content Security Policy para la API
- Permissions Policy restrictiva
- Problem Details y manejo centralizado de excepciones
- Logging estructurado con Serilog
- Endpoint de health check
- Secretos mediante configuración/variables de entorno o .NET User Secrets en desarrollo

Consulta [`SECURITY.md`](./SECURITY.md).

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

Las **6 fases están completadas**. IngSoft Studio dispone de una base full stack estable, autenticación, autorización, persistencia SQL Server, gestión de proyectos y requisitos, Quality Center, trazabilidad, simulación, reportes ejecutivos, aprendizaje, diseño responsive, accesibilidad y hardening de seguridad.

| Área | Estado | Detalle |
|---|---|---|
| Backend ASP.NET Core | ✅ | Domain, Application, Infrastructure y API |
| Frontend React | ✅ | Portada, autenticación, proyectos, Quality Center y Studio Insights |
| SQL Server | ✅ | Identity, proyectos, requisitos, calidad y simulación |
| GitHub Actions | ✅ | Build, pruebas de dominio, integración y frontend |
| Identidad y acceso | ✅ | Registro, login, JWT, roles, perfil y recuperación |
| Ingeniería de requisitos | ✅ | Tipos, MoSCoW, criterios y trazabilidad |
| Quality Center | ✅ | Riesgos, pruebas, defectos, hallazgos, métricas y estados terminales |
| Reportes | ✅ | PDF/Excel, Quality Score, Go/No-Go, branding y hora de Santo Domingo |
| Simulador | ✅ | Escenarios, feedback e historial persistente |
| Centro de aprendizaje | ✅ | 9 conceptos de Ingeniería de Software |
| Responsive | ✅ | Desktop, tablet y móvil |
| Accesibilidad | ✅ | NORTIC B2:2017 como referencia, objetivo A/AA aplicable |
| Seguridad | ✅ | Rate limit, HSTS, headers, CORS, Identity y JWT |
| Release | ✅ | Publicado como `1.1.0` |

---

## 🗺️ Roadmap

### ✅ Fase 1 — Fundación técnica
- [x] Arquitectura backend y frontend
- [x] SQL Server y migraciones
- [x] CI y pruebas base

### ✅ Fase 2 — Identidad y acceso
- [x] Registro, login y JWT
- [x] Roles y permisos
- [x] Perfil y cambio de contraseña
- [x] Recuperación de contraseña

### ✅ Fase 3 — Proyectos y requisitos
- [x] CRUD y ciclo de vida de proyectos
- [x] Requisitos funcionales/no funcionales
- [x] Historias de usuario y casos de uso
- [x] MoSCoW, estados y criterios de aceptación

### ✅ Fase 4 — Calidad y pruebas
- [x] Riesgos y métricas
- [x] Casos y ejecución de pruebas
- [x] Defectos y estados terminales
- [x] Hallazgos consolidados
- [x] Matriz de trazabilidad

### ✅ Fase 5 — Simulación y reportes
- [x] Dashboard real y tendencias
- [x] Simulador e historial
- [x] PDF y Excel
- [x] Quality Score y decisión de liberación
- [x] Branding y zona horaria de reportes
- [x] Centro de aprendizaje

### ✅ Fase 6 — Hardening y release
- [x] UX responsive
- [x] Accesibilidad basada en NORTIC B2:2017
- [x] Seguridad y hardening
- [x] Health checks y logging
- [x] Documentación final
- [x] Release `1.1.0` publicado

---

## ▶️ Ejecución local

### Requisitos

- .NET 10 SDK
- Node.js 22+
- SQL Server LocalDB o SQL Server Express
- Git

### Backend

El proyecto API está configurado con `UserSecretsId`, por lo que en desarrollo se recomienda utilizar **.NET User Secrets** en lugar de guardar credenciales en archivos versionados.

```powershell
cd backend/src/IngSoftStudio.Api

dotnet user-secrets set "Jwt:SigningKey" "TU_CLAVE_LOCAL_SEGURA_DE_AL_MENOS_32_CARACTERES"
dotnet user-secrets set "SeedAdmin:Email" "admin@ingsoftstudio.local"
dotnet user-secrets set "SeedAdmin:FullName" "Administrador IngSoft Studio"
dotnet user-secrets set "SeedAdmin:Password" "CambiaEstaClave123!"

dotnet user-secrets list
```

Luego:

```powershell
dotnet restore
dotnet ef database update --project ../IngSoftStudio.Infrastructure/IngSoftStudio.Infrastructure.csproj
dotnet run
```

> `UserSecretsId` puede versionarse de forma segura: identifica el almacén local de secretos, pero no contiene las credenciales.

### Pruebas

Desde la raíz del repositorio:

```powershell
dotnet build .\backend\src\IngSoftStudio.Api\IngSoftStudio.Api.csproj
dotnet test backend/tests/IngSoftStudio.Domain.Tests/IngSoftStudio.Domain.Tests.csproj
dotnet test backend/tests/IngSoftStudio.Api.IntegrationTests/IngSoftStudio.Api.IntegrationTests.csproj
```

### Frontend

```powershell
cd frontend
$env:VITE_API_URL="http://localhost:5000"
npm install
npm run lint
npm run build
npm run dev
```

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

## 📁 Proyectos y requisitos

| Método | Endpoint |
|---|---|
| GET/POST | `/api/v1/projects` |
| GET/PUT/DELETE | `/api/v1/projects/{id}` |
| PATCH | `/api/v1/projects/{id}/status` |
| GET/POST | `/api/v1/projects/{projectId}/requirements` |
| PUT/DELETE | `/api/v1/projects/{projectId}/requirements/{id}` |

## 📊 Studio y calidad

- `/api/v1/projects/{projectId}/quality`
- `/api/v1/studio/dashboard`
- `/api/v1/studio/projects`
- `/api/v1/studio/trends`
- `/api/v1/studio/simulation/scenarios`
- `/api/v1/studio/simulation/evaluate`
- `/api/v1/studio/simulation/summary`
- `/api/v1/studio/learning`
- `/api/v1/studio/reports/pdf`
- `/api/v1/studio/reports/excel`

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