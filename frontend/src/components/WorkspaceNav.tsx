import { BarChart3, FolderKanban, Home, ShieldCheck, UserCircle } from 'lucide-react'
import { NavLink } from 'react-router-dom'
import './workspace-nav.css'

const items = [
  { to: '/projects', label: 'Proyectos', icon: FolderKanban },
  { to: '/quality', label: 'Quality Center', icon: ShieldCheck },
  { to: '/studio', label: 'Studio Insights', icon: BarChart3 },
  { to: '/account', label: 'Mi cuenta', icon: UserCircle },
  { to: '/', label: 'Inicio', icon: Home },
]

export default function WorkspaceNav() {
  return (
    <nav className="workspace-nav" aria-label="Navegación principal de IngSoft Studio">
      {items.map(({ to, label, icon: Icon }) => (
        <NavLink key={to} to={to} end={to === '/'} className={({ isActive }) => isActive ? 'workspace-nav__link active' : 'workspace-nav__link'}>
          <Icon aria-hidden="true" />
          <span>{label}</span>
        </NavLink>
      ))}
    </nav>
  )
}
