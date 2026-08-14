import { BarChart3, CheckCircle2, FlaskConical, FolderKanban, Rocket, RotateCcw } from 'lucide-react'
import { Link, Route, Routes } from 'react-router-dom'
import AccessibilityTools from './components/AccessibilityTools'
import AccessibilityPage from './pages/AccessibilityPage'
import AccountPage from './pages/AccountPage'
import AuthPage from './pages/AuthPage'
import PasswordRecoveryPage from './pages/PasswordRecoveryPage'
import ProjectsPage from './pages/ProjectsPage'
import QualityPage from './pages/QualityPage'
import StudioPage from './pages/StudioPage'

const lifecycle = [
  { icon: FolderKanban, title: 'Requisitos', description: 'Define y gestiona requisitos funcionales y no funcionales.' },
  { icon: BarChart3, title: 'Análisis y diseño', description: 'Modela casos de uso, historias de usuario y arquitectura.' },
  { icon: CheckCircle2, title: 'Desarrollo', description: 'Planifica tareas y da seguimiento al progreso.' },
  { icon: FlaskConical, title: 'Pruebas', description: 'Crea casos de prueba y mide la calidad.' },
  { icon: Rocket, title: 'Despliegue', description: 'Gestiona versiones, liberaciones y entregas.' },
  { icon: RotateCcw, title: 'Mantenimiento', description: 'Controla incidencias y evolución continua.' },
]

function LandingPage() {
  return (
    <main className="app-shell" aria-labelledby="home-title">
      <section className="hero">
        <div className="brand-mark" role="img" aria-label="Símbolo de IngSoft Studio"><span aria-hidden="true">&lt;IS&gt;</span></div>
        <div>
          <p className="eyebrow">Engineering Better Software</p>
          <h1 id="home-title">Ing<span>Soft</span> Studio</h1>
          <p className="lead">Plataforma integral para gestionar, analizar, planificar y simular el ciclo de vida del desarrollo de software.</p>
          <nav className="actions" aria-label="Accesos principales">
            <Link className="primary-link" to="/projects">Abrir Studio</Link>
            <Link className="secondary-link" to="/quality">Quality Center</Link>
            <Link className="secondary-link" to="/studio">Studio Insights</Link>
            <Link className="secondary-link" to="/auth">Acceder</Link>
          </nav>
        </div>
      </section>
      <section className="lifecycle" aria-labelledby="lifecycle-title">
        <div className="section-heading"><p>Proceso integral</p><h2 id="lifecycle-title">Ciclo de vida de Ingeniería de Software</h2></div>
        <div className="lifecycle-grid">
          {lifecycle.map(({ icon: Icon, title, description }, index) => (
            <article key={title} className="lifecycle-card"><div className="card-number" aria-label={`Etapa ${index + 1}`}>{String(index + 1).padStart(2, '0')}</div><Icon aria-hidden="true" /><h3>{title}</h3><p>{description}</p></article>
          ))}
        </div>
      </section>
    </main>
  )
}

function App() {
  return <>
    <AccessibilityTools />
    <div id="main-content" tabIndex={-1}>
      <Routes>
        <Route path="/" element={<LandingPage />} />
        <Route path="/auth" element={<AuthPage />} />
        <Route path="/forgot-password" element={<PasswordRecoveryPage />} />
        <Route path="/account" element={<AccountPage />} />
        <Route path="/projects" element={<ProjectsPage />} />
        <Route path="/quality" element={<QualityPage />} />
        <Route path="/studio" element={<StudioPage />} />
        <Route path="/accessibility" element={<AccessibilityPage />} />
      </Routes>
    </div>
  </>
}

export default App
