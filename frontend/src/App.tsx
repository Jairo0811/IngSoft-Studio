import { BarChart3, CheckCircle2, FlaskConical, FolderKanban, Rocket, RotateCcw } from 'lucide-react'

const lifecycle = [
  { icon: FolderKanban, title: 'Requisitos', description: 'Define y gestiona requisitos funcionales y no funcionales.' },
  { icon: BarChart3, title: 'Análisis y diseño', description: 'Modela casos de uso, historias de usuario y arquitectura.' },
  { icon: CheckCircle2, title: 'Desarrollo', description: 'Planifica tareas y da seguimiento al progreso.' },
  { icon: FlaskConical, title: 'Pruebas', description: 'Crea casos de prueba y mide la calidad.' },
  { icon: Rocket, title: 'Despliegue', description: 'Gestiona versiones, liberaciones y entregas.' },
  { icon: RotateCcw, title: 'Mantenimiento', description: 'Controla incidencias y evolución continua.' },
]

function App() {
  return (
    <main className="app-shell">
      <section className="hero">
        <div className="brand-mark" aria-label="IngSoft Studio">
          <span>&lt;IS&gt;</span>
        </div>
        <div>
          <p className="eyebrow">Engineering Better Software</p>
          <h1>Ing<span>Soft</span> Studio</h1>
          <p className="lead">
            Plataforma integral para gestionar, analizar, planificar y simular el ciclo de vida del desarrollo de software.
          </p>
          <div className="actions">
            <button type="button">Crear proyecto</button>
            <button className="secondary" type="button">Explorar módulos</button>
          </div>
        </div>
      </section>

      <section className="lifecycle" aria-labelledby="lifecycle-title">
        <div className="section-heading">
          <p>Proceso integral</p>
          <h2 id="lifecycle-title">Ciclo de vida de Ingeniería de Software</h2>
        </div>
        <div className="lifecycle-grid">
          {lifecycle.map(({ icon: Icon, title, description }, index) => (
            <article key={title} className="lifecycle-card">
              <div className="card-number">{String(index + 1).padStart(2, '0')}</div>
              <Icon aria-hidden="true" />
              <h3>{title}</h3>
              <p>{description}</p>
            </article>
          ))}
        </div>
      </section>
    </main>
  )
}

export default App
