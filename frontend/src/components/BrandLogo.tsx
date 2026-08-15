type BrandLogoProps = {
  compact?: boolean
  className?: string
}

export default function BrandLogo({ compact = false, className = '' }: BrandLogoProps) {
  return (
    <div className={`brand-logo ${compact ? 'brand-logo--compact' : ''} ${className}`.trim()}>
      <img
        className="brand-logo__image"
        src="/ingsoft-studio-logo.webp"
        alt="IngSoft Studio"
        width={compact ? 150 : 320}
        height={compact ? 150 : 320}
      />
    </div>
  )
}
