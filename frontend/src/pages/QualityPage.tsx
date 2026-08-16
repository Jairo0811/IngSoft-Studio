import { FormEvent, useCallback, useEffect, useMemo, useState } from "react";
import {
  AlertTriangle,
  BarChart3,
  Beaker,
  Bug,
  Gauge,
  ShieldCheck,
} from "lucide-react";
import { Navigate, useLocation } from "react-router-dom";
import BrandLogo from "../components/BrandLogo";
import WorkspaceNav from "../components/WorkspaceNav";
import { authService } from "../services/auth";
import { Project, projectsService, Requirement } from "../services/projects";
import { QualityDashboard, qualityService } from "../services/quality";
import "./projects.css";
import "./quality.css";

export default function QualityPage() {
  const location = useLocation();

  const [projects, setProjects] = useState<Project[]>([]);
  const [requirements, setRequirements] = useState<Requirement[]>([]);
  const [projectId, setProjectId] = useState("");
  const [dashboard, setDashboard] = useState<QualityDashboard | null>(null);
  const [error, setError] = useState("");

  const loadProjects = useCallback(async () => {
    try {
      setError("");

      const data = await projectsService.list();

      setProjects(data);
      setProjectId(data[0]?.id || "");
    } catch {
      setError("No fue posible cargar los proyectos.");
    }
  }, []);

  const fetchQuality = useCallback(async (id: string) => {
    const [quality, reqs] = await Promise.all([
      qualityService.dashboard(id),
      projectsService.requirements(id),
    ]);

    setDashboard(quality);
    setRequirements(reqs);
  }, []);

  const loadQuality = useCallback(
    async (id: string) => {
      try {
        setError("");
        await fetchQuality(id);
      } catch {
        setError("No fue posible cargar el centro de calidad.");
      }
    },
    [fetchQuality],
  );

  useEffect(() => {
    void loadProjects();
  }, [loadProjects]);

  useEffect(() => {
    if (projectId) {
      void loadQuality(projectId);
      return;
    }

    setDashboard(null);
    setRequirements([]);
  }, [projectId, loadQuality]);

  async function refreshAfterMutation(message: string) {
    if (!projectId) return;

    try {
      await fetchQuality(projectId);
    } catch {
      setError(message);
    }
  }

  async function createRisk(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!projectId) return;

    const formElement = event.currentTarget;
    const form = new FormData(formElement);

    setError("");

    try {
      await qualityService.createRisk(projectId, {
        title: form.get("title"),
        description: form.get("description"),
        probability: Number(form.get("probability")),
        impact: Number(form.get("impact")),
        mitigation: form.get("mitigation"),
      });
    } catch {
      setError("No fue posible registrar el riesgo.");
      return;
    }

    formElement.reset();

    await refreshAfterMutation(
      "El riesgo fue registrado, pero no fue posible actualizar la vista. Recarga la página.",
    );
  }

  async function createTest(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!projectId) return;

    const formElement = event.currentTarget;
    const form = new FormData(formElement);

    setError("");

    try {
      await qualityService.createTest(projectId, {
        requirementId: form.get("requirementId") || null,
        title: form.get("title"),
        preconditions: form.get("preconditions"),
        steps: form.get("steps"),
        expectedResult: form.get("expectedResult"),
      });
    } catch {
      setError("No fue posible crear el caso de prueba.");
      return;
    }

    formElement.reset();

    await refreshAfterMutation(
      "El caso de prueba fue creado, pero no fue posible actualizar la vista. Recarga la página.",
    );
  }

  async function createDefect(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!projectId) return;

    const formElement = event.currentTarget;
    const form = new FormData(formElement);

    setError("");

    try {
      await qualityService.createDefect(projectId, {
        requirementId: form.get("requirementId") || null,
        testCaseId: form.get("testCaseId") || null,
        title: form.get("title"),
        description: form.get("description"),
        severity: Number(form.get("severity")),
        priority: Number(form.get("priority")),
      });
    } catch {
      setError("No fue posible registrar el defecto.");
      return;
    }

    formElement.reset();

    await refreshAfterMutation(
      "El defecto fue registrado, pero no fue posible actualizar la vista. Recarga la página.",
    );
  }

  async function changeRiskStatus(id: string, status: number) {
    if (!projectId) return;

    setError("");

    try {
      await qualityService.changeRiskStatus(projectId, id, status);
    } catch {
      setError("No fue posible actualizar el riesgo.");
      return;
    }

    await refreshAfterMutation(
      "El riesgo fue actualizado, pero no fue posible refrescar la vista. Recarga la página.",
    );
  }

  async function executeTest(id: string, status: number) {
    if (!projectId) return;

    const actualResult = window.prompt("Resultado real de la ejecución:");

    if (actualResult === null) return;

    setError("");

    try {
      await qualityService.executeTest(projectId, id, status, actualResult);
    } catch {
      setError("No fue posible ejecutar el caso de prueba.");
      return;
    }

    await refreshAfterMutation(
      "El caso de prueba fue ejecutado, pero no fue posible refrescar la vista. Recarga la página.",
    );
  }

  async function changeDefectStatus(id: string, status: number) {
    if (!projectId) return;

    setError("");

    try {
      await qualityService.changeDefectStatus(projectId, id, status);
    } catch {
      setError("No fue posible actualizar el defecto.");
      return;
    }

    await refreshAfterMutation(
      "El defecto fue actualizado, pero no fue posible refrescar la vista. Recarga la página.",
    );
  }

  const metrics = dashboard?.metrics;

  const activeRiskItems =
    dashboard?.risks.filter(
      (risk) => !["Accepted", "Closed", "Mitigated"].includes(risk.status),
    ) ?? [];

  const activeRisks = activeRiskItems.length;

  const highRiskCount = activeRiskItems.filter(
    (risk) => risk.impact === "High",
  ).length;

  const mediumRiskCount = activeRiskItems.filter(
    (risk) => risk.impact === "Medium",
  ).length;

  const tests = metrics?.totalTests ?? 0;
  const defects = metrics?.openDefects ?? 0;
  const coverage = metrics?.requirementCoveragePercent ?? 0;

  const riskSegments = useMemo(() => {
    if (activeRisks === 0) {
      return {
        high: 0,
        medium: 0,
        low: 0,
        denominator: 1,
      };
    }

    const high = Math.min(highRiskCount, activeRisks);

    const medium = Math.min(mediumRiskCount, Math.max(activeRisks - high, 0));

    const low = Math.max(activeRisks - high - medium, 0);

    return {
      high,
      medium,
      low,
      denominator: activeRisks,
    };
  }, [activeRisks, highRiskCount, mediumRiskCount]);

  const donutBackground =
    activeRisks === 0
      ? "conic-gradient(#243447 0 100%)"
      : `conic-gradient(
          #f97316 0 ${
            (riskSegments.high / riskSegments.denominator) * 100
          }%,
          #facc15 0 ${
            ((riskSegments.high + riskSegments.medium) /
              riskSegments.denominator) *
            100
          }%,
          #34d399 0
        )`;

  const totalFindings =
    (dashboard?.risks.length ?? 0) +
    (dashboard?.testCases.length ?? 0) +
    (dashboard?.defects.length ?? 0);

  if (!authService.hasToken()) {
    return <Navigate to="/auth" replace state={{ from: location.pathname }} />;
  }

  return (
    <main className="quality-app" aria-labelledby="quality-title">
      <aside
        className="quality-sidebar"
        aria-label="Menú de gestión de calidad"
      >
        <BrandLogo compact />

        <p className="quality-menu-label">Gestión de calidad</p>

        <nav className="quality-side-nav">
          <a href="#quality-summary" className="active">
            <Gauge />
            Resumen
          </a>

          <a href="#quality-risks-form">
            <AlertTriangle />
            Riesgos
          </a>

          <a href="#quality-tests">
            <Beaker />
            Casos de prueba
          </a>

          <a href="#quality-defects">
            <Bug />
            Defectos
          </a>

          <a href="#quality-metrics">
            <BarChart3 />
            Métricas
          </a>

          <a href="#quality-coverage">
            <ShieldCheck />
            Cobertura
          </a>
        </nav>

        <footer>
          © 2026 IngSoft Studio
          <br />
          <small>v1.1.0</small>
        </footer>
      </aside>

      <section className="quality-content">
        <WorkspaceNav />

        <header className="quality-hero" id="quality-summary">
          <div>
            <p className="eyebrow">Fase 4 · Calidad y trazabilidad</p>

            <h1 id="quality-title">
              Quality <span>Center</span>
            </h1>

            <p>
              Gestiona riesgos, casos de prueba, defectos, métricas y cobertura
              para asegurar la calidad del software.
            </p>
          </div>

          <div className="quality-hero-icon">
            <ShieldCheck aria-hidden="true" />
          </div>
        </header>

        {error && (
          <p className="workspace-error" role="alert" aria-live="assertive">
            {error}
          </p>
        )}

        <section
          className="quality-project-card"
          aria-labelledby="active-project-label"
        >
          <label id="active-project-label" htmlFor="quality-project">
            Proyecto activo
          </label>

          <select
            id="quality-project"
            value={projectId}
            onChange={(event) => setProjectId(event.target.value)}
          >
            <option value="">Selecciona un proyecto</option>

            {projects.map((project) => (
              <option key={project.id} value={project.id}>
                {project.name}
              </option>
            ))}
          </select>
        </section>

        {!projectId ? (
          <section className="quality-card">
            <h2>Aún no hay un proyecto activo</h2>

            <p>
              Crea o selecciona un proyecto en Proyectos para comenzar a
              gestionar calidad y trazabilidad.
            </p>
          </section>
        ) : (
          <>
            <section
              className="quality-kpis"
              id="quality-metrics"
              aria-label="Métricas de calidad"
            >
              <article className="quality-kpi green">
                <ShieldCheck />

                <div>
                  <span>Riesgos activos</span>
                  <strong>{activeRisks}</strong>

                  <small>{highRiskCount} de alta prioridad</small>
                </div>
              </article>

              <article className="quality-kpi blue">
                <Beaker />

                <div>
                  <span>Casos de prueba</span>
                  <strong>{tests}</strong>

                  <small>{metrics?.passedTests ?? 0} aprobados</small>
                </div>
              </article>

              <article className="quality-kpi purple">
                <Bug />

                <div>
                  <span>Defectos abiertos</span>
                  <strong>{defects}</strong>

                  <small>{metrics?.criticalDefects ?? 0} críticos</small>
                </div>
              </article>

              <article className="quality-kpi cyan">
                <BarChart3 />

                <div>
                  <span>Cobertura global</span>
                  <strong>{coverage}%</strong>

                  <small>
                    {metrics?.coveredRequirements ?? 0} requisitos cubiertos
                  </small>
                </div>
              </article>
            </section>

            <section className="quality-dashboard-grid">
              <article className="quality-card">
                <div className="quality-card-header">
                  <h2>Riesgos activos por impacto</h2>
                </div>

                <div className="risk-visual">
                  <div
                    className="risk-donut"
                    style={{
                      background: donutBackground,
                    }}
                  >
                    <div>
                      <strong>{activeRisks}</strong>
                      <span>Total</span>
                    </div>
                  </div>

                  <ul>
                    <li>
                      <i className="dot high" />
                      Altos <strong>{riskSegments.high}</strong>
                    </li>

                    <li>
                      <i className="dot medium" />
                      Medios <strong>{riskSegments.medium}</strong>
                    </li>

                    <li>
                      <i className="dot low" />
                      Bajos <strong>{riskSegments.low}</strong>
                    </li>
                  </ul>
                </div>
              </article>

              <article className="quality-card">
                <div className="quality-card-header">
                  <h2>Tendencia de defectos</h2>
                </div>

                <div className="empty-state">
                  <p>
                    El histórico temporal todavía no está disponible. Las
                    métricas actuales se muestran con datos reales del proyecto.
                  </p>
                </div>
              </article>
            </section>

            <section className="quality-forms-grid">
              <article className="quality-card" id="quality-risks-form">
                <h2>Registrar riesgo</h2>

                <form className="workspace-form" onSubmit={createRisk}>
                  <label htmlFor="risk-title">Riesgo</label>

                  <input id="risk-title" name="title" required />

                  <label htmlFor="risk-description">Descripción</label>

                  <textarea id="risk-description" name="description" />

                  <div className="quality-form-row">
                    <div>
                      <label htmlFor="risk-probability">Probabilidad</label>

                      <select id="risk-probability" name="probability">
                        <option value="1">Baja</option>
                        <option value="2">Media</option>
                        <option value="3">Alta</option>
                      </select>
                    </div>

                    <div>
                      <label htmlFor="risk-impact">Impacto</label>

                      <select id="risk-impact" name="impact">
                        <option value="1">Bajo</option>
                        <option value="2">Medio</option>
                        <option value="3">Alto</option>
                      </select>
                    </div>
                  </div>

                  <label htmlFor="risk-mitigation">Plan de mitigación</label>

                  <textarea id="risk-mitigation" name="mitigation" />

                  <button type="submit">Agregar riesgo</button>
                </form>
              </article>

              <article className="quality-card" id="quality-tests">
                <h2>Crear caso de prueba</h2>

                <form className="workspace-form" onSubmit={createTest}>
                  <label htmlFor="test-requirement">
                    Requisito relacionado
                  </label>

                  <select id="test-requirement" name="requirementId">
                    <option value="">Sin requisito</option>

                    {requirements.map((requirement) => (
                      <option key={requirement.id} value={requirement.id}>
                        {requirement.title}
                      </option>
                    ))}
                  </select>

                  <label htmlFor="test-title">Caso de prueba</label>

                  <input id="test-title" name="title" required />

                  <label htmlFor="test-preconditions">Precondiciones</label>

                  <textarea id="test-preconditions" name="preconditions" />

                  <label htmlFor="test-steps">Pasos</label>

                  <textarea id="test-steps" name="steps" />

                  <label htmlFor="test-expected">Resultado esperado</label>

                  <textarea id="test-expected" name="expectedResult" />

                  <button type="submit">Agregar caso</button>
                </form>
              </article>

              <article className="quality-card" id="quality-defects">
                <h2>Registrar defecto</h2>

                <form className="workspace-form" onSubmit={createDefect}>
                  <label htmlFor="defect-requirement">
                    Requisito relacionado
                  </label>

                  <select id="defect-requirement" name="requirementId">
                    <option value="">Sin requisito</option>

                    {requirements.map((requirement) => (
                      <option key={requirement.id} value={requirement.id}>
                        {requirement.title}
                      </option>
                    ))}
                  </select>

                  <label htmlFor="defect-test">
                    Caso de prueba relacionado
                  </label>

                  <select id="defect-test" name="testCaseId">
                    <option value="">Sin caso de prueba</option>

                    {dashboard?.testCases.map((test) => (
                      <option key={test.id} value={test.id}>
                        {test.title}
                      </option>
                    ))}
                  </select>

                  <label htmlFor="defect-title">Defecto</label>

                  <input id="defect-title" name="title" required />

                  <label htmlFor="defect-description">Descripción</label>

                  <textarea id="defect-description" name="description" />

                  <div className="quality-form-row">
                    <div>
                      <label htmlFor="defect-severity">Severidad</label>

                      <select id="defect-severity" name="severity">
                        <option value="1">Baja</option>
                        <option value="2">Media</option>
                        <option value="3">Alta</option>
                        <option value="4">Crítica</option>
                      </select>
                    </div>

                    <div>
                      <label htmlFor="defect-priority">Prioridad</label>

                      <select id="defect-priority" name="priority">
                        <option value="1">Baja</option>
                        <option value="2">Media</option>
                        <option value="3">Alta</option>
                        <option value="4">Urgente</option>
                      </select>
                    </div>
                  </div>

                  <button type="submit">Agregar defecto</button>
                </form>
              </article>
            </section>

            <section
              className="quality-card quality-findings"
              aria-labelledby="quality-findings-title"
            >
              <div className="quality-card-header quality-findings-header">
                <div>
                  <p className="quality-section-kicker">
                    Seguimiento de calidad
                  </p>

                  <h2 id="quality-findings-title">Riesgos encontrados</h2>
                </div>

                <span>
                  {totalFindings}{" "}
                  {totalFindings === 1 ? "resultado" : "resultados"}
                </span>
              </div>

              {totalFindings === 0 ? (
                <div className="quality-findings-empty">
                  <ShieldCheck aria-hidden="true" />

                  <div>
                    <h3>No hay resultados registrados</h3>

                    <p>
                      Los riesgos, casos de prueba y defectos del proyecto
                      aparecerán aquí.
                    </p>
                  </div>
                </div>
              ) : (
                <div className="quality-findings-list">
                  {dashboard?.risks.map((risk, index) => {
                    const riskIsTerminal =
                      risk.status === "Accepted" ||
                      risk.status === "Closed" ||
                      risk.status === "Mitigated";

                    return (
                      <article
                        key={`risk-${risk.id}`}
                        className="quality-finding quality-finding--risk"
                      >
                        <div className="quality-finding-heading">
                          <div className="quality-finding-identity">
                            <span className="quality-finding-number">
                              {String(index + 1).padStart(2, "0")}
                            </span>

                            <div>
                              <span className="quality-finding-type">
                                Riesgo
                              </span>

                              <span className="quality-finding-category">
                                Gestión de riesgos
                              </span>
                            </div>
                          </div>

                          <AlertTriangle
                            className="quality-finding-icon"
                            aria-hidden="true"
                          />
                        </div>

                        <div className="requirement-meta">
                          <span>{risk.status}</span>
                          <span>Impacto {risk.impact}</span>
                          <span>Score {risk.score}</span>
                        </div>

                        <h3>{risk.title}</h3>

                        <p>{risk.description || "Sin descripción."}</p>

                        {risk.mitigation && (
                          <div className="quality-finding-detail">
                            <strong>Plan de mitigación</strong>

                            <p>{risk.mitigation}</p>
                          </div>
                        )}

                        {!riskIsTerminal ? (
                          <div className="requirement-actions">
                            <button
                              type="button"
                              onClick={() => void changeRiskStatus(risk.id, 2)}
                            >
                              Mitigar
                            </button>

                            <button
                              type="button"
                              onClick={() => void changeRiskStatus(risk.id, 3)}
                            >
                              Cerrar
                            </button>

                            <button
                              type="button"
                              onClick={() => void changeRiskStatus(risk.id, 4)}
                            >
                              Aceptar
                            </button>
                          </div>
                        ) : (
                          <p className="quality-finding-state-note">
                            Estado final registrado. No hay acciones pendientes.
                          </p>
                        )}
                      </article>
                    );
                  })}

                  {dashboard?.testCases.map((test, index) => {
                    const offset = dashboard?.risks.length ?? 0;

                    const testIsTerminal =
                      test.status === "Passed" ||
                      test.status === "Failed" ||
                      test.status === "Blocked";

                    return (
                      <article
                        key={`test-${test.id}`}
                        className="quality-finding quality-finding--test"
                      >
                        <div className="quality-finding-heading">
                          <div className="quality-finding-identity">
                            <span className="quality-finding-number">
                              {String(offset + index + 1).padStart(2, "0")}
                            </span>

                            <div>
                              <span className="quality-finding-type">
                                Caso de prueba
                              </span>

                              <span className="quality-finding-category">
                                Validación funcional
                              </span>
                            </div>
                          </div>

                          <Beaker
                            className="quality-finding-icon"
                            aria-hidden="true"
                          />
                        </div>

                        <div className="requirement-meta">
                          <span>{test.status}</span>
                        </div>

                        <h3>{test.title}</h3>

                        <div className="quality-finding-detail">
                          <strong>Resultado esperado</strong>

                          <p>
                            {test.expectedResult || "Sin resultado esperado."}
                          </p>
                        </div>

                        {test.actualResult && (
                          <div className="quality-finding-detail">
                            <strong>Resultado real</strong>

                            <p>{test.actualResult}</p>
                          </div>
                        )}

                        {!testIsTerminal ? (
                          <div className="requirement-actions">
                            <button
                              type="button"
                              onClick={() => void executeTest(test.id, 2)}
                            >
                              Aprobar
                            </button>

                            <button
                              type="button"
                              onClick={() => void executeTest(test.id, 3)}
                            >
                              Fallar
                            </button>

                            <button
                              type="button"
                              onClick={() => void executeTest(test.id, 4)}
                            >
                              Bloquear
                            </button>
                          </div>
                        ) : (
                          <p className="quality-finding-state-note">
                            Ejecución finalizada. No hay acciones pendientes.
                          </p>
                        )}
                      </article>
                    );
                  })}

                  {dashboard?.defects.map((defect, index) => {
                    const offset =
                      (dashboard?.risks.length ?? 0) +
                      (dashboard?.testCases.length ?? 0);

                    return (
                      <article
                        key={`defect-${defect.id}`}
                        className="quality-finding quality-finding--defect"
                      >
                        <div className="quality-finding-heading">
                          <div className="quality-finding-identity">
                            <span className="quality-finding-number">
                              {String(offset + index + 1).padStart(2, "0")}
                            </span>

                            <div>
                              <span className="quality-finding-type">
                                Defecto
                              </span>

                              <span className="quality-finding-category">
                                Seguimiento de calidad
                              </span>
                            </div>
                          </div>

                          <Bug
                            className="quality-finding-icon"
                            aria-hidden="true"
                          />
                        </div>

                        <div className="requirement-meta">
                          <span>{defect.status}</span>
                          <span>{defect.severity}</span>
                          <span>{defect.priority}</span>
                        </div>

                        <h3>{defect.title}</h3>

                        {defect.status === "Open" && (
                          <div className="requirement-actions">
                            <button
                              type="button"
                              onClick={() =>
                                void changeDefectStatus(defect.id, 2)
                              }
                            >
                              En progreso
                            </button>
                          </div>
                        )}

                        {defect.status === "InProgress" && (
                          <div className="requirement-actions">
                            <button
                              type="button"
                              onClick={() =>
                                void changeDefectStatus(defect.id, 3)
                              }
                            >
                              Resolver
                            </button>
                          </div>
                        )}

                        {defect.status === "Resolved" && (
                          <div className="requirement-actions">
                            <button
                              type="button"
                              onClick={() =>
                                void changeDefectStatus(defect.id, 4)
                              }
                            >
                              Cerrar
                            </button>
                          </div>
                        )}

                        {defect.status === "Closed" && (
                          <p className="quality-finding-state-note">
                            Defecto cerrado. No hay acciones pendientes.
                          </p>
                        )}
                      </article>
                    );
                  })}
                </div>
              )}
            </section>

            <section
              className="quality-card quality-traceability"
              id="quality-coverage"
              aria-labelledby="traceability-title"
            >
              <div className="quality-card-header">
                <h2 id="traceability-title">
                  Cobertura y matriz de trazabilidad
                </h2>

                <span>{coverage}% global</span>
              </div>

              <div className="requirement-list">
                {dashboard?.traceability.length ? (
                  dashboard.traceability.map((row) => (
                    <article
                      key={row.requirementId}
                      className="requirement-card"
                    >
                      <div className="requirement-meta">
                        <span>{row.requirementStatus}</span>

                        <span>
                          {row.covered ? "Cubierto" : "Sin cobertura"}
                        </span>
                      </div>

                      <h3>{row.requirementTitle}</h3>

                      <p>
                        {row.testCases} pruebas · {row.passedTests} aprobadas ·{" "}
                        {row.failedTests} fallidas · {row.openDefects} defectos
                        abiertos
                      </p>
                    </article>
                  ))
                ) : (
                  <p>No hay requisitos para construir trazabilidad.</p>
                )}
              </div>
            </section>
          </>
        )}
      </section>
    </main>
  );
}
