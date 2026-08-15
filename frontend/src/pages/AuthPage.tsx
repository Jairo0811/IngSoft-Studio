import { FormEvent, useState } from 'react'
import { Link, Navigate, useLocation, useNavigate } from 'react-router-dom'
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

  if (authService.hasToken()) {
    return <Navigate to={destination} replace />
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setSubmitting(true)
    setError('')

    try {
      if (mode === 'register') {
        await authService.register(fullName, email, password)
      } else {
        await authService.login(email, password)
      }
      navigate(destination, { replace: true })
    } catch {
      setError('No fue posible completar la autenticación. Verifica los datos e inténtalo nuevamente.')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <main className="auth-shell">
      <section className="auth-card">
        <div className="auth-brand">&lt;IS&gt;</div>
        <p className="eyebrow">IngSoft Studio</p>
        <h1>{mode === 'login' ? 'Iniciar sesión' : 'Crear cuenta'}</h1>
        <p className="auth-copy">
          {mode === 'login'
            ? 'Accede a tu espacio de trabajo de Ingeniería de Software.'
            : 'Crea tu cuenta para gestionar proyectos, requisitos, calidad y pruebas.'}
        </p>

        <form onSubmit={handleSubmit} className="auth-form">
          {mode === 'register' && (
            <label>
              Nombre completo
              <input value={fullName} onChange={(event) => setFullName(event.target.value)} required maxLength={150} />
            </label>
          )}

          <label>
            Correo electrónico
            <input type="email" value={email} onChange={(event) => setEmail(event.target.value)} required autoComplete="email" />
          </label>

          <label>
            Contraseña
            <input
              type="password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              required
              minLength={8}
              autoComplete={mode === 'login' ? 'current-password' : 'new-password'}
            />
          </label>

          {error && <p className="form-error" role="alert">{error}</p>}

          <button type="submit" disabled={submitting}>
            {submitting ? 'Procesando…' : mode === 'login' ? 'Entrar' : 'Registrarme'}
          </button>
        </form>

        {mode === 'login' && <Link to="/forgot-password">¿Olvidaste tu contraseña?</Link>}

        <button className="text-button" type="button" onClick={() => setMode(mode === 'login' ? 'register' : 'login')}>
          {mode === 'login' ? '¿No tienes cuenta? Crear una' : '¿Ya tienes cuenta? Iniciar sesión'}
        </button>
      </section>
    </main>
  )
}
