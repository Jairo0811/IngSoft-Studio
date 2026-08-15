import { FormEvent, useEffect, useMemo, useState } from 'react'
import { AlertTriangle, BarChart3, Beaker, Bug, Gauge, ShieldCheck } from 'lucide-react'
import { Navigate, useLocation } from 'react-router-dom'
import BrandLogo from '../components/BrandLogo'
import WorkspaceNav from '../components/WorkspaceNav'
import { authService } from '../services/auth'
import { Project, projectsService, Requirement } from '../services/projects'
import { QualityDashboard, qualityService } from '../services/quality'
import './projects.css'
import './quality.css'

export default function QualityPage() {
  const location = useLocation()
  const [projects, setProjects] = useState<Project[]>([])
  const [requirements, setRequirements] = useState<Requirement[]>([])
  const [projectId, setProjectId] = useState('')
  const [dashboard, setDashboard] = useState<QualityDashboard | null>(null)
  const [error, setError] = useState('')

  useEffect(() => { void loadProjects() }, [])
  useEffect(() => { if (projectId) void loadQuality(projectId); else setDashboard(null) }, [projectId])

  async function loadProjects() {
    try {
      const data = await projectsService.list()
      setProjects(data)
      setProjectId(data[0]?.id || '')
    } catch {
      setError('No fue posible cargar los proyectos.')
    }
  }

  async function loadQuality(id: string) {
    try {
      setError('')
      const [quality, reqs] = await Promise.all([qualityService.dashboard(id), projectsService.requirements(id)])
      setDashboard(quality)
      setRequirements(reqs)
    } catch {
      setError('No fue posible cargar el centro de calidad.')
    }
  }

  async function createRisk(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!projectId) return
    try {
      const form = new FormData(event.currentTarget)
      await qualityService.createRisk(projectId, { title: form.get('title'), description: form.get('description'), probability: Number(form.get('probability')), impact: Number(form.get('impact')), mitigation: form.get('mitigation') })
      event.currentTarget.reset()
      await loadQuality(projectId)
    } catch { setError('No fue posible registrar el riesgo.') }
  }

  async function createTest(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!projectId) return
    try {
      const form = new FormData(event.currentTarget)
      await qualityService.createTest(projectId, { requirementId: form.get('requirementId') || null, title: form.get('title'), preconditions: form.get('preconditions'), steps: form.get('steps'), expectedResult: form.get('expectedResult') })
      event.currentTarget.reset()
      await loadQuality(projectId)
    } catch { setError('No fue posible crear el caso de prueba.') }
  }

  async function createDefect(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!projectId) return
    try {
      const form = new FormData(event.currentTarget)
      await qualityService.createDefect(projectId, { requirementId: form.get('requirementId') || null, testCaseId: form.get('testCaseId') || null, title: form.get('title'), description: form.get('description'), severity: Number(form.get('severity')), priority: Number(form.get('priority')) })
      event.currentTarget.reset()
      await loadQuality(projectId)
    } catch { setError('No fue posible registrar el defecto.') }
  }

  async function changeRiskStatus(id: string, status: number) {
    if (!projectId) return
    try { await qualityService.changeRiskStatus(projectId, id, status); await loadQuality(projectId) } catch { setError('No fue posible actualizar el riesgo.') }
  }

  async function executeTest(id: string, status: number) {
    if (!projectId) return
    const actualResult = window.prompt('Resultado real de la ejecución:') ?? ''
    try { await qualityService.executeTest(projectId, id, status, actualResult); await loadQuality(projectId) } catch { setError('No fue posible ejecutar el caso de prueba.') }
  }

  async function changeDefectStatus(id: string, status: number) {
    if (!projectId) return
    try { await qualityService.changeDefectStatus(projectId, id, status); await loadQuality(projectId) } catch { setError('No fue posible actualizar el defecto.') }
  }

  const metrics = dashboard?.metrics
  const activeRisks = metrics?.openRisks ?? 0
  const tests = metrics?.totalTests ?? 0
  const defects = metrics?.openDefects ?? 0
  const coverage = metrics?.requirementCoveragePercent ?? 0
  const highRiskCount = dashboard?.risks.filter((item) => item.status !== 'Closed' && item.impact === 'High').length ?? 0
  const mediumRiskCount = dashboard?.risks.filter((item) => item.status !== 'Closed' && item.impact === 'Medium').length ?? 0
  const riskSegments = useMemo(() => {
    if (activeRisks === 0) return { critical: 0, high: 0, medium: 0, low: 0, denominator: 1 }
    const critical = Math.min(metrics?.highRisks ?? 0, activeRisks)
    const high = Math.min(highRiskCount, Math.max(activeRisks - critical, 0))
    const medium = Math.min(mediumRiskCount, Math.max(activeRisks - critical - high, 0))
    const low = Math.max(activeRisks - critical - high - medium, 0)
    return { critical, high, medium, low, denominator: activeRisks }
  }, [activeRisks, highRiskCount, mediumRiskCount, metrics?.highRisks])

  const donutBackground = activeRisks === 0
    ? 'conic-gradient(#243447 0 100%)'
    : `conic-gradient(#ef4444 0 ${(riskSegments.critical / riskSegments.denominator) * 100}%,#f97316 0 ${((riskSegments.critical + riskSegments.high) / riskSegments.denominator) * 100}%,#facc15 0 ${((riskSegments.critical + riskSegments.high + riskSegments.medium) / riskSegments.denominator) * 100}%,#34d399 0)`

  if (!authService.hasToken()) return <Navigate to="/auth" replace state={{ from: location.pathname }} />

  return <main className="quality-app" aria-labelledby="quality-title">
    <aside className="quality-sidebar" aria-label="Menú de gestión de calidad">
      <BrandLogo compact />
      <p className="quality-menu-label">Gestión de calidad</p>
      <nav className="quality-side-nav">
        <a href="#quality-summary" className="active"><Gauge />Resumen</a>
        <a href="#quality-risks"><AlertTriangle />Riesgos</a>
        <a href="#quality-tests"><Beaker />Casos de prueba</a>
        <a href="#quality-defects"><Bug />Defectos</a>
        <a href="#quality-metrics"><BarChart3 />Métricas</a>
        <a href="#quality-coverage"><ShieldCheck />Cobertura</a>
      </nav>
      <footer>© 2026 IngSoft Studio<br /><small>v1.1.0</small></footer>
    </aside>

    <section className="quality-content">
      <WorkspaceNav />
      <header className="quality-hero" id="quality-summary"><div><p className="eyebrow">Fase 4 · Calidad y trazabilidad</p><h1 id="quality-title">Quality <span>Center</span></h1><p>Gestiona riesgos, casos de prueba, defectos, métricas y cobertura para asegurar la calidad del software.</p></div><div className="quality-hero-icon"><ShieldCheck aria-hidden="true" /></div></header>
      {error && <p className="workspace-error" role="alert" aria-live="assertive">{error}</p>}

      <section className="quality-project-card" aria-labelledby="active-project-label"><label id="active-project-label" htmlFor="quality-project">Proyecto activo</label><select id="quality-project" value={projectId} onChange={(event) => setProjectId(event.target.value)}><option value="">Selecciona un proyecto</option>{projects.map((project) => <option key={project.id} value={project.id}>{project.name}</option>)}</select></section>

      {!projectId ? <section className="quality-card"><h2>Aún no hay un proyecto activo</h2><p>Crea o selecciona un proyecto en Proyectos para comenzar a gestionar calidad y trazabilidad.</p></section> : <>
        <section className="quality-kpis" id="quality-metrics" aria-label="Métricas de calidad">
          <article className="quality-kpi green"><ShieldCheck /><div><span>Riesgos activos</span><strong>{activeRisks}</strong><small>{metrics?.highRisks ?? 0} de alta prioridad</small></div></article>
          <article className="quality-kpi blue"><Beaker /><div><span>Casos de prueba</span><strong>{tests}</strong><small>{metrics?.passedTests ?? 0} aprobados</small></div></article>
          <article className="quality-kpi purple"><Bug /><div><span>Defectos abiertos</span><strong>{defects}</strong><small>{metrics?.criticalDefects ?? 0} críticos</small></div></article>
          <article className="quality-kpi cyan"><BarChart3 /><div><span>Cobertura global</span><strong>{coverage}%</strong><small>{metrics?.coveredRequirements ?? 0} requisitos cubiertos</small></div></article>
        </section>

        <section className="quality-dashboard-grid">
          <article className="quality-card" id="quality-risks"><div className="quality-card-header"><h2>Estado de riesgos</h2></div><div className="risk-visual"><div className="risk-donut" style={{ background: donutBackground }}><div><strong>{activeRisks}</strong><span>Total</span></div></div><ul><li><i className="dot critical" />Críticos <strong>{riskSegments.critical}</strong></li><li><i className="dot high" />Altos <strong>{riskSegments.high}</strong></li><li><i className="dot medium" />Medios <strong>{riskSegments.medium}</strong></li><li><i className="dot low" />Bajos <strong>{riskSegments.low}</strong></li></ul></div></article>
          <article className="quality-card"><div className="quality-card-header"><h2>Tendencia de defectos</h2></div><div className="empty-state"><p>El histórico temporal todavía no está disponible. Las métricas actuales se muestran con datos reales del proyecto.</p></div></article>
        </section>

        <section className="quality-card"><h2>Riesgos registrados</h2>{dashboard?.risks.length ? <div className="requirement-list">{dashboard.risks.map((risk) => <article key={risk.id} className="requirement-card"><div className="requirement-meta"><span>{risk.status}</span><span>Impacto {risk.impact}</span><span>Score {risk.score}</span></div><h3>{risk.title}</h3><p>{risk.description || 'Sin descripción.'}</p><div className="requirement-actions"><button type="button" onClick={() => void changeRiskStatus(risk.id, 2)}>Mitigar</button><button type="button" onClick={() => void changeRiskStatus(risk.id, 3)}>Cerrar</button><button type="button" onClick={() => void changeRiskStatus(risk.id, 4)}>Aceptar</button></div></article>)}</div> : <p>No hay riesgos registrados.</p>}</section>

        <section className="quality-forms-grid">
          <article className="quality-card" id="quality-risks-form"><h2>Registrar riesgo</h2><form className="workspace-form" onSubmit={createRisk}><label htmlFor="risk-title">Riesgo</label><input id="risk-title" name="title" required /><label htmlFor="risk-description">Descripción</label><textarea id="risk-description" name="description" /><div className="quality-form-row"><div><label htmlFor="risk-probability">Probabilidad</label><select id="risk-probability" name="probability"><option value="1">Baja</option><option value="2">Media</option><option value="3">Alta</option></select></div><div><label htmlFor="risk-impact">Impacto</label><select id="risk-impact" name="impact"><option value="1">Bajo</option><option value="2">Medio</option><option value="3">Alto</option></select></div></div><label htmlFor="risk-mitigation">Plan de mitigación</label><textarea id="risk-mitigation" name="mitigation" /><button type="submit">Agregar riesgo</button></form></article>
          <article className="quality-card" id="quality-tests"><h2>Crear caso de prueba</h2><form className="workspace-form" onSubmit={createTest}><label htmlFor="test-requirement">Requisito relacionado</label><select id="test-requirement" name="requirementId"><option value="">Sin requisito</option>{requirements.map((r) => <option key={r.id} value={r.id}>{r.title}</option>)}</select><label htmlFor="test-title">Caso de prueba</label><input id="test-title" name="title" required /><label htmlFor="test-preconditions">Precondiciones</label><textarea id="test-preconditions" name="preconditions" /><label htmlFor="test-steps">Pasos</label><textarea id="test-steps" name="steps" /><label htmlFor="test-expected">Resultado esperado</label><textarea id="test-expected" name="expectedResult" /><button type="submit">Agregar caso</button></form>{dashboard?.testCases.length ? <div className="requirement-list">{dashboard.testCases.map((test) => <article key={test.id} className="requirement-card"><div className="requirement-meta"><span>{test.status}</span></div><h3>{test.title}</h3><p>Esperado: {test.expectedResult || 'Sin resultado esperado.'}</p>{test.actualResult && <p>Real: {test.actualResult}</p>}<div className="requirement-actions"><button type="button" onClick={() => void executeTest(test.id, 2)}>Aprobar</button><button type="button" onClick={() => void executeTest(test.id, 3)}>Fallar</button><button type="button" onClick={() => void executeTest(test.id, 4)}>Bloquear</button></div></article>)}</div> : <p>No hay casos de prueba registrados.</p>}</article>
          <article className="quality-card" id="quality-defects-form"><h2>Registrar defecto</h2><form className="workspace-form" onSubmit={createDefect}><label htmlFor="defect-requirement">Requisito relacionado</label><select id="defect-requirement" name="requirementId"><option value="">Sin requisito</option>{requirements.map((r) => <option key={r.id} value={r.id}>{r.title}</option>)}</select><label htmlFor="defect-test">Caso de prueba relacionado</label><select id="defect-test" name="testCaseId"><option value="">Sin caso de prueba</option>{dashboard?.testCases.map((t) => <option key={t.id} value={t.id}>{t.title}</option>)}</select><label htmlFor="defect-title">Defecto</label><input id="defect-title" name="title" required /><label htmlFor="defect-description">Descripción</label><textarea id="defect-description" name="description" /><div className="quality-form-row"><div><label htmlFor="defect-severity">Severidad</label><select id="defect-severity" name="severity"><option value="1">Baja</option><option value="2">Media</option><option value="3">Alta</option><option value="4">Crítica</option></select></div><div><label htmlFor="defect-priority">Prioridad</label><select id="defect-priority" name="priority"><option value="1">Baja</option><option value="2">Media</option><option value="3">Alta</option><option value="4">Urgente</option></select></div></div><button type="submit">Agregar defecto</button></form>{dashboard?.defects.length ? <div className="requirement-list">{dashboard.defects.map((defect) => <article key={defect.id} className="requirement-card"><div className="requirement-meta"><span>{defect.status}</span><span>{defect.severity}</span><span>{defect.priority}</span></div><h3>{defect.title}</h3><div className="requirement-actions"><button type="button" onClick={() => void changeDefectStatus(defect.id, 2)}>En progreso</button><button type="button" onClick={() => void changeDefectStatus(defect.id, 3)}>Resolver</button><button type="button" onClick={() => void changeDefectStatus(defect.id, 4)}>Cerrar</button></div></article>)}</div> : <p>No hay defectos registrados.</p>}</article>
        </section>

        <section className="quality-card quality-traceability" id="quality-coverage" aria-labelledby="traceability-title"><div className="quality-card-header"><h2 id="traceability-title">Cobertura y matriz de trazabilidad</h2><span>{coverage}% global</span></div><div className="requirement-list">{dashboard?.traceability.length ? dashboard.traceability.map((row) => <article key={row.requirementId} className="requirement-card"><div className="requirement-meta"><span>{row.requirementStatus}</span><span>{row.covered ? 'Cubierto' : 'Sin cobertura'}</span></div><h3>{row.requirementTitle}</h3><p>{row.testCases} pruebas · {row.passedTests} aprobadas · {row.failedTests} fallidas · {row.openDefects} defectos abiertos</p></article>) : <p>No hay requisitos para construir trazabilidad.</p>}</div></section>
      </>}
    </section>
  </main>
}
