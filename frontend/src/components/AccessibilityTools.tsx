import { useEffect, useState } from 'react'
import { Link, useLocation } from 'react-router-dom'

const TEXT_SCALE_KEY = 'ingsoftstudio.textScale'
const CONTRAST_KEY = 'ingsoftstudio.highContrast'

const pageTitles: Record<string, string> = {
  '/': 'Inicio',
  '/auth': 'Acceso',
  '/forgot-password': 'Recuperar contraseña',
  '/account': 'Mi cuenta',
  '/projects': 'Proyectos y requisitos',
  '/quality': 'Quality Center',
  '/studio': 'Studio Insights',
  '/accessibility': 'Accesibilidad',
}

export default function AccessibilityTools() {
  const location = useLocation()
  const [scale, setScale] = useState(() => Number(localStorage.getItem(TEXT_SCALE_KEY) ?? 100))
  const [highContrast, setHighContrast] = useState(() => localStorage.getItem(CONTRAST_KEY) === 'true')

  useEffect(() => {
    document.documentElement.style.fontSize = `${scale}%`
    localStorage.setItem(TEXT_SCALE_KEY, String(scale))
  }, [scale])

  useEffect(() => {
    document.documentElement.dataset.contrast = highContrast ? 'high' : 'normal'
    localStorage.setItem(CONTRAST_KEY, String(highContrast))
  }, [highContrast])

  useEffect(() => {
    document.title = `${pageTitles[location.pathname] ?? 'IngSoft Studio'} | IngSoft Studio`
    document.getElementById('main-content')?.focus({ preventScroll: true })
  }, [location.pathname])

  return (
    <>
      <a className="skip-link" href="#main-content">Saltar al contenido principal</a>
      <aside className="accessibility-toolbar" aria-label="Herramientas de accesibilidad">
        <span className="accessibility-label">Accesibilidad</span>
        <button type="button" onClick={() => setScale((value) => Math.min(200, value + 10))} aria-label="Aumentar tamaño del texto">A+</button>
        <button type="button" onClick={() => setScale((value) => Math.max(100, value - 10))} aria-label="Reducir tamaño del texto">A−</button>
        <button type="button" onClick={() => { setScale(100); setHighContrast(false) }} aria-label="Restablecer preferencias de accesibilidad">Restablecer</button>
        <button type="button" aria-pressed={highContrast} onClick={() => setHighContrast((value) => !value)}>Alto contraste</button>
        <Link to="/accessibility">Declaración de accesibilidad</Link>
      </aside>
    </>
  )
}
