using Microsoft.AspNetCore.Mvc;

namespace ApprovalWorkflowOrchestrator.Api.Controllers;

[ApiController]
[Route("")]
public sealed class SystemController : ControllerBase
{
    [HttpGet]
    public IActionResult GetRoot()
    {
        return Ok(new
        {
            status = "ok",
            service = "approval-workflow-orchestrator",
            docs = "/docs",
            routes = new[]
            {
                "/api/requests",
                "/api/requests/{id}",
                "/api/dashboard/summary",
                "/api/analyze/workflow"
            }
        });
    }

    [HttpGet("health")]
    public IActionResult GetHealth() => GetRoot();
}
