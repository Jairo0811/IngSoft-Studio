import { FormEvent, useEffect, useMemo, useState } from 'react'
import { Link, Navigate } from 'react-router-dom'
import { authService } from '../services/auth'
import { Project, projectsService, Requirement, RequirementInput } from '../services/projects'
import './projects.css'

const emptyRequirement: RequirementInput = {
  title: '',
  description: '',
  type: 'Functional',
  priority: 'Must',
  acceptanceCriteria: '',
  source: '',
}

export default function ProjectsPage() {
  const [projects, setProjects] = useState<Project[]>([])
  const [selectedProjectId, setSelectedProjectId] = useState('')
  const [requirements, setRequirements] = useState<Requirement[]>([])
  const [name, setName] = useState('')
  const [description, setDescription] = useState('')
  const [requirement, setRequirement] = useState<RequirementInput>(emptyRequirement)
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(true)

  const selectedProject = useMemo(
    () => projects.find((project) => project.id === selectedProjectId),
    [projects, selectedProjectId],
  )

  useEffect(() => {
    void loadProjects()
  }, [])

  useEffect(() => {
    if (selectedProjectId) void loadRequirements(selectedProjectId)
    else setRequirements([])
  }, [selectedProjectId])

  if (!authService.hasToken()) return <Navigate to="/auth" replace />

  async function loadProjects() {
    try {
      setLoading(true)
      const data = await projectsService.list()
      setProjects(data)
      setSelectedProjectId((current) => current || data[0]?.id || '')
    } catch {
      setError('No fue posible cargar los proyectos.')
    } finally {
      setLoading(false)
    }
  }

  async function loadRequirements(projectId: string) {
    try {
      setRequirements(await projectsService.requirements(projectId))
    } catch {
      setError('No fue posible cargar los requisitos del proyecto.')
    }
  }

  async function createProject(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    try {
      setError('')
      const created = await projectsService.create(name, description)
      setProjects((current) => [created, ...current])
      setSelectedProjectId(created.id)
      setName('')
      setDescription('')
    } catch {
      setError('No fue posible crear el proyecto.')
    }
  }

  async function createRequirement(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (!selectedProjectId) return
    try {
      setError('')
      const created = await projectsService.createRequirement(selectedProjectId, requirement)
      setRequirements((current) => [...current, created])
      setRequirement(emptyRequirement)
    } catch {
      setError('No fue posible crear el requisito.')
    }
  }

  async function changeRequirementStatus(item: Requirement, status: number) {
    const updated = await projectsService.changeRequirementStatus(item.projectId, item.id, status)
    setRequirements((current) => current.map((value) => (value.id === item.id ? updated : value)))
  }

  async function deleteRequirement(item: Requirement) {
    await projectsService.removeRequirement(item.projectId, item.id)
    setRequirements((current) => current.filter((value) => value.id !== item.id))
  }

  return (
    <main className="workspace-shell">
      <header className="workspace-header">
        <div>
          <p className="eyebrow">Fase 3 · Ingeniería de Requisitos</p>
          <h1>Proyectos y requisitos</h1>
          <p>Gestiona el alcance funcional, historias de usuario, casos de uso y prioridades MoSCoW.</p>
        </div>
        <nav>
          <Link to="/account">Mi cuenta</Link>
          <Link to="/">Inicio</Link>
        </nav>
      </header>

      {error && <p className="workspace-error" role="alert">{error}</p>}

      <section className="workspace-grid">
        <aside className="workspace-panel projects-panel">
          <h2>Proyectos</h2>
          <form onSubmit={createProject} className="workspace-form">
            <input value={name} onChange={(event) => setName(event.target.value)} placeholder="Nombre del proyecto" required maxLength={150} />
            <textarea value={description} onChange={(event) => setDescription(event.target.value)} placeholder="Descripción" maxLength={1000} />
            <button type="submit">Crear proyecto</button>
          </form>

          <div className="project-list">
            {loading && <p>Cargando…</p>}
            {!loading && projects.length === 0 && <p>No hay proyectos todavía.</p>}
            {projects.map((project) => (
              <button
                type="button"
                key={project.id}
                className={project.id === selectedProjectId ? 'project-item selected' : 'project-item'}
                onClick={() => setSelectedProjectId(project.id)}
              >
                <strong>{project.name}</strong>
                <span>{project.status}</span>
              </button>
            ))}
          </div>
        </aside>

        <section className="workspace-panel requirements-panel">
          {!selectedProject ? (
            <div className="empty-state"><h2>Selecciona un proyecto</h2><p>Los requisitos aparecerán aquí.</p></div>
          ) : (
            <>
              <div className="requirements-heading">
                <div>
                  <p className="eyebrow">{selectedProject.status}</p>
                  <h2>{selectedProject.name}</h2>
                  <p>{selectedProject.description || 'Sin descripción.'}</p>
                </div>
                <strong>{requirements.length} requisitos</strong>
              </div>

              <form onSubmit={createRequirement} className="requirement-form">
                <input value={requirement.title} onChange={(event) => setRequirement({ ...requirement, title: event.target.value })} placeholder="Título del requisito" required />
                <textarea value={requirement.description} onChange={(event) => setRequirement({ ...requirement, description: event.target.value })} placeholder="Descripción" required />
                <div className="form-row">
                  <select value={requirement.type} onChange={(event) => setRequirement({ ...requirement, type: event.target.value as RequirementInput['type'] })}>
                    <option value="Functional">Funcional</option>
                    <option value="NonFunctional">No funcional</option>
                    <option value="UserStory">Historia de usuario</option>
                    <option value="UseCase">Caso de uso</option>
                  </select>
                  <select value={requirement.priority} onChange={(event) => setRequirement({ ...requirement, priority: event.target.value as RequirementInput['priority'] })}>
                    <option value="Must">Must</option>
                    <option value="Should">Should</option>
                    <option value="Could">Could</option>
                    <option value="Wont">Won't</option>
                  </select>
                </div>
                <textarea value={requirement.acceptanceCriteria} onChange={(event) => setRequirement({ ...requirement, acceptanceCriteria: event.target.value })} placeholder="Criterios de aceptación" />
                <input value={requirement.source} onChange={(event) => setRequirement({ ...requirement, source: event.target.value })} placeholder="Fuente / stakeholder / documento" />
                <button type="submit">Agregar requisito</button>
              </form>

              <div className="requirement-list">
                {requirements.map((item) => (
                  <article key={item.id} className="requirement-card">
                    <div className="requirement-meta">
                      <span>{item.type}</span><span>{item.priority}</span><span>{item.status}</span>
                    </div>
                    <h3>{item.title}</h3>
                    <p>{item.description}</p>
                    {item.acceptanceCriteria && <div><strong>Criterios:</strong><p>{item.acceptanceCriteria}</p></div>}
                    {item.source && <small>Fuente: {item.source}</small>}
                    <div className="requirement-actions">
                      <button type="button" onClick={() => void changeRequirementStatus(item, 2)}>Aprobar</button>
                      <button type="button" onClick={() => void changeRequirementStatus(item, 3)}>En progreso</button>
                      <button type="button" onClick={() => void changeRequirementStatus(item, 4)}>Implementado</button>
                      <button type="button" className="danger" onClick={() => void deleteRequirement(item)}>Eliminar</button>
                    </div>
                  </article>
                ))}
              </div>
            </>
          )}
        </section>
      </section>
    </main>
  )
}
