using System.Globalization;
using ClosedXML.Excel;
using IngSoftStudio.Application.Studio;
using IngSoftStudio.Domain.Projects;
using IngSoftStudio.Domain.Quality;
using IngSoftStudio.Domain.Studio;
using IngSoftStudio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace IngSoftStudio.Infrastructure.Studio;

public sealed class StudioService(IngSoftStudioDbContext dbContext) : IStudioService
{
    private static readonly IReadOnlyCollection<SimulationScenario> Scenarios =
    [
        new("requirements-change", "Cambio crítico de alcance", "El cliente solicita una funcionalidad importante cuando el sprint está a mitad y el equipo ya comprometió su capacidad.", "¿Cuál es la decisión más profesional?", [new("accept-now", "Aceptarla inmediatamente sin análisis", 20, "Aumenta el riesgo de alcance, calidad y previsibilidad."), new("assess-change", "Analizar impacto, costo, prioridad y negociar el cambio", 100, "Aplica control de cambios y protege alcance, plazo y calidad."), new("reject", "Rechazarla automáticamente", 40, "Evita el cambio, pero ignora el valor de negocio y la gestión de stakeholders.")]),
        new("critical-defect", "Defecto crítico antes de liberar", "Horas antes del release aparece un defecto reproducible que compromete una operación principal.", "¿Qué harías?", [new("ship", "Liberar y corregir después", 10, "Traslada un riesgo conocido a producción y puede generar un incidente grave."), new("evaluate-stop", "Detener, clasificar impacto y decidir con criterios de release", 100, "Prioriza evidencia, riesgo y una decisión trazable de go/no-go."), new("hide", "Ocultar el defecto para no retrasar", 0, "Viola principios de calidad, transparencia y responsabilidad profesional.")]),
        new("architecture-pressure", "Presión por acelerar arquitectura", "El equipo quiere duplicar lógica en varios módulos porque parece más rápido para la entrega inmediata.", "¿Cuál alternativa elegirías?", [new("duplicate", "Duplicar para terminar rápido", 20, "Introduce deuda técnica y eleva el costo de mantenimiento."), new("shared-boundary", "Definir una abstracción compartida con responsabilidad clara", 100, "Equilibra velocidad, DRY, cohesión y mantenibilidad."), new("rewrite", "Reescribir toda la solución", 30, "Es desproporcionado para el problema y aumenta el riesgo del cambio.")]),
        new("test-coverage", "Cobertura insuficiente", "Un requisito Must está listo para pasar a producción, pero no tiene ningún caso de prueba asociado.", "¿Qué decisión corresponde?", [new("release", "Liberar porque el desarrollador ya lo probó", 25, "La validación informal no aporta evidencia repetible ni trazabilidad."), new("add-tests", "Crear y ejecutar pruebas trazables antes del release", 100, "Asegura cobertura, evidencia y control de regresión."), new("downgrade", "Cambiar el requisito a Could para evitar probarlo", 10, "Manipula la prioridad sin resolver el riesgo real.")]),
        new("risk-response", "Riesgo de alta exposición", "Un riesgo de alta probabilidad e impacto puede afectar una entrega crítica y todavía no tiene mitigación.", "¿Cuál es la mejor respuesta?", [new("ignore", "Esperar a que ocurra", 5, "Convierte una incertidumbre conocida en una crisis evitable."), new("mitigate", "Asignar responsable, mitigación, contingencia y seguimiento", 100, "Reduce exposición y convierte el riesgo en una decisión gestionable."), new("close", "Cerrar el riesgo para limpiar el tablero", 0, "Oculta información y destruye la trazabilidad del riesgo.")])
    ];

