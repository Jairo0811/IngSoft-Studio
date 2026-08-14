using IngSoftStudio.Application.Studio;
using IngSoftStudio.Domain.Projects;
using IngSoftStudio.Domain.Quality;
using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IngSoftStudio.Infrastructure.Studio;

public sealed class StudioService(IngSoftStudioDbContext dbContext) : IStudioService
{
    private static readonly IReadOnlyCollection<SimulationScenario> Scenarios =
    [
        new(
            "requirements-change",
            "Cambio crítico de alcance",
            "El cliente solicita una funcionalidad importante cuando el sprint está a mitad y el equipo ya comprometió su capacidad.",
            "¿Cuál es la decisión más profesional?",
            [
                new("accept-now", "Aceptarla inmediatamente sin análisis", 20, "Aumenta el riesgo de alcance, calidad y previsibilidad."),
                new("assess-change", "Analizar impacto, costo, prioridad y negociar el cambio", 100, "Aplica control de cambios y protege alcance, plazo y calidad."),
                new("reject", "Rechazarla automáticamente", 40, "Evita el cambio, pero ignora el valor de negocio y la gestión de stakeholders.")
            ]),
        new(
            "critical-defect",
            "Defecto crítico antes de liberar",
            "Horas antes del release aparece un defecto reproducible que compromete una operación principal.",
            "¿Qué harías?",
            [
                new("ship", "Liberar y corregir después", 10, "Traslada un riesgo conocido a producción y puede generar un incidente grave."),
                new("evaluate-stop", "Detener, clasificar impacto y decidir con criterios de release", 100, "Prioriza evidencia, riesgo y una decisión trazable de go/no-go."),
                new("hide", "Ocultar el defecto para no retrasar", 0, "Viola principios de calidad, transparencia y responsabilidad profesional.")
            ]),
        new(
            "architecture-pressure",
            "Presión por acelerar arquitectura",
            "El equipo quiere duplicar lógica en varios módulos porque parece más rápido para la entrega inmediata.",
            "¿Cuál alternativa elegirías?",
            [
                new("duplicate", "Duplicar para terminar rápido", 20, "Introduce deuda técnica y eleva el costo de mantenimiento."),
                new("shared-boundary", "Definir una abstracción compartida con responsabilidad clara", 100, "Equilibra velocidad, DRY, cohesión y mantenibilidad."),
                new("rewrite", "Reescribir toda la solución", 30, "Es desproporcionado para el problema y aumenta el riesgo del cambio.")
            ])
    ];

    private static readonly IReadOnlyCollection<LearningTopic> Topics =
    [
        new("requirements", "Ingeniería de requisitos", "Análisis", "Proceso para descubrir, documentar, validar y gestionar necesidades del sistema.", ["Diferenciar requisitos funcionales y no funcionales", "Definir criterios de aceptación", "Mantener trazabilidad y control de cambios"]),
        new("solid", "Principios SOLID", "Diseño", "Cinco principios para construir software modular, extensible y mantenible.", ["Responsabilidad única", "Abierto/cerrado", "Sustitución de Liskov", "Segregación de interfaces", "Inversión de dependencias"]),
        new("testing", "Estrategia de pruebas", "Calidad", "Una estrategia equilibrada combina pruebas unitarias, integración y validación de flujos críticos.", ["Probar reglas de dominio", "Validar integraciones reales", "Automatizar regresión en CI"]),
        new("risk", "Gestión de riesgos", "Gestión", "Identificar incertidumbres antes de que se conviertan en problemas permite priorizar mitigaciones.", ["Probabilidad × impacto", "Mitigación", "Seguimiento continuo"]),
        new("traceability", "Trazabilidad", "Calidad", "Relaciona requisitos, pruebas y defectos para evidenciar cobertura y cambios.", ["Requirement → Test Case", "Test Case → Defect", "Cobertura medible"])
    ];

    public async Task<PortfolioDashboard> GetDashboardAsync(CancellationToken cancellationToken)
    {
        var projects = await dbContext.Projects.AsNoTracking().GroupBy(_ => 1).Select(group => new
        {
            Total = group.Count(),
            Draft = group.Count(project => project.Status == ProjectStatus.Draft),
            Active = group.Count(project => project.Status == ProjectStatus.Active),
            Completed = group.Count(project => project.Status == ProjectStatus.Completed),
            Archived = group.Count(project => project.Status == ProjectStatus.Archived)
        }).SingleOrDefaultAsync(cancellationToken);

        var totalRequirements = await dbContext.Requirements.CountAsync(cancellationToken);
        var coveredRequirements = await dbContext.TestCases.Where(test => test.RequirementId != null).Select(test => test.RequirementId).Distinct().CountAsync(cancellationToken);
        var totalTests = await dbContext.TestCases.CountAsync(cancellationToken);
        var passedTests = await dbContext.TestCases.CountAsync(test => test.Status == TestCaseStatus.Passed, cancellationToken);
        var openDefects = await dbContext.Defects.CountAsync(defect => defect.Status != DefectStatus.Closed, cancellationToken);
        var openRisks = await dbContext.Risks.CountAsync(risk => risk.Status != RiskStatus.Closed, cancellationToken);

        return new PortfolioDashboard(
            projects?.Total ?? 0,
            projects?.Draft ?? 0,
            projects?.Active ?? 0,
            projects?.Completed ?? 0,
            projects?.Archived ?? 0,
            totalRequirements,
            totalTests,
            passedTests,
            openDefects,
            openRisks,
            totalTests == 0 ? 0 : decimal.Round((decimal)passedTests / totalTests * 100, 2),
            totalRequirements == 0 ? 0 : decimal.Round((decimal)coveredRequirements / totalRequirements * 100, 2));
    }

    public IReadOnlyCollection<SimulationScenario> GetScenarios() => Scenarios;

    public SimulationResult? Evaluate(EvaluateSimulationRequest request)
    {
        var scenario = Scenarios.SingleOrDefault(item => item.Id == request.ScenarioId);
        var option = scenario?.Options.SingleOrDefault(item => item.Id == request.OptionId);
        if (option is null) return null;

        var level = option.Score switch
        {
            >= 90 => "Excelente",
            >= 70 => "Buena decisión",
            >= 40 => "Mejorable",
            _ => "Riesgo alto"
        };

        return new SimulationResult(request.ScenarioId, request.OptionId, option.Score, option.Feedback, level);
    }

    public IReadOnlyCollection<LearningTopic> GetLearningTopics() => Topics;
}
