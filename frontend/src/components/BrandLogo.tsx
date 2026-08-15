type BrandLogoProps = {
  compact?: boolean
  className?: string
}

const tiles = [
  { className: 'brand-tile brand-tile--requirements', label: 'Requisitos', glyph: '✓' },
  { className: 'brand-tile brand-tile--design', label: 'Análisis y diseño', glyph: '⌘' },
  { className: 'brand-tile brand-tile--development', label: 'Desarrollo', glyph: '</>' },
  { className: 'brand-tile brand-tile--testing', label: 'Pruebas', glyph: '⚗' },
  { className: 'brand-tile brand-tile--deployment', label: 'Despliegue', glyph: '↗' },
  { className: 'brand-tile brand-tile--maintenance', label: 'Mantenimiento', glyph: '↻' },
]

export default function BrandLogo({ compact = false, className = '' }: BrandLogoProps) {
  return (
    <div className={`brand-logo ${compact ? 'brand-logo--compact' : ''} ${className}`.trim()} role="img" aria-label="Logo de IngSoft Studio">
      <div className="brand-symbol" aria-hidden="true">
        <div className="brand-symbol__center"><span>&lt;</span><strong>IS</strong><span>&gt;</span></div>
        {tiles.map((tile, index) => (
          <div key={tile.label} className={`${tile.className} brand-tile--${index + 1}`} title={tile.label}>
            <span>{tile.glyph}</span>
          </div>
        ))}
      </div>
      {!compact && (
        <div className="brand-wordmark" aria-hidden="true">
          <div><strong>Ing</strong><strong>Soft</strong></div>
          <span>STUDIO</span>
        </div>
      )}
    </div>
  )
}
