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

    [HttpGet("simulation/scenarios")]
    public ActionResult<IReadOnlyCollection<SimulationScenario>> GetScenarios() => Ok(service.GetScenarios());

    [HttpPost("simulation/evaluate")]
    public ActionResult<SimulationResult> Evaluate(EvaluateSimulationRequest request)
    {
        var result = service.Evaluate(request);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("learning")]
    public ActionResult<IReadOnlyCollection<LearningTopic>> GetLearningTopics() => Ok(service.GetLearningTopics());
}