    private static readonly IReadOnlyCollection<LearningTopic> Topics =
    [
        new("requirements", "Ingeniería de requisitos", "Análisis", "Proceso para descubrir, documentar, validar y gestionar necesidades del sistema.", ["Diferenciar requisitos funcionales y no funcionales", "Definir criterios de aceptación", "Mantener trazabilidad y control de cambios"]),
        new("user-stories", "Historias de usuario", "Análisis", "Describen una necesidad desde la perspectiva del usuario y conectan valor de negocio con requisitos verificables.", ["Como [rol], quiero [objetivo], para [beneficio]", "Definir criterios de aceptación claros", "Refinar con usuarios y stakeholders"]),
        new("solid", "Principios SOLID", "Diseño", "Cinco principios para construir software modular, extensible y mantenible.", ["Responsabilidad única", "Abierto/cerrado", "Sustitución de Liskov", "Segregación de interfaces", "Inversión de dependencias"]),
        new("testing", "Estrategia de pruebas", "Calidad", "Una estrategia equilibrada combina pruebas unitarias, integración y validación de flujos críticos.", ["Probar reglas de dominio", "Validar integraciones reales", "Automatizar regresión en CI"]),
        new("black-white-box", "Caja blanca y caja negra", "Calidad", "Dos enfoques complementarios permiten validar el comportamiento observable y la lógica interna del software.", ["Caja negra: entradas, salidas y requisitos", "Caja blanca: lógica, caminos, condiciones y cobertura", "Combinar ambas técnicas mejora la detección de defectos"]),
        new("risk", "Gestión de riesgos", "Gestión", "Identificar incertidumbres antes de que se conviertan en problemas permite priorizar mitigaciones.", ["Probabilidad × impacto", "Mitigación", "Seguimiento continuo"]),
        new("traceability", "Trazabilidad", "Calidad", "Relaciona requisitos, pruebas y defectos para evidenciar cobertura y cambios.", ["Requirement → Test Case", "Test Case → Defect", "Cobertura medible"]),
        new("change-control", "Control de cambios", "Gestión", "Los cambios deben evaluarse por valor, costo, riesgo e impacto antes de incorporarse.", ["Analizar impacto", "Negociar prioridad", "Registrar la decisión"]),
        new("release", "Criterios de liberación", "Calidad", "Un release profesional se basa en evidencia y umbrales de aceptación, no solo en fechas.", ["Defectos críticos", "Cobertura", "Go/No-Go trazable"])
    ];

    public async Task<PortfolioDashboard> GetDashboardAsync(CancellationToken cancellationToken)
    {
        var projects = await dbContext.Projects.AsNoTracking().GroupBy(_ => 1).Select(group => new { Total = group.Count(), Draft = group.Count(x => x.Status == ProjectStatus.Draft), Active = group.Count(x => x.Status == ProjectStatus.Active), Completed = group.Count(x => x.Status == ProjectStatus.Completed), Archived = group.Count(x => x.Status == ProjectStatus.Archived) }).SingleOrDefaultAsync(cancellationToken);
        var totalRequirements = await dbContext.Requirements.CountAsync(cancellationToken);
        var coveredRequirements = await dbContext.TestCases.Where(x => x.RequirementId != null).Select(x => x.RequirementId).Distinct().CountAsync(cancellationToken);
        var totalTests = await dbContext.TestCases.CountAsync(cancellationToken);
        var passedTests = await dbContext.TestCases.CountAsync(x => x.Status == TestCaseStatus.Passed, cancellationToken);
        var openDefects = await dbContext.Defects.CountAsync(x => x.Status != DefectStatus.Closed && x.Status != DefectStatus.Resolved, cancellationToken);
        var openRisks = await dbContext.Risks.CountAsync(x => x.Status != RiskStatus.Closed && x.Status != RiskStatus.Accepted, cancellationToken);
        return new PortfolioDashboard(projects?.Total ?? 0, projects?.Draft ?? 0, projects?.Active ?? 0, projects?.Completed ?? 0, projects?.Archived ?? 0, totalRequirements, totalTests, passedTests, openDefects, openRisks, Percent(passedTests, totalTests), Percent(coveredRequirements, totalRequirements));
    }

