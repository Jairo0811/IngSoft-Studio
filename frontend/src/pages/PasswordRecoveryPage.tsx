import { FormEvent, useState } from 'react'
import { Link } from 'react-router-dom'
import BrandLogo from '../components/BrandLogo'
import { authService } from '../services/auth'

export default function PasswordRecoveryPage() {
  const [email, setEmail] = useState('')
  const [token, setToken] = useState('')
  const [newPassword, setNewPassword] = useState('')
  const [step, setStep] = useState<'request' | 'reset'>('request')
  const [message, setMessage] = useState('')
  const [error, setError] = useState('')

  async function requestReset(event: FormEvent<HTMLFormElement>) { event.preventDefault(); setError(''); setMessage(''); try { const response = await authService.forgotPassword(email); if (response?.resetToken) setToken(response.resetToken); setStep('reset'); setMessage('Solicitud procesada. En desarrollo, el token puede mostrarse automáticamente; en producción se enviará por un canal seguro.') } catch { setError('No fue posible procesar la solicitud.') } }
  async function resetPassword(event: FormEvent<HTMLFormElement>) { event.preventDefault(); setError(''); setMessage(''); try { await authService.resetPassword(email, token, newPassword); setMessage('Contraseña restablecida correctamente. Ya puedes iniciar sesión.') } catch { setError('El token no es válido, expiró o la nueva contraseña no cumple la política de seguridad.') } }

  return <main className="auth-shell auth-shell--brand">
    <section className="auth-brand-panel"><BrandLogo /><p className="eyebrow">Seguridad de acceso</p><h2>Recupera tu cuenta sin romper el flujo de trabajo.</h2><p>Restablece tu contraseña y vuelve a tus proyectos, métricas y simulaciones.</p></section>
    <section className="auth-card auth-card--brand"><p className="eyebrow">Seguridad</p><h1>Recuperar acceso</h1>{message && <p className="form-success" role="status">{message}</p>}{error && <p className="form-error" role="alert">{error}</p>}{step === 'request' ? <form className="auth-form" onSubmit={requestReset}><label>Correo electrónico<input type="email" value={email} onChange={(event) => setEmail(event.target.value)} required /></label><button type="submit">Solicitar restablecimiento</button></form> : <form className="auth-form" onSubmit={resetPassword}><label>Correo electrónico<input type="email" value={email} onChange={(event) => setEmail(event.target.value)} required /></label><label>Token de recuperación<input value={token} onChange={(event) => setToken(event.target.value)} required /></label><label>Nueva contraseña<input type="password" value={newPassword} onChange={(event) => setNewPassword(event.target.value)} required minLength={8} /></label><button type="submit">Restablecer contraseña</button></form>}<Link className="secondary-link" to="/auth">Volver al inicio de sesión</Link></section>
  </main>
}
