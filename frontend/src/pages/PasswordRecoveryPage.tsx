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

  async function requestReset(event: FormEvent<HTMLFormElement>) { event.preventDefault(); setError(''); setMessage(''); try { await authService.forgotPassword(email); setStep('reset'); setMessage('Si la cuenta existe, recibirás el token mediante el canal de recuperación configurado. La aplicación nunca lo muestra ni lo registra.') } catch { setError('No fue posible procesar la solicitud.') } }
  async function resetPassword(event: FormEvent<HTMLFormElement>) { event.preventDefault(); setError(''); setMessage(''); try { await authService.resetPassword(email, token, newPassword); setMessage('Contraseña restablecida correctamente. Ya puedes iniciar sesión.') } catch { setError('El token no es válido, expiró o la nueva contraseña no cumple la política de seguridad.') } }

  return <main className="auth-shell auth-shell--brand">
    <section className="auth-brand-panel"><BrandLogo /><p className="eyebrow">Seguridad de acceso</p><h2>Recupera tu cuenta sin romper el flujo de trabajo.</h2><p>Restablece tu contraseña y vuelve a tus proyectos, métricas y simulaciones.</p></section>
    <section className="auth-card auth-card--brand"><p className="eyebrow">Seguridad</p><h1>Recuperar acceso</h1>{message && <p className="form-success" role="status">{message}</p>}{error && <p className="form-error" role="alert">{error}</p>}{step === 'request' ? <form className="auth-form" onSubmit={requestReset}><label>Correo electrónico<input type="email" value={email} onChange={(event) => setEmail(event.target.value)} required maxLength={256} autoComplete="email" /></label><button type="submit">Solicitar restablecimiento</button></form> : <form className="auth-form" onSubmit={resetPassword}><label>Correo electrónico<input type="email" value={email} onChange={(event) => setEmail(event.target.value)} required maxLength={256} autoComplete="email" /></label><label>Token de recuperación<input value={token} onChange={(event) => setToken(event.target.value)} required minLength={8} maxLength={4096} autoComplete="off" /></label><label>Nueva contraseña<input type="password" value={newPassword} onChange={(event) => setNewPassword(event.target.value)} required minLength={12} maxLength={128} autoComplete="new-password" /></label><button type="submit">Restablecer contraseña</button></form>}<Link className="secondary-link" to="/auth">Volver al inicio de sesión</Link></section>
  </main>
}