    public async Task<IReadOnlyCollection<ProjectInsight>> GetProjectInsightsAsync(CancellationToken cancellationToken)
    {
        var projects = await dbContext.Projects.AsNoTracking().OrderBy(x => x.Name).ToListAsync(cancellationToken);
        var result = new List<ProjectInsight>(projects.Count);
        foreach (var project in projects)
        {
            var requirements = await dbContext.Requirements.CountAsync(x => x.ProjectId == project.Id, cancellationToken);
            var covered = await dbContext.TestCases.Where(x => x.ProjectId == project.Id && x.RequirementId != null).Select(x => x.RequirementId).Distinct().CountAsync(cancellationToken);
            var tests = await dbContext.TestCases.CountAsync(x => x.ProjectId == project.Id, cancellationToken);
            var passed = await dbContext.TestCases.CountAsync(x => x.ProjectId == project.Id && x.Status == TestCaseStatus.Passed, cancellationToken);
            var defects = await dbContext.Defects.CountAsync(x => x.ProjectId == project.Id && x.Status != DefectStatus.Closed && x.Status != DefectStatus.Resolved, cancellationToken);
            var risks = await dbContext.Risks.CountAsync(x => x.ProjectId == project.Id && x.Status != RiskStatus.Closed && x.Status != RiskStatus.Accepted, cancellationToken);
            result.Add(new ProjectInsight(project.Id, project.Name, project.Status.ToString(), requirements, tests, passed, defects, risks, Percent(covered, requirements), Percent(passed, tests)));
        }
        return result;
    }

    public async Task<IReadOnlyCollection<PortfolioTrend>> GetTrendsAsync(CancellationToken cancellationToken)
    {
        var projects = await GetProjectInsightsAsync(cancellationToken);
        return projects.Select(x => new PortfolioTrend(x.ProjectName, x.Requirements, x.Tests, x.OpenDefects, x.OpenRisks)).ToArray();
    }

    public IReadOnlyCollection<SimulationScenario> GetScenarios() => Scenarios;

    public async Task<SimulationResult?> EvaluateAsync(Guid userId, EvaluateSimulationRequest request, CancellationToken cancellationToken)
    {
        var scenario = Scenarios.SingleOrDefault(x => x.Id == request.ScenarioId);
        var option = scenario?.Options.SingleOrDefault(x => x.Id == request.OptionId);
        if (option is null) return null;
        var level = option.Score switch { >= 90 => "Excelente", >= 70 => "Buena decisión", >= 40 => "Mejorable", _ => "Riesgo alto" };
        dbContext.SimulationAttempts.Add(new SimulationAttempt(userId, request.ScenarioId, request.OptionId, option.Score, level));
        await dbContext.SaveChangesAsync(cancellationToken);
        return new SimulationResult(request.ScenarioId, request.OptionId, option.Score, option.Feedback, level);
    }

