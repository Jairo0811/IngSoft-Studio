import { useEffect, useState } from 'react'
import { Link, Navigate } from 'react-router-dom'
import { authService } from '../services/auth'
import { LearningTopic, PortfolioDashboard, PortfolioTrend, ProjectInsight, SimulationResult, SimulationScenario, SimulationSummary, studioService } from '../services/studio'
import './studio.css'

export default function StudioPage() {
  const [dashboard, setDashboard] = useState<PortfolioDashboard | null>(null)
  const [projects, setProjects] = useState<ProjectInsight[]>([])
  const [trends, setTrends] = useState<PortfolioTrend[]>([])
  const [scenarios, setScenarios] = useState<SimulationScenario[]>([])
  const [topics, setTopics] = useState<LearningTopic[]>([])
  const [summary, setSummary] = useState<SimulationSummary | null>(null)
  const [result, setResult] = useState<SimulationResult | null>(null)
  const [error, setError] = useState('')

  useEffect(() => {
    void Promise.all([studioService.dashboard(), studioService.projectInsights(), studioService.trends(), studioService.scenarios(), studioService.learning(), studioService.simulationSummary()])
      .then(([dashboardData, projectData, trendData, scenarioData, topicData, summaryData]) => { setDashboard(dashboardData); setProjects(projectData); setTrends(trendData); setScenarios(scenarioData); setTopics(topicData); setSummary(summaryData) })
      .catch(() => setError('No fue posible cargar Studio Insights.'))
  }, [])
  if (!authService.hasToken()) return <Navigate to="/auth" replace />

  async function evaluate(scenarioId: string, optionId: string) { try { setResult(await studioService.evaluate(scenarioId, optionId)); setSummary(await studioService.simulationSummary()) } catch { setError('No fue posible evaluar la decisión.') } }
  async function download(format: 'pdf' | 'excel') { try { if (format === 'pdf') await studioService.downloadPdf(); else await studioService.downloadExcel() } catch { setError('No fue posible generar el reporte solicitado.') } }

  return <main className="studio-shell" aria-labelledby="studio-title">
    <header className="studio-header"><div><p className="eyebrow">Fase 5 · Simulación, reportes y aprendizaje</p><h1 id="studio-title">Studio Insights</h1><p>Convierte los datos del portafolio en métricas, decisiones, reportes y aprendizaje aplicado.</p></div><nav aria-label="Navegación de Studio Insights"><Link to="/projects">Proyectos</Link><Link to="/quality">Calidad</Link><Link to="/">Inicio</Link></nav></header>
    {error && <p className="studio-error" role="alert" aria-live="assertive">{error}</p>}

    <section className="studio-section" aria-labelledby="portfolio-title"><div className="section-heading"><p>Portafolio</p><h2 id="portfolio-title">Dashboard conectado a datos reales</h2></div>{dashboard && <div className="metric-grid" aria-label="Métricas globales"><article><strong>{dashboard.totalProjects}</strong><span>Proyectos</span></article><article><strong>{dashboard.activeProjects}</strong><span>Activos</span></article><article><strong>{dashboard.totalRequirements}</strong><span>Requisitos</span></article><article><strong>{dashboard.requirementCoveragePercent}%</strong><span>Cobertura</span></article><article><strong>{dashboard.testPassRatePercent}%</strong><span>Pruebas aprobadas</span></article><article><strong>{dashboard.openDefects}</strong><span>Defectos abiertos</span></article><article><strong>{dashboard.openRisks}</strong><span>Riesgos abiertos</span></article></div>}<div className="scenario-options" aria-label="Exportar reportes"><button type="button" onClick={() => void download('pdf')}>Descargar reporte PDF</button><button type="button" onClick={() => void download('excel')}>Exportar Excel</button></div></section>

    <section className="studio-section" aria-labelledby="project-insights-title"><div className="section-heading"><p>Por proyecto</p><h2 id="project-insights-title">Indicadores y tendencias comparativas</h2></div><div className="learning-grid">{projects.map((project) => <article key={project.projectId} className="learning-card"><span>{project.status}</span><h3>{project.projectName}</h3><p>{project.requirements} requisitos · {project.tests} pruebas · {project.passedTests} aprobadas</p><p>Cobertura {project.coveragePercent}% · Pass rate {project.passRatePercent}%</p><p>{project.openDefects} defectos abiertos · {project.openRisks} riesgos abiertos</p></article>)}</div>{trends.length > 0 && <p>{trends.map((item) => `${item.label}: ${item.requirements} req / ${item.tests} tests`).join(' · ')}</p>}</section>

    <section className="studio-section" aria-labelledby="simulator-title"><div className="section-heading"><p>Simulador</p><h2 id="simulator-title">Decisiones de Ingeniería de Software</h2></div>{summary && <div className="metric-grid" aria-label="Resumen del simulador"><article><strong>{summary.attempts}</strong><span>Intentos</span></article><article><strong>{summary.averageScore}</strong><span>Promedio</span></article><article><strong>{summary.bestScore}</strong><span>Mejor puntuación</span></article></div>}<div className="scenario-grid">{scenarios.map((scenario) => <article key={scenario.id} className="scenario-card"><h3>{scenario.title}</h3><p>{scenario.context}</p><strong>{scenario.question}</strong><div className="scenario-options">{scenario.options.map((option) => <button key={option.id} type="button" onClick={() => void evaluate(scenario.id, option.id)}>{option.label}</button>)}</div></article>)}</div>{result && <aside className="simulation-result" role="status" aria-live="polite"><strong>{result.level} · {result.score}/100</strong><p>{result.feedback}</p></aside>}{summary && summary.recentAttempts.length > 0 && <div className="learning-grid">{summary.recentAttempts.slice(0, 6).map((attempt) => <article key={attempt.id} className="learning-card"><span>{attempt.level}</span><h3>{attempt.scenarioId}</h3><p>{attempt.score}/100 · {new Date(attempt.createdAtUtc).toLocaleString()}</p></article>)}</div>}</section>

    <section className="studio-section" aria-labelledby="learning-title"><div className="section-heading"><p>Centro de aprendizaje</p><h2 id="learning-title">Conceptos para aplicar en proyectos reales</h2></div><div className="learning-grid">{topics.map((topic) => <article key={topic.id} className="learning-card"><span>{topic.category}</span><h3>{topic.title}</h3><p>{topic.summary}</p><ul>{topic.keyPoints.map((point) => <li key={point}>{point}</li>)}</ul></article>)}</div></section>
  </main>
}
