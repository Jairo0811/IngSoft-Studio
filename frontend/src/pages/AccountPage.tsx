import { FormEvent, useEffect, useState } from 'react'
import { Navigate, useLocation, useNavigate } from 'react-router-dom'
import BrandLogo from '../components/BrandLogo'
import WorkspaceNav from '../components/WorkspaceNav'
import { authService, type User } from '../services/auth'

export default function AccountPage() {
  const navigate = useNavigate()
  const location = useLocation()
  const [user, setUser] = useState<User | null>(null)
  const [fullName, setFullName] = useState('')
  const [currentPassword, setCurrentPassword] = useState('')
  const [newPassword, setNewPassword] = useState('')
  const [message, setMessage] = useState('')
  const [error, setError] = useState('')

  useEffect(() => {
    authService.me().then((profile) => { setUser(profile); setFullName(profile.fullName) }).catch(() => { authService.logout(); navigate('/auth', { replace: true, state: { from: location.pathname } }) })
  }, [location.pathname, navigate])

  if (!authService.hasToken()) return <Navigate to="/auth" replace state={{ from: location.pathname }} />

  async function handleProfile(event: FormEvent<HTMLFormElement>) { event.preventDefault(); setError(''); setMessage(''); try { const updated = await authService.updateProfile(fullName); setUser(updated); setMessage('Perfil actualizado correctamente.') } catch { setError('No fue posible actualizar el perfil.') } }
  async function handlePassword(event: FormEvent<HTMLFormElement>) { event.preventDefault(); setError(''); setMessage(''); try { await authService.changePassword(currentPassword, newPassword); setCurrentPassword(''); setNewPassword(''); setMessage('Contraseña actualizada correctamente.') } catch { setError('No fue posible cambiar la contraseña. Verifica la contraseña actual y los requisitos de seguridad.') } }
  function logout() { authService.logout(); navigate('/', { replace: true }) }

  return <main className="account-shell branded-workspace">
    <aside className="workspace-sidebar" aria-label="Cuenta de IngSoft Studio">
      <BrandLogo />
      <div className="workspace-sidebar__section"><p className="workspace-sidebar__label">Cuenta</p><strong>{user?.fullName ?? 'Usuario'}</strong><span>{user?.email ?? 'Cargando perfil…'}</span></div>
    </aside>
    <section className="workspace-main">
      <WorkspaceNav />
      <section className="account-header account-header--brand"><div><p className="eyebrow">Mi cuenta</p><h1>{user?.fullName ?? 'Cargando perfil…'}</h1><p className="auth-copy">{user?.email}</p>{user && <p className="role-list">Roles: {user.roles.join(', ') || 'User'}</p>}</div><button type="button" className="secondary-button" onClick={logout}>Cerrar sesión</button></section>
      {(message || error) && <p className={error ? 'form-error' : 'form-success'} role="status">{error || message}</p>}
      <section className="account-grid">
        <form className="auth-card" onSubmit={handleProfile}><h2>Perfil</h2><label>Nombre completo<input value={fullName} onChange={(event) => setFullName(event.target.value)} required maxLength={150} /></label><button type="submit">Guardar cambios</button></form>
        <form className="auth-card" onSubmit={handlePassword}><h2>Seguridad</h2><label>Contraseña actual<input type="password" value={currentPassword} onChange={(event) => setCurrentPassword(event.target.value)} required /></label><label>Nueva contraseña<input type="password" value={newPassword} onChange={(event) => setNewPassword(event.target.value)} required minLength={8} /></label><button type="submit">Cambiar contraseña</button></form>
      </section>
    </section>
  </main>
}