    public async Task<SimulationSummary> GetSimulationSummaryAsync(Guid userId, CancellationToken cancellationToken)
    {
        var attempts = await dbContext.SimulationAttempts.AsNoTracking().Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAtUtc).ToListAsync(cancellationToken);
        var recent = attempts.Take(10).Select(x => new SimulationAttemptResponse(x.Id, x.ScenarioId, x.OptionId, x.Score, x.Level, x.CreatedAtUtc)).ToArray();
        return new SimulationSummary(attempts.Count, attempts.Count == 0 ? 0 : decimal.Round((decimal)attempts.Average(x => x.Score), 2), attempts.Count == 0 ? 0 : attempts.Max(x => x.Score), recent);
    }

    public IReadOnlyCollection<LearningTopic> GetLearningTopics() => Topics;

    public async Task<ReportFile> BuildPdfReportAsync(CancellationToken cancellationToken)
    {
        var dashboard = await GetDashboardAsync(cancellationToken);
        var projects = await GetProjectInsightsAsync(cancellationToken);
        var generatedAt = DateTime.UtcNow;
        var releaseAssessment = BuildReleaseAssessment(dashboard);

        QuestPDF.Settings.License = LicenseType.Community;
        var bytes = Document.Create(container => container.Page(page =>
        {
            page.Margin(32);
            page.Header().Column(column =>
            {
                column.Item().Text("IngSoft Studio").FontSize(11).Bold().FontColor(Colors.Teal.Medium);
                column.Item().Text("Reporte Ejecutivo de Calidad y Portafolio").FontSize(22).Bold();
                column.Item().Text($"Generado UTC: {generatedAt:yyyy-MM-dd HH:mm}").FontSize(9).FontColor(Colors.Grey.Medium);
            });
            page.Content().PaddingVertical(16).Column(column =>
            {
                column.Spacing(14);
                column.Item().Text("Resumen ejecutivo").FontSize(16).Bold();
                column.Item().Text("Consolidado de proyectos, requisitos, cobertura, ejecución de pruebas y elementos de calidad pendientes. Los indicadores utilizan la misma semántica que Studio Insights: los defectos resueltos y los riesgos aceptados no se contabilizan como abiertos.");

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns => { for (var i = 0; i < 7; i++) columns.RelativeColumn(); });
                    table.Header(header =>
                    {
                        foreach (var title in new[] { "Proyectos", "Activos", "Requisitos", "Pruebas", "Cobertura", "Pass rate", "Pendientes" }) header.Cell().Padding(4).Text(title).Bold();
                    });
                    table.Cell().Padding(4).Text(dashboard.TotalProjects.ToString(CultureInfo.InvariantCulture));
                    table.Cell().Padding(4).Text(dashboard.ActiveProjects.ToString(CultureInfo.InvariantCulture));
                    table.Cell().Padding(4).Text(dashboard.TotalRequirements.ToString(CultureInfo.InvariantCulture));
                    table.Cell().Padding(4).Text(dashboard.TotalTests.ToString(CultureInfo.InvariantCulture));
                    table.Cell().Padding(4).Text($"{dashboard.RequirementCoveragePercent}%");
                    table.Cell().Padding(4).Text($"{dashboard.TestPassRatePercent}%");
                    table.Cell().Padding(4).Text((dashboard.OpenDefects + dashboard.OpenRisks).ToString(CultureInfo.InvariantCulture));
                });

                column.Item().Text("Evaluación de liberación").FontSize(16).Bold();
                column.Item().Text(releaseAssessment);
                column.Item().Text($"Defectos abiertos: {dashboard.OpenDefects} · Riesgos abiertos: {dashboard.OpenRisks}");

                column.Item().Text("Detalle por proyecto").FontSize(16).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns => { columns.RelativeColumn(3); columns.RelativeColumn(); columns.RelativeColumn(); columns.RelativeColumn(); columns.RelativeColumn(); columns.RelativeColumn(); columns.RelativeColumn(); });
                    table.Header(header =>
                    {
                        foreach (var title in new[] { "Proyecto", "Estado", "Req.", "Tests", "Cobertura", "Def. abiertos", "Riesgos abiertos" }) header.Cell().Padding(3).Text(title).Bold();
                    });
                    foreach (var project in projects)
                    {
                        table.Cell().Padding(3).Text(project.ProjectName);
                        table.Cell().Padding(3).Text(project.Status);
                        table.Cell().Padding(3).Text(project.Requirements.ToString(CultureInfo.InvariantCulture));
                        table.Cell().Padding(3).Text(project.Tests.ToString(CultureInfo.InvariantCulture));
                        table.Cell().Padding(3).Text($"{project.CoveragePercent}%");
                        table.Cell().Padding(3).Text(project.OpenDefects.ToString(CultureInfo.InvariantCulture));
                        table.Cell().Padding(3).Text(project.OpenRisks.ToString(CultureInfo.InvariantCulture));
                    }
                });

                column.Item().Text("Criterios de interpretación").FontSize(16).Bold();
                column.Item().Text("• Cobertura: requisitos con al menos un caso de prueba asociado.\n• Pass rate: casos de prueba aprobados sobre el total registrado.\n• Defectos abiertos: estados operativos pendientes; Resolved y Closed quedan fuera.\n• Riesgos abiertos: riesgos todavía gestionables; Accepted y Closed quedan fuera.");
            });
            page.Footer().AlignCenter().Text("IngSoft Studio · Engineering Better Software").FontSize(9).FontColor(Colors.Grey.Medium);
        })).GeneratePdf();
        return new ReportFile(bytes, "application/pdf", $"ingsoft-studio-report-{generatedAt:yyyyMMdd}.pdf");
    }

    public async Task<ReportFile> BuildExcelReportAsync(CancellationToken cancellationToken)
    {
        var dashboard = await GetDashboardAsync(cancellationToken);
        var projects = await GetProjectInsightsAsync(cancellationToken);
        using var workbook = new XLWorkbook();

        var summary = workbook.Worksheets.Add("Resumen");
        summary.Cell("A1").Value = "IngSoft Studio — Reporte Ejecutivo de Calidad y Portafolio";
        summary.Range("A1:B1").Merge().Style.Font.SetBold().Font.SetFontSize(16);
        summary.Cell("A2").Value = "Generado UTC"; summary.Cell("B2").Value = DateTime.UtcNow;
        var summaryRows = new (string Label, object Value)[]
        {
            ("Proyectos", dashboard.TotalProjects), ("Proyectos activos", dashboard.ActiveProjects), ("Requisitos", dashboard.TotalRequirements),
            ("Pruebas", dashboard.TotalTests), ("Pruebas aprobadas", dashboard.PassedTests), ("Cobertura %", dashboard.RequirementCoveragePercent),
            ("Pass rate %", dashboard.TestPassRatePercent), ("Defectos abiertos", dashboard.OpenDefects), ("Riesgos abiertos", dashboard.OpenRisks),
            ("Evaluación de liberación", BuildReleaseAssessment(dashboard))
        };
        for (var i = 0; i < summaryRows.Length; i++)
        {
            summary.Cell(i + 4, 1).Value = summaryRows[i].Label;
            summary.Cell(i + 4, 2).Value = XLCellValue.FromObject(summaryRows[i].Value);
        }
        summary.Range(4, 1, summaryRows.Length + 3, 1).Style.Font.Bold = true;
        summary.Columns().AdjustToContents();

        var detail = workbook.Worksheets.Add("Proyectos");
        var headers = new[] { "Proyecto", "Estado", "Requisitos", "Pruebas", "Aprobadas", "Defectos abiertos", "Riesgos abiertos", "Cobertura %", "Pass rate %" };
        for (var i = 0; i < headers.Length; i++) detail.Cell(1, i + 1).Value = headers[i];
        var row = 2;
        foreach (var project in projects)
        {
            detail.Cell(row, 1).Value = project.ProjectName; detail.Cell(row, 2).Value = project.Status; detail.Cell(row, 3).Value = project.Requirements; detail.Cell(row, 4).Value = project.Tests; detail.Cell(row, 5).Value = project.PassedTests; detail.Cell(row, 6).Value = project.OpenDefects; detail.Cell(row, 7).Value = project.OpenRisks; detail.Cell(row, 8).Value = project.CoveragePercent; detail.Cell(row, 9).Value = project.PassRatePercent; row++;
        }
        detail.Range(1, 1, 1, headers.Length).Style.Font.Bold = true;
        detail.SheetView.FreezeRows(1);
        detail.Columns().AdjustToContents();

        var definitions = workbook.Worksheets.Add("Definiciones");
        definitions.Cell("A1").Value = "Indicador"; definitions.Cell("B1").Value = "Definición";
        definitions.Cell("A2").Value = "Cobertura"; definitions.Cell("B2").Value = "Requisitos con al menos un caso de prueba asociado.";
        definitions.Cell("A3").Value = "Pass rate"; definitions.Cell("B3").Value = "Casos de prueba aprobados sobre el total registrado.";
        definitions.Cell("A4").Value = "Defectos abiertos"; definitions.Cell("B4").Value = "Defectos pendientes de atención; Resolved y Closed no se contabilizan.";
        definitions.Cell("A5").Value = "Riesgos abiertos"; definitions.Cell("B5").Value = "Riesgos pendientes de gestión; Accepted y Closed no se contabilizan.";
        definitions.Range("A1:B1").Style.Font.Bold = true;
        definitions.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return new ReportFile(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ingsoft-studio-report-{DateTime.UtcNow:yyyyMMdd}.xlsx");
    }

    private static string BuildReleaseAssessment(PortfolioDashboard dashboard)
    {
        if (dashboard.OpenDefects > 0) return "NO-GO: existen defectos abiertos que requieren evaluación antes de liberar.";
        if (dashboard.OpenRisks > 0) return "REVISAR: no hay defectos abiertos, pero existen riesgos pendientes de gestión.";
        if (dashboard.TotalRequirements > 0 && dashboard.RequirementCoveragePercent < 100) return "REVISAR: existen requisitos sin cobertura de pruebas completa.";
        if (dashboard.TotalTests > 0 && dashboard.TestPassRatePercent < 100) return "REVISAR: no todos los casos de prueba registrados están aprobados.";
        return dashboard.TotalTests == 0 ? "SIN EVIDENCIA: todavía no existen casos de prueba para sustentar una decisión de liberación." : "GO: no existen defectos ni riesgos abiertos y la evidencia de pruebas registrada cumple los criterios actuales.";
    }

    private static decimal Percent(int value, int total) => total == 0 ? 0 : decimal.Round((decimal)value / total * 100, 2);
}
