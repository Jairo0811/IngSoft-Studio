import { FormEvent, useEffect, useState } from 'react'
import { Link, Navigate } from 'react-router-dom'
import { authService } from '../services/auth'
import { Project, projectsService, Requirement } from '../services/projects'
import { QualityDashboard, qualityService } from '../services/quality'
import './projects.css'

export default function QualityPage() {
  const [projects, setProjects] = useState<Project[]>([])
  const [requirements, setRequirements] = useState<Requirement[]>([])
  const [projectId, setProjectId] = useState('')
  const [dashboard, setDashboard] = useState<QualityDashboard | null>(null)
  const [error, setError] = useState('')

  useEffect(() => { void loadProjects() }, [])
  useEffect(() => { if (projectId) void loadQuality(projectId) }, [projectId])

  if (!authService.hasToken()) return <Navigate to="/auth" replace />

  async function loadProjects() {
    try {
      const data = await projectsService.list()
      setProjects(data)
      setProjectId(data[0]?.id || '')
    } catch { setError('No fue posible cargar los proyectos.') }
  }

  async function loadQuality(id: string) {
    try {
      setError('')
      const [quality, reqs] = await Promise.all([qualityService.dashboard(id), projectsService.requirements(id)])
      setDashboard(quality)
      setRequirements(reqs)
    } catch { setError('No fue posible cargar el centro de calidad.') }
  }

  async function createRisk(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!projectId) return
    const form = new FormData(event.currentTarget)
    await qualityService.createRisk(projectId, {
      title: form.get('title'), description: form.get('description'), probability: Number(form.get('probability')),
      impact: Number(form.get('impact')), mitigation: form.get('mitigation'),
    })
    event.currentTarget.reset(); await loadQuality(projectId)
  }

  async function createTest(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!projectId) return
    const form = new FormData(event.currentTarget)
    await qualityService.createTest(projectId, {
      requirementId: form.get('requirementId') || null, title: form.get('title'), preconditions: form.get('preconditions'),
      steps: form.get('steps'), expectedResult: form.get('expectedResult'),
    })
    event.currentTarget.reset(); await loadQuality(projectId)
  }

  async function createDefect(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!projectId) return
    const form = new FormData(event.currentTarget)
    await qualityService.createDefect(projectId, {
      requirementId: form.get('requirementId') || null, testCaseId: form.get('testCaseId') || null,
      title: form.get('title'), description: form.get('description'), severity: Number(form.get('severity')), priority: Number(form.get('priority')),
    })
    event.currentTarget.reset(); await loadQuality(projectId)
  }

  return <main className="workspace-shell">
    <header className="workspace-header">
      <div><p className="eyebrow">Fase 4 · Calidad y trazabilidad</p><h1>Quality Center</h1><p>Riesgos, casos de prueba, defectos, métricas y cobertura de requisitos.</p></div>
      <nav><Link to="/projects">Proyectos</Link><Link to="/account">Mi cuenta</Link><Link to="/">Inicio</Link></nav>
    </header>

    {error && <p className="workspace-error" role="alert">{error}</p>}
    <section className="workspace-panel">
      <label>Proyecto activo</label>
      <select value={projectId} onChange={(event) => setProjectId(event.target.value)}>
        {projects.map((project) => <option key={project.id} value={project.id}>{project.name}</option>)}
      </select>
    </section>

    {dashboard && <>
      <section className="lifecycle-grid">
        <article className="lifecycle-card"><h3>{dashboard.metrics.requirementCoveragePercent}%</h3><p>Cobertura de requisitos</p></article>
        <article className="lifecycle-card"><h3>{dashboard.metrics.testPassRatePercent}%</h3><p>Tasa de pruebas aprobadas</p></article>
        <article className="lifecycle-card"><h3>{dashboard.metrics.openDefects}</h3><p>Defectos abiertos</p></article>
        <article className="lifecycle-card"><h3>{dashboard.metrics.highRisks}</h3><p>Riesgos altos</p></article>
      </section>

      <section className="workspace-grid">
        <article className="workspace-panel">
          <h2>Registrar riesgo</h2>
          <form className="workspace-form" onSubmit={createRisk}>
            <input name="title" placeholder="Riesgo" required />
            <textarea name="description" placeholder="Descripción" />
            <select name="probability"><option value="1">Probabilidad baja</option><option value="2">Media</option><option value="3">Alta</option></select>
            <select name="impact"><option value="1">Impacto bajo</option><option value="2">Medio</option><option value="3">Alto</option></select>
            <textarea name="mitigation" placeholder="Plan de mitigación" />
            <button type="submit">Agregar riesgo</button>
          </form>
        </article>

        <article className="workspace-panel">
          <h2>Crear caso de prueba</h2>
          <form className="workspace-form" onSubmit={createTest}>
            <select name="requirementId"><option value="">Sin requisito</option>{requirements.map((r) => <option key={r.id} value={r.id}>{r.title}</option>)}</select>
            <input name="title" placeholder="Caso de prueba" required />
            <textarea name="preconditions" placeholder="Precondiciones" />
            <textarea name="steps" placeholder="Pasos" />
            <textarea name="expectedResult" placeholder="Resultado esperado" />
            <button type="submit">Agregar caso</button>
          </form>
        </article>

        <article className="workspace-panel">
          <h2>Registrar defecto</h2>
          <form className="workspace-form" onSubmit={createDefect}>
            <select name="requirementId"><option value="">Sin requisito</option>{requirements.map((r) => <option key={r.id} value={r.id}>{r.title}</option>)}</select>
            <select name="testCaseId"><option value="">Sin caso de prueba</option>{dashboard.testCases.map((t) => <option key={t.id} value={t.id}>{t.title}</option>)}</select>
            <input name="title" placeholder="Defecto" required />
            <textarea name="description" placeholder="Descripción" />
            <select name="severity"><option value="1">Severidad baja</option><option value="2">Media</option><option value="3">Alta</option><option value="4">Crítica</option></select>
            <select name="priority"><option value="1">Prioridad baja</option><option value="2">Media</option><option value="3">Alta</option><option value="4">Urgente</option></select>
            <button type="submit">Agregar defecto</button>
          </form>
        </article>
      </section>

      <section className="workspace-panel">
        <h2>Matriz de trazabilidad</h2>
        <div className="requirement-list">
          {dashboard.traceability.map((row) => <article key={row.requirementId} className="requirement-card">
            <div className="requirement-meta"><span>{row.requirementStatus}</span><span>{row.covered ? 'Cubierto' : 'Sin cobertura'}</span></div>
            <h3>{row.requirementTitle}</h3>
            <p>{row.testCases} pruebas · {row.passedTests} aprobadas · {row.failedTests} fallidas · {row.openDefects} defectos abiertos</p>
          </article>)}
        </div>
      </section>
    </>}
  </main>
}
