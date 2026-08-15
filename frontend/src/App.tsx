import { BarChart3, CheckCircle2, FlaskConical, FolderKanban, Rocket, RotateCcw } from 'lucide-react'
import { Link, Route, Routes } from 'react-router-dom'
import AccessibilityTools from './components/AccessibilityTools'
import BrandLogo from './components/BrandLogo'
import AccessibilityPage from './pages/AccessibilityPage'
import AccountPage from './pages/AccountPage'
import AuthPage from './pages/AuthPage'
import PasswordRecoveryPage from './pages/PasswordRecoveryPage'
import ProjectsPage from './pages/ProjectsPage'
import QualityPage from './pages/QualityPage'
import StudioPage from './pages/StudioPage'
import { authService } from './services/auth'

const lifecycle = [
  { icon: FolderKanban, title: 'Requisitos', description: 'Define, prioriza y traza requisitos funcionales y no funcionales.' },
  { icon: BarChart3, title: 'Análisis y diseño', description: 'Modela casos de uso, decisiones técnicas y arquitectura.' },
  { icon: CheckCircle2, title: 'Desarrollo', description: 'Organiza el trabajo y da seguimiento al avance del producto.' },
  { icon: FlaskConical, title: 'Pruebas', description: 'Gestiona calidad, cobertura, riesgos, defectos y validaciones.' },
  { icon: Rocket, title: 'Despliegue', description: 'Prepara entregas, versiones y liberaciones de forma controlada.' },
  { icon: RotateCcw, title: 'Mantenimiento', description: 'Administra incidencias, mejoras y evolución continua.' },
]

function LandingPage() {
  const isAuthenticated = authService.hasToken()

  return (
    <main className="home-shell" aria-labelledby="home-title">
      <section className="home-hero">
        <div className="home-hero__glow" aria-hidden="true" />
        <div className="home-brand-card">
          <BrandLogo />
          <div className="home-brand-card__meta">
            <span>Software Engineering Workspace</span>
            <strong>Requirements · Quality · Insights</strong>
          </div>
        </div>

        <div className="home-copy">
          <p className="eyebrow">Engineering Better Software</p>
          <h1 id="home-title">Ingeniería de Software <span>de extremo a extremo</span></h1>
          <p className="lead">Gestiona requisitos, calidad, trazabilidad, métricas y aprendizaje aplicado desde una plataforma diseñada alrededor del ciclo de vida del software.</p>
          <nav className="actions home-actions" aria-label="Accesos principales">
            <Link className="primary-link" to="/projects">Abrir Studio</Link>
            <Link className="secondary-link" to="/quality">Quality Center</Link>
            <Link className="secondary-link" to="/studio">Studio Insights</Link>
            <Link className="secondary-link" to={isAuthenticated ? '/account' : '/auth'}>{isAuthenticated ? 'Mi cuenta' : 'Acceder'}</Link>
          </nav>
          <div className="home-trust" aria-label="Capacidades principales">
            <span>Clean Architecture</span><span>Calidad y trazabilidad</span><span>Accesibilidad NORTIC B2</span>
          </div>
        </div>
      </section>

      <section className="lifecycle home-lifecycle" aria-labelledby="lifecycle-title">
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
