import { Link } from 'react-router-dom'
import BrandLogo from '../components/BrandLogo'

export default function AccessibilityPage() {
  return (
    <main className="accessibility-page accessibility-page--brand" aria-labelledby="accessibility-title">
      <header className="accessibility-brand-header"><BrandLogo /><div><p className="eyebrow">NORTIC B2:2017 · Accesibilidad web</p><h1 id="accessibility-title">Declaración de <span>accesibilidad</span></h1><p className="lead">IngSoft Studio incorpora criterios de accesibilidad basados en los principios Perceptible, Operable, Comprensible y Robusto, con objetivo de conformidad equivalente a los criterios A y AA aplicables de la NORTIC B2:2017.</p></div></header>

      <section aria-labelledby="implemented-title"><h2 id="implemented-title">Medidas implementadas</h2><ul><li>Navegación completa mediante teclado, orden lógico de foco y foco visible.</li><li>Enlace para saltar bloques repetitivos y acceso directo al contenido principal.</li><li>Contraste mínimo reforzado y modo de alto contraste.</li><li>Escalado de texto hasta 200 % sin pérdida intencional de funcionalidad.</li><li>Encabezados, regiones semánticas, etiquetas e instrucciones descriptivas.</li><li>Mensajes de estado y error disponibles como texto y no únicamente mediante color.</li><li>Diseño adaptable a escritorio, tableta y móvil sin desplazamiento horizontal en flujos principales.</li><li>Idioma principal declarado como español y títulos de página descriptivos.</li><li>Preferencia por texto real frente a imágenes de texto y elementos decorativos ocultos a tecnologías asistivas.</li><li>Respeto a la preferencia del sistema para reducir movimiento.</li></ul></section>

      <section aria-labelledby="scope-title"><h2 id="scope-title">Alcance</h2><p>La accesibilidad se aplica a la portada, autenticación, recuperación de contraseña, perfil, proyectos, requisitos, Quality Center y Studio Insights. Los documentos PDF y Excel generados constituyen formatos de exportación y deben revisarse nuevamente si en el futuro incorporan contenido multimedia o diagramas complejos.</p></section>

      <section aria-labelledby="status-title"><h2 id="status-title">Estado de conformidad</h2><p>Esta implementación es una adecuación técnica del proyecto a los lineamientos de NORTIC B2:2017; no representa una certificación oficial emitida por una entidad gubernamental.</p></section>

      <Link className="primary-link" to="/">Volver al inicio</Link>
    </main>
  )
}
