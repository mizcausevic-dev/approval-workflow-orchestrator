using ApprovalWorkflowOrchestrator.Api.Data;
using ApprovalWorkflowOrchestrator.Api.Services;

namespace ApprovalWorkflowOrchestrator.Tests;

public sealed class WorkflowAnalysisServiceTests
{
    private readonly WorkflowAnalysisService _service = new();

    [Fact]
    public void Analyze_HighPressureRequest_ReturnsEscalation()
    {
        var request = SampleApprovalData.Requests.First(item => item.Id == "apr-4018");

        var report = _service.Analyze(request);

        Assert.Equal("escalate", report.Status);
        Assert.True(report.Score >= 85);
        Assert.Contains(report.Issues, issue => issue.Code == "policy_exception");
        Assert.Contains(report.Issues, issue => issue.Code == "sla_pressure");
    }

    [Fact]
    public void Summarize_ReturnsExpectedQueueSignals()
    {
        var summary = _service.Summarize(SampleApprovalData.Requests);

        Assert.Equal(4, summary.RequestsInFlight);
        Assert.True(summary.RequestsAtRisk >= 3);
        Assert.True(summary.RequestsWithExceptions >= 2);
        Assert.NotEmpty(summary.CurrentPressureThemes);
    }
}
