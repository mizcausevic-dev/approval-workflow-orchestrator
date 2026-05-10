using ApprovalWorkflowOrchestrator.Api.Data;
using ApprovalWorkflowOrchestrator.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalWorkflowOrchestrator.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class ApprovalRequestsController(WorkflowAnalysisService analysisService) : ControllerBase
{
    [HttpGet("requests")]
    public IActionResult GetRequests()
    {
        return Ok(SampleApprovalData.Requests);
    }

    [HttpGet("requests/{id}")]
    public IActionResult GetRequest(string id)
    {
        var request = SampleApprovalData.Requests.FirstOrDefault(item => string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase));
        return request is null ? NotFound(new { error = "request_not_found" }) : Ok(request);
    }

    [HttpGet("dashboard/summary")]
    public IActionResult GetDashboardSummary()
    {
        return Ok(analysisService.Summarize(SampleApprovalData.Requests));
    }
}
