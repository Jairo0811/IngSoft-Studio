import { useEffect, useState } from 'react'
import { Link, Navigate } from 'react-router-dom'
import { authService } from '../services/auth'
import { LearningTopic, PortfolioDashboard, SimulationResult, SimulationScenario, studioService } from '../services/studio'
import './studio.css'

export default function StudioPage() {
  const [dashboard, setDashboard] = useState<PortfolioDashboard | null>(null)
  const [scenarios, setScenarios] = useState<SimulationScenario[]>([])
  const [topics, setTopics] = useState<LearningTopic[]>([])
  const [result, setResult] = useState<SimulationResult | null>(null)
  const [error, setError] = useState('')

  useEffect(() => {
    void Promise.all([studioService.dashboard(), studioService.scenarios(), studioService.learning()])
      .then(([dashboardData, scenarioData, topicData]) => {
        setDashboard(dashboardData)
        setScenarios(scenarioData)
        setTopics(topicData)
      })
      .catch(() => setError('No fue posible cargar Studio Insights.'))
  }, [])

  if (!authService.hasToken()) return <Navigate to="/auth" replace />

  async function evaluate(scenarioId: string, optionId: string) {
    try {
      setResult(await studioService.evaluate(scenarioId, optionId))
    } catch {
      setError('No fue posible evaluar la decisión.')
    }
  }

  return (
    <main className="studio-shell">
      <header className="studio-header">
        <div>
          <p className="eyebrow">Fase 5 · Simulación y aprendizaje</p>
          <h1>Studio Insights</h1>
          <p>Convierte los datos del proyecto en métricas, decisiones y aprendizaje aplicado.</p>
        </div>
        <nav><Link to="/projects">Proyectos</Link><Link to="/quality">Calidad</Link><Link to="/">Inicio</Link></nav>
      </header>

      {error && <p className="studio-error" role="alert">{error}</p>}

      <section className="studio-section">
        <div className="section-heading"><p>Portafolio</p><h2>Dashboard conectado a datos reales</h2></div>
        {dashboard && (
          <div className="metric-grid">
            <article><strong>{dashboard.totalProjects}</strong><span>Proyectos</span></article>
            <article><strong>{dashboard.activeProjects}</strong><span>Activos</span></article>
            <article><strong>{dashboard.totalRequirements}</strong><span>Requisitos</span></article>
            <article><strong>{dashboard.requirementCoveragePercent}%</strong><span>Cobertura</span></article>
            <article><strong>{dashboard.testPassRatePercent}%</strong><span>Pruebas aprobadas</span></article>
            <article><strong>{dashboard.openDefects}</strong><span>Defectos abiertos</span></article>
            <article><strong>{dashboard.openRisks}</strong><span>Riesgos abiertos</span></article>
          </div>
        )}
      </section>

      <section className="studio-section">
        <div className="section-heading"><p>Simulador</p><h2>Decisiones de Ingeniería de Software</h2></div>
        <div className="scenario-grid">
          {scenarios.map((scenario) => (
            <article key={scenario.id} className="scenario-card">
              <h3>{scenario.title}</h3>
              <p>{scenario.context}</p>
              <strong>{scenario.question}</strong>
              <div className="scenario-options">
                {scenario.options.map((option) => <button key={option.id} type="button" onClick={() => void evaluate(scenario.id, option.id)}>{option.label}</button>)}
              </div>
            </article>
          ))}
        </div>
        {result && <aside className="simulation-result"><strong>{result.level} · {result.score}/100</strong><p>{result.feedback}</p></aside>}
      </section>

      <section className="studio-section">
        <div className="section-heading"><p>Centro de aprendizaje</p><h2>Conceptos para aplicar en proyectos reales</h2></div>
        <div className="learning-grid">
          {topics.map((topic) => (
            <article key={topic.id} className="learning-card">
              <span>{topic.category}</span><h3>{topic.title}</h3><p>{topic.summary}</p>
              <ul>{topic.keyPoints.map((point) => <li key={point}>{point}</li>)}</ul>
            </article>
          ))}
        </div>
      </section>
    </main>
  )
}
