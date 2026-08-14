using System.Security.Claims;
using IngSoftStudio.Application.Studio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IngSoftStudio.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/studio")]
public sealed class StudioController(IStudioService service) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<PortfolioDashboard>> GetDashboard(CancellationToken cancellationToken) =>
        Ok(await service.GetDashboardAsync(cancellationToken));

    [HttpGet("projects")]
    public async Task<ActionResult<IReadOnlyCollection<ProjectInsight>>> GetProjectInsights(CancellationToken cancellationToken) =>
        Ok(await service.GetProjectInsightsAsync(cancellationToken));

    [HttpGet("trends")]
    public async Task<ActionResult<IReadOnlyCollection<PortfolioTrend>>> GetTrends(CancellationToken cancellationToken) =>
        Ok(await service.GetTrendsAsync(cancellationToken));

    [HttpGet("simulation/scenarios")]
    public ActionResult<IReadOnlyCollection<SimulationScenario>> GetScenarios() => Ok(service.GetScenarios());

    [HttpPost("simulation/evaluate")]
    public async Task<ActionResult<SimulationResult>> Evaluate(EvaluateSimulationRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var result = await service.EvaluateAsync(userId, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("simulation/summary")]
    public async Task<ActionResult<SimulationSummary>> GetSimulationSummary(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        return Ok(await service.GetSimulationSummaryAsync(userId, cancellationToken));
    }

    [HttpGet("learning")]
    public ActionResult<IReadOnlyCollection<LearningTopic>> GetLearningTopics() => Ok(service.GetLearningTopics());

    [HttpGet("reports/pdf")]
    public async Task<IActionResult> DownloadPdf(CancellationToken cancellationToken)
    {
        var report = await service.BuildPdfReportAsync(cancellationToken);
        return File(report.Content, report.ContentType, report.FileName);
    }

    [HttpGet("reports/excel")]
    public async Task<IActionResult> DownloadExcel(CancellationToken cancellationToken)
    {
        var report = await service.BuildExcelReportAsync(cancellationToken);
        return File(report.Content, report.ContentType, report.FileName);
    }

    private bool TryGetUserId(out Guid userId) => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
}
