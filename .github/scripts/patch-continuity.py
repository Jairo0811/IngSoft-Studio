from pathlib import Path
import re

path = Path("README.md")
text = path.read_text(encoding="utf-8")

section = '''## 🧭 Continuidad académica

**IngSoft Studio** continúa una trayectoria académica previa por **compañero recurrente** con [**AuditCore**](https://github.com/Jairo0811/AuditCore) dentro de la formación de Francis Jairo Matías Rosario en el Instituto Tecnológico de Las Américas (ITLA). La relación entre ambos proyectos es **formativa y cronológica**: son aplicaciones independientes creadas posteriormente a partir de contenidos académicos distintos, pero comparten una coincidencia verificable entre integrantes de los grupos originales.

La secuencia comenzó en **2017-C2** con **Auditoría Informática (SOF-009)**, donde **Pedro Arturo de León Parra (2015-3018)** coincidió con Francis Jairo Matías Rosario en el grupo cuya exposición inspiró posteriormente AuditCore. En el período siguiente, **2017-C3**, ambos volvieron a coincidir en **Introducción a la Ingeniería en Software (SOF-015)**, materia que posteriormente dio origen conceptual a IngSoft Studio.

| Orden | Código | Asignatura | Proyecto | Período | Compañero recurrente |
|---:|---|---|---|---|---|
| 1 | SOF-009 | Auditoría Informática | [**AuditCore**](https://github.com/Jairo0811/AuditCore) | 2017-C2 | **Pedro Arturo de León Parra — 2015-3018** |
| 2 | SOF-015 | Introducción a la Ingeniería en Software | **IngSoft Studio** | 2017-C3 | **Pedro Arturo de León Parra — 2015-3018** |

Vistos en conjunto, ambos proyectos documentan una continuidad real entre compañeros durante dos períodos consecutivos y una progresión temática desde **auditoría, controles y cumplimiento** hacia **ingeniería de software, requisitos, calidad y ciclo de vida del desarrollo**. La coincidencia se considera verificada por el mismo **nombre completo y matrícula 2015-3018**.
'''

pattern = r'### Reencuentro académico.*?(?=\n\n---\n\n## 📦 Estado actual)'
new_text = re.sub(pattern, section.rstrip(), text, flags=re.S)
if new_text == text:
    raise SystemExit("Reencuentro académico section not found")

path.write_text(new_text, encoding="utf-8")
