using ApprovalWorkflowOrchestrator.Api.Models;
using ApprovalWorkflowOrchestrator.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalWorkflowOrchestrator.Api.Controllers;

[ApiController]
[Route("api/analyze")]
public sealed class WorkflowAnalysisController(WorkflowAnalysisService analysisService) : ControllerBase
{
    [HttpPost("workflow")]
    public IActionResult AnalyzeWorkflow([FromBody] ApprovalRequest request)
    {
        return Ok(analysisService.Analyze(request));
    }
}
