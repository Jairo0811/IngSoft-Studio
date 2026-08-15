import { FormEvent, useEffect, useMemo, useState } from 'react'
import { AlertTriangle, BarChart3, Beaker, Bug, Download, FileText, Gauge, LifeBuoy, Settings, ShieldCheck, SlidersHorizontal } from 'lucide-react'
import { Navigate, useLocation } from 'react-router-dom'
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
  useEffect(() => { if (projectId) void loadQuality(projectId) }, [projectId])
  if (!authService.hasToken()) return <Navigate to="/auth" replace state={{ from: location.pathname }} />

  async function loadProjects() { try { const data = await projectsService.list(); setProjects(data); setProjectId(data[0]?.id || '') } catch { setError('No fue posible cargar los proyectos.') } }
  async function loadQuality(id: string) { try { setError(''); const [quality, reqs] = await Promise.all([qualityService.dashboard(id), projectsService.requirements(id)]); setDashboard(quality); setRequirements(reqs) } catch { setError('No fue posible cargar el centro de calidad.') } }
  async function createRisk(event: FormEvent<HTMLFormElement>) { event.preventDefault(); if (!projectId) return; const form = new FormData(event.currentTarget); await qualityService.createRisk(projectId, { title: form.get('title'), description: form.get('description'), probability: Number(form.get('probability')), impact: Number(form.get('impact')), mitigation: form.get('mitigation') }); event.currentTarget.reset(); await loadQuality(projectId) }
  async function createTest(event: FormEvent<HTMLFormElement>) { event.preventDefault(); if (!projectId) return; const form = new FormData(event.currentTarget); await qualityService.createTest(projectId, { requirementId: form.get('requirementId') || null, title: form.get('title'), preconditions: form.get('preconditions'), steps: form.get('steps'), expectedResult: form.get('expectedResult') }); event.currentTarget.reset(); await loadQuality(projectId) }
  async function createDefect(event: FormEvent<HTMLFormElement>) { event.preventDefault(); if (!projectId) return; const form = new FormData(event.currentTarget); await qualityService.createDefect(projectId, { requirementId: form.get('requirementId') || null, testCaseId: form.get('testCaseId') || null, title: form.get('title'), description: form.get('description'), severity: Number(form.get('severity')), priority: Number(form.get('priority')) }); event.currentTarget.reset(); await loadQuality(projectId) }

  const metrics = dashboard?.metrics
  const activeRisks = metrics?.openRisks ?? 0
  const tests = metrics?.totalTests ?? 0
  const defects = metrics?.openDefects ?? 0
  const coverage = metrics?.requirementCoveragePercent ?? 0
  const highRiskCount = dashboard?.risks.filter((item) => ['High', 'Alta', 'Critical', 'Crítica'].includes(item.impact)).length ?? 0
  const mediumRiskCount = dashboard?.risks.filter((item) => ['Medium', 'Media'].includes(item.impact)).length ?? 0
  const lowRiskCount = Math.max(activeRisks - highRiskCount - mediumRiskCount, 0)
  const riskSegments = useMemo(() => {
    const total = Math.max(activeRisks, 1)
    const critical = Math.min(metrics?.highRisks ?? 0, total)
    const high = Math.min(highRiskCount, Math.max(total - critical, 0))
    const medium = Math.min(mediumRiskCount, Math.max(total - critical - high, 0))
    const low = Math.max(total - critical - high - medium, 0)
    return { critical, high, medium, low, total }
  }, [activeRisks, highRiskCount, mediumRiskCount, metrics?.highRisks])

  return <main className="quality-app" aria-labelledby="quality-title">
    <aside className="quality-sidebar" aria-label="Menú de gestión de calidad">
      <div className="quality-brand"><div className="quality-brand-mark">&lt;IS&gt;</div><div><strong>Ing<span>Soft</span></strong><small>STUDIO</small></div></div>
      <p className="quality-menu-label">Gestión de calidad</p>
      <nav className="quality-side-nav">
        <a href="#quality-summary" className="active"><Gauge />Resumen</a><a href="#quality-risks"><AlertTriangle />Riesgos</a><a href="#quality-tests"><Beaker />Casos de prueba</a><a href="#quality-defects"><Bug />Defectos</a><a href="#quality-metrics"><BarChart3 />Métricas</a><a href="#quality-coverage"><ShieldCheck />Cobertura</a>
      </nav>
      <div className="quality-side-divider" />
      <p className="quality-menu-label">Administración</p>
      <nav className="quality-side-nav"><a href="#quality-settings"><Settings />Configuración</a><a href="#quality-templates"><FileText />Plantillas</a><a href="#quality-parameters"><SlidersHorizontal />Parámetros</a><a href="#quality-export"><Download />Exportar datos</a></nav>
      <div className="quality-help"><LifeBuoy /><div><strong>¿Necesitas ayuda?</strong><p>Consulta la documentación o contacta al soporte.</p><button type="button">Centro de ayuda</button></div></div>
      <footer>© 2026 IngSoft Studio<br /><small>v1.1.0</small></footer>
    </aside>

    <section className="quality-content">
      <WorkspaceNav />
      <header className="quality-hero" id="quality-summary"><div><p className="eyebrow">Fase 4 · Calidad y trazabilidad</p><h1 id="quality-title">Quality <span>Center</span></h1><p>Gestiona riesgos, casos de prueba, defectos, métricas y cobertura para asegurar la calidad del software.</p></div><div className="quality-hero-icon"><ShieldCheck aria-hidden="true" /></div></header>
      {error && <p className="workspace-error" role="alert" aria-live="assertive">{error}</p>}

      <section className="quality-project-card" aria-labelledby="active-project-label"><label id="active-project-label" htmlFor="quality-project">Proyecto activo</label><select id="quality-project" value={projectId} onChange={(event) => setProjectId(event.target.value)}>{projects.map((project) => <option key={project.id} value={project.id}>{project.name}</option>)}</select></section>

      <section className="quality-kpis" id="quality-metrics" aria-label="Métricas de calidad">
        <article className="quality-kpi green"><ShieldCheck /><div><span>Riesgos activos</span><strong>{activeRisks}</strong><small>{metrics?.highRisks ?? 0} de alta prioridad</small></div></article>
        <article className="quality-kpi blue"><Beaker /><div><span>Casos de prueba</span><strong>{tests}</strong><small>{metrics?.passedTests ?? 0} aprobados</small></div></article>
        <article className="quality-kpi purple"><Bug /><div><span>Defectos abiertos</span><strong>{defects}</strong><small>{metrics?.criticalDefects ?? 0} críticos</small></div></article>
        <article className="quality-kpi cyan"><BarChart3 /><div><span>Cobertura global</span><strong>{coverage}%</strong><small>{metrics?.coveredRequirements ?? 0} requisitos cubiertos</small></div></article>
      </section>

      <section className="quality-dashboard-grid">
        <article className="quality-card" id="quality-risks"><div className="quality-card-header"><h2>Estado de riesgos</h2></div><div className="risk-visual"><div className="risk-donut" style={{ background: `conic-gradient(#ef4444 0 ${(riskSegments.critical / riskSegments.total) * 100}%,#f97316 0 ${((riskSegments.critical + riskSegments.high) / riskSegments.total) * 100}%,#facc15 0 ${((riskSegments.critical + riskSegments.high + riskSegments.medium) / riskSegments.total) * 100}%,#34d399 0)` }}><div><strong>{activeRisks}</strong><span>Total</span></div></div><ul><li><i className="dot critical" />Críticos <strong>{riskSegments.critical}</strong></li><li><i className="dot high" />Altos <strong>{riskSegments.high}</strong></li><li><i className="dot medium" />Medios <strong>{riskSegments.medium}</strong></li><li><i className="dot low" />Bajos <strong>{riskSegments.low}</strong></li></ul></div></article>
        <article className="quality-card" id="quality-defects"><div className="quality-card-header"><h2>Tendencia de defectos</h2><span>Estado actual</span></div><div className="defect-trend"><div className="trend-line" /><div className="trend-points"><span style={{ left: '7%', bottom: '55%' }} /><span style={{ left: '25%', bottom: '43%' }} /><span style={{ left: '43%', bottom: '34%' }} /><span style={{ left: '61%', bottom: '48%' }} /><span style={{ left: '79%', bottom: '28%' }} /><span style={{ left: '94%', bottom: '20%' }} /></div><div className="trend-labels"><span>Ene</span><span>Feb</span><span>Mar</span><span>Abr</span><span>May</span><span>Jun</span></div></div></article>
      </section>

      <section className="quality-forms-grid">
        <article className="quality-card" id="quality-risks-form"><h2>Registrar riesgo</h2><form className="workspace-form" onSubmit={createRisk}><label htmlFor="risk-title">Riesgo</label><input id="risk-title" name="title" required /><label htmlFor="risk-description">Descripción</label><textarea id="risk-description" name="description" /><div className="quality-form-row"><div><label htmlFor="risk-probability">Probabilidad</label><select id="risk-probability" name="probability"><option value="1">Baja</option><option value="2">Media</option><option value="3">Alta</option></select></div><div><label htmlFor="risk-impact">Impacto</label><select id="risk-impact" name="impact"><option value="1">Bajo</option><option value="2">Medio</option><option value="3">Alto</option></select></div></div><label htmlFor="risk-mitigation">Plan de mitigación</label><textarea id="risk-mitigation" name="mitigation" /><button type="submit">Agregar riesgo</button></form></article>
        <article className="quality-card" id="quality-tests"><h2>Crear caso de prueba</h2><form className="workspace-form" onSubmit={createTest}><label htmlFor="test-requirement">Requisito relacionado</label><select id="test-requirement" name="requirementId"><option value="">Sin requisito</option>{requirements.map((r) => <option key={r.id} value={r.id}>{r.title}</option>)}</select><label htmlFor="test-title">Caso de prueba</label><input id="test-title" name="title" required /><label htmlFor="test-preconditions">Precondiciones</label><textarea id="test-preconditions" name="preconditions" /><label htmlFor="test-steps">Pasos</label><textarea id="test-steps" name="steps" /><label htmlFor="test-expected">Resultado esperado</label><textarea id="test-expected" name="expectedResult" /><button type="submit">Agregar caso</button></form></article>
        <article className="quality-card" id="quality-defects-form"><h2>Registrar defecto</h2><form className="workspace-form" onSubmit={createDefect}><label htmlFor="defect-requirement">Requisito relacionado</label><select id="defect-requirement" name="requirementId"><option value="">Sin requisito</option>{requirements.map((r) => <option key={r.id} value={r.id}>{r.title}</option>)}</select><label htmlFor="defect-test">Caso de prueba relacionado</label><select id="defect-test" name="testCaseId"><option value="">Sin caso de prueba</option>{dashboard?.testCases.map((t) => <option key={t.id} value={t.id}>{t.title}</option>)}</select><label htmlFor="defect-title">Defecto</label><input id="defect-title" name="title" required /><label htmlFor="defect-description">Descripción</label><textarea id="defect-description" name="description" /><div className="quality-form-row"><div><label htmlFor="defect-severity">Severidad</label><select id="defect-severity" name="severity"><option value="1">Baja</option><option value="2">Media</option><option value="3">Alta</option><option value="4">Crítica</option></select></div><div><label htmlFor="defect-priority">Prioridad</label><select id="defect-priority" name="priority"><option value="1">Baja</option><option value="2">Media</option><option value="3">Alta</option><option value="4">Urgente</option></select></div></div><button type="submit">Agregar defecto</button></form></article>
      </section>

      <section className="quality-card quality-traceability" id="quality-coverage" aria-labelledby="traceability-title"><div className="quality-card-header"><h2 id="traceability-title">Cobertura y matriz de trazabilidad</h2><span>{coverage}% global</span></div><div className="requirement-list">{dashboard?.traceability.map((row) => <article key={row.requirementId} className="requirement-card"><div className="requirement-meta"><span>{row.requirementStatus}</span><span>{row.covered ? 'Cubierto' : 'Sin cobertura'}</span></div><h3>{row.requirementTitle}</h3><p>{row.testCases} pruebas · {row.passedTests} aprobadas · {row.failedTests} fallidas · {row.openDefects} defectos abiertos</p></article>)}</div></section>
    </section>
  </main>
}
