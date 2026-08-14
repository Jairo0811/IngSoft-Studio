using IngSoftStudio.Application.Quality;
using IngSoftStudio.Domain.Quality;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IngSoftStudio.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/projects/{projectId:guid}/quality")]
public sealed class QualityController(IQualityService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<QualityDashboard>> GetDashboard(Guid projectId, CancellationToken cancellationToken) =>
        Ok(await service.GetDashboardAsync(projectId, cancellationToken));

    [HttpPost("risks")]
    public async Task<ActionResult<RiskResponse>> CreateRisk(Guid projectId, CreateRiskRequest request, CancellationToken cancellationToken) =>
        Ok(await service.CreateRiskAsync(projectId, request, cancellationToken));

    [HttpPatch("risks/{riskId:guid}/status")]
    public async Task<ActionResult<RiskResponse>> ChangeRiskStatus(Guid projectId, Guid riskId, ChangeRiskStatusRequest request, CancellationToken cancellationToken)
    {
        var risk = await service.ChangeRiskStatusAsync(projectId, riskId, request.Status, cancellationToken);
        return risk is null ? NotFound() : Ok(risk);
    }

    [HttpPost("tests")]
    public async Task<ActionResult<TestCaseResponse>> CreateTestCase(Guid projectId, CreateTestCaseRequest request, CancellationToken cancellationToken) =>
        Ok(await service.CreateTestCaseAsync(projectId, request, cancellationToken));

    [HttpPatch("tests/{testCaseId:guid}/execute")]
    public async Task<ActionResult<TestCaseResponse>> ExecuteTest(Guid projectId, Guid testCaseId, ExecuteTestCaseRequest request, CancellationToken cancellationToken)
    {
        var testCase = await service.ExecuteTestCaseAsync(projectId, testCaseId, request, cancellationToken);
        return testCase is null ? NotFound() : Ok(testCase);
    }

    [HttpPost("defects")]
    public async Task<ActionResult<DefectResponse>> CreateDefect(Guid projectId, CreateDefectRequest request, CancellationToken cancellationToken) =>
        Ok(await service.CreateDefectAsync(projectId, request, cancellationToken));

    [HttpPatch("defects/{defectId:guid}/status")]
    public async Task<ActionResult<DefectResponse>> ChangeDefectStatus(Guid projectId, Guid defectId, ChangeDefectStatusRequest request, CancellationToken cancellationToken)
    {
        var defect = await service.ChangeDefectStatusAsync(projectId, defectId, request.Status, cancellationToken);
        return defect is null ? NotFound() : Ok(defect);
    }
}
