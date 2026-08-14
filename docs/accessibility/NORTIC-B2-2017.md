# Accesibilidad web — NORTIC B2:2017

IngSoft Studio adopta como referencia la **Norma sobre Accesibilidad Web del Estado Dominicano, NORTIC B2:2017**, aplicada como buena práctica de accesibilidad para este proyecto de portafolio.

> Esta adecuación técnica no constituye una certificación oficial de NORTIC.

## Objetivo de conformidad

Se adopta como objetivo técnico el cumplimiento de los criterios **Nivel A y Nivel AA aplicables** al contenido y procesos actuales de IngSoft Studio. La norma establece que el Nivel A contiene los criterios mínimos y que el Nivel AA agrega un nivel intermedio de conformidad.

## Principios aplicados

### 1. Perceptible

- Alternativas textuales o nombres accesibles para elementos no textuales relevantes.
- Estructura semántica mediante `main`, `nav`, `header`, `section`, `article`, encabezados y etiquetas.
- La información no depende únicamente del color; estados y errores también se expresan mediante texto.
- Contraste visual reforzado, con modo adicional de alto contraste.
- Controles para escalar el texto entre 100 % y 200 %.
- Diseño responsive sin pérdida intencional de información ni funcionalidad en zoom/móvil.
- Preferencia por texto real en lugar de imágenes de texto.

### 2. Operable

- Todas las acciones principales utilizan controles HTML operables mediante teclado.
- Indicador de foco visible global mediante `:focus-visible`.
- Enlace “Saltar al contenido principal” para evitar bloques repetitivos.
- Orden del DOM alineado con el orden visual y de foco.
- Enlaces con propósito identificable por su texto.
- Navegación consistente entre los workspaces.
- No se incorporan animaciones con destellos ni contenido que parpadee.
- Se respeta `prefers-reduced-motion`.

### 3. Comprensible

- Idioma principal declarado con `lang="es"`.
- Títulos de página dinámicos y descriptivos.
- Etiquetas visibles para formularios de proyectos, requisitos y calidad.
- Mensajes de error textuales mediante regiones `role="alert"`/`aria-live` donde aplica.
- Confirmación previa para operaciones destructivas críticas, como eliminar requisitos.
- Navegación y denominaciones coherentes en las distintas pantallas.

### 4. Robusto

- Uso de HTML semántico y componentes nativos antes de ARIA personalizado.
- IDs únicos en controles etiquetados.
- Nombres y funciones determinables por software.
- Estados de controles expuestos mediante atributos como `aria-pressed`.
- Aplicación React basada en estándares del navegador y compatible con tecnologías asistivas modernas.

## Criterios A/AA relevantes implementados

| Criterio NORTIC B2 | Implementación |
|---|---|
| 3.01.3.a Información y relaciones | Regiones semánticas, encabezados y labels |
| 3.01.3.b Secuencia significativa | Orden DOM y navegación lógica |
| 3.01.4.a Uso del color | Estados acompañados por texto |
| 3.01.4.c Contraste mínimo | Paleta reforzada y modo alto contraste |
| 3.01.4.d Cambio de tamaño del texto | Herramienta A+/A− hasta 200 % |
| 3.02.1.a Teclado | Botones, enlaces, inputs y selects nativos |
| 3.02.1.b Sin trampas para el foco | Navegación estándar sin focus traps |
| 3.02.4.a Evitar bloques | Skip link al contenido principal |
| 3.02.4.b Titulado de páginas | Título dinámico por ruta |
| 3.02.4.c Orden de foco | Secuencia de DOM coherente |
| 3.02.4.d Propósito de enlaces | Textos descriptivos |
| 3.02.4.f Encabezados y etiquetas | Jerarquía y labels visibles |
| 3.02.4.g Foco visible | `:focus-visible` de alto contraste |
| 3.03.1.a Idioma de la página | `lang="es"` |
| 3.03.2.c Navegación coherente | Navegación estable por módulo |
| 3.03.2.d Identificación coherente | Misma nomenclatura para acciones repetidas |
| 3.03.3.a Identificación de errores | Mensajes textuales de error |
| 3.03.3.b Etiquetas o instrucciones | Labels explícitos en formularios principales |
| 3.03.3.d Prevención de errores | Confirmación previa a eliminación |
| 3.04.1.a Procesamiento | JSX/HTML estructurado y válido |
| 3.04.1.b Nombre, función, valor | Semántica nativa + ARIA puntual |

## Responsive

Se definieron breakpoints para escritorio, tableta y móvil. Los grids colapsan progresivamente, la navegación se reordena, los formularios pasan a una sola columna y las tablas extensas se mantienen dentro de contenedores con desplazamiento controlado cuando no es posible representarlas sin pérdida de datos.

## Mantenimiento

Toda funcionalidad nueva deberá conservar el mismo nivel objetivo de accesibilidad. Las revisiones deben incluir, como mínimo:

1. Navegación completa con teclado.
2. Foco visible.
3. Labels y nombres accesibles.
4. Contraste.
5. Zoom/texto al 200 %.
6. Reflow en anchos móviles.
7. Mensajes de error comprensibles.
8. Lectura semántica con tecnología asistiva.

Fuente normativa: NORTIC B2:2017, Norma sobre Accesibilidad Web del Estado Dominicano.
