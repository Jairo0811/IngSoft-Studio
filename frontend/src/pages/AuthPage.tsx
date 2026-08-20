import { FormEvent, useState } from 'react'
import { Link, Navigate, useLocation, useNavigate } from 'react-router-dom'
import BrandLogo from '../components/BrandLogo'
import { authService } from '../services/auth'

type AuthLocationState = { from?: string }

export default function AuthPage() {
  const navigate = useNavigate()
  const location = useLocation()
  const [mode, setMode] = useState<'login' | 'register'>('login')
  const [fullName, setFullName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const destination = (location.state as AuthLocationState | null)?.from || '/projects'

  if (authService.hasToken()) return <Navigate to={destination} replace />

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setSubmitting(true)
    setError('')
    try {
      if (mode === 'register') await authService.register(fullName, email, password)
      else await authService.login(email, password)
      navigate(destination, { replace: true })
    } catch {
      setError('No fue posible completar la autenticación. Verifica los datos e inténtalo nuevamente.')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <main className="auth-shell auth-shell--branded">
      <section className="auth-showcase" aria-label="Presentación de IngSoft Studio">
        <Link className="auth-home-link" to="/">← Volver al inicio</Link>
        <BrandLogo />
        <div>
          <p className="eyebrow">Engineering Better Software</p>
          <h2>Tu espacio de trabajo para construir software con trazabilidad.</h2>
          <p>Centraliza proyectos, requisitos, riesgos, pruebas, defectos, métricas e insights en una sola experiencia.</p>
        </div>
        <div className="auth-showcase__features"><span>Requisitos</span><span>Quality Center</span><span>Studio Insights</span></div>
      </section>

      <section className="auth-card auth-card--login" aria-labelledby="auth-title">
        <div className="auth-card__logo"><BrandLogo compact /></div>
        <p className="eyebrow">IngSoft Studio</p>
        <h1 id="auth-title">{mode === 'login' ? 'Iniciar sesión' : 'Crear cuenta'}</h1>
        <p className="auth-copy">{mode === 'login' ? 'Accede a tu workspace de Ingeniería de Software.' : 'Crea tu cuenta para comenzar a gestionar el ciclo de vida de tus proyectos.'}</p>

        <form onSubmit={handleSubmit} className="auth-form">
          {mode === 'register' && <label>Nombre completo<input value={fullName} onChange={(event) => setFullName(event.target.value)} required maxLength={150} autoComplete="name" /></label>}
          <label>Correo electrónico<input type="email" value={email} onChange={(event) => setEmail(event.target.value)} required autoComplete="email" /></label>
          <label>Contraseña<input type="password" value={password} onChange={(event) => setPassword(event.target.value)} required minLength={mode === 'register' ? 12 : 1} maxLength={128} autoComplete={mode === 'login' ? 'current-password' : 'new-password'} /></label>
          {error && <p className="form-error" role="alert">{error}</p>}
          <button type="submit" disabled={submitting}>{submitting ? 'Procesando…' : mode === 'login' ? 'Entrar al Studio' : 'Crear cuenta'}</button>
        </form>

        <div className="auth-card__links">
          {mode === 'login' && <Link to="/forgot-password">¿Olvidaste tu contraseña?</Link>}
          <button className="text-button" type="button" onClick={() => setMode(mode === 'login' ? 'register' : 'login')}>{mode === 'login' ? '¿No tienes cuenta? Crear una' : '¿Ya tienes cuenta? Iniciar sesión'}</button>
        </div>
      </section>
    </main>
  )
}
