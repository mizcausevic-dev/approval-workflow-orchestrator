using ApprovalWorkflowOrchestrator.Api.Models;

namespace ApprovalWorkflowOrchestrator.Api.Services;

public sealed class WorkflowAnalysisService
{
    public WorkflowReport Analyze(ApprovalRequest request)
    {
        var issues = new List<WorkflowIssue>();
        var passedChecks = new List<string>();

        if (request.SlaHoursRemaining <= 6)
        {
            issues.Add(new WorkflowIssue("sla_pressure", "high", "The request is close enough to the stage deadline that escalation should be prepared now."));
        }
        else
        {
            passedChecks.Add("SLA runway is still workable for the current stage.");
        }

        if (request.RequiredApprovalsPending >= 2)
        {
            issues.Add(new WorkflowIssue("approval_depth", "high", "Multiple approvals are still pending, which increases the chance of sequential delay."));
        }
        else
        {
            passedChecks.Add("Approval depth is contained to a narrow owner set.");
        }

        if (request.HasPolicyException)
        {
            issues.Add(new WorkflowIssue("policy_exception", "critical", "The request includes non-standard terms and should remain in a governed exception lane."));
        }
        else
        {
            passedChecks.Add("The request is within standard policy posture.");
        }

        if (request.LaunchCritical)
        {
            issues.Add(new WorkflowIssue("launch_critical", "high", "Delay pressure is amplified because the request is attached to a launch or live commercial event."));
        }
        else
        {
            passedChecks.Add("The request is not attached to a near-term launch window.");
        }

        if (request.RequestAmount >= 250000m)
        {
            issues.Add(new WorkflowIssue("amount_exposure", "moderate", "The request carries enough contract or spend exposure to warrant tighter senior visibility."));
        }
        else
        {
            passedChecks.Add("Commercial exposure is moderate relative to the current lane.");
        }

        var score = CalculateScore(request, issues);
        var status = score >= 85 ? "escalate" : score >= 65 ? "watch" : "stable";

        return new WorkflowReport(
            request.Id,
            status,
            score,
            issues,
            passedChecks,
            RecommendNextAction(request, issues),
            request.Owners);
    }

    public DashboardSummary Summarize(IReadOnlyList<ApprovalRequest> requests)
    {
        var reports = requests.Select(Analyze).ToList();

        return new DashboardSummary(
            requests.Count,
            reports.Count(report => report.Score >= 65),
            requests.Count(request => request.HasPolicyException),
            requests.Count(request => request.LaunchCritical),
            BuildPressureThemes(reports));
    }

    private static int CalculateScore(ApprovalRequest request, IReadOnlyList<WorkflowIssue> issues)
    {
        var score = 24;

        score += Math.Max(0, 24 - request.SlaHoursRemaining);
        score += request.RequiredApprovalsPending * 8;
        score += request.HasPolicyException ? 18 : 0;
        score += request.LaunchCritical ? 14 : 0;
        score += request.RequestAmount >= 250000m ? 10 : request.RequestAmount >= 100000m ? 6 : 0;
        score += request.Priority switch
        {
            "critical" => 12,
            "high" => 8,
            "medium" => 4,
            _ => 0
        };
        score += issues.Count * 3;

        return Math.Min(score, 97);
    }

    private static string RecommendNextAction(ApprovalRequest request, IReadOnlyList<WorkflowIssue> issues)
    {
        if (issues.Any(issue => issue.Code == "policy_exception") && request.SlaHoursRemaining <= 6)
        {
            return "Escalate the exception lane immediately, attach executive owner visibility, and collapse remaining approvals into the current review window.";
        }

        if (request.RequiredApprovalsPending >= 2)
        {
            return "Convert the request into a same-day owner sweep so the remaining approvals do not continue sequentially.";
        }

        if (request.LaunchCritical)
        {
            return "Keep the request in the launch-critical lane and assign a single accountable coordinator for the remaining stage movement.";
        }

        return "Keep the request moving in the current lane and verify the next owner handoff before the SLA window compresses further.";
    }

    private static IReadOnlyList<string> BuildPressureThemes(IEnumerable<WorkflowReport> reports)
    {
        var issueCodes = reports
            .SelectMany(report => report.Issues)
            .GroupBy(issue => issue.Code)
            .OrderByDescending(group => group.Count())
            .Take(3)
            .Select(group => group.Key switch
            {
                "sla_pressure" => "SLA pressure is shaping the current queue.",
                "policy_exception" => "Exception posture is demanding tighter governed lanes.",
                "approval_depth" => "Approval depth is creating sequential delay risk.",
                "launch_critical" => "Launch-linked requests are concentrating operational pressure.",
                "amount_exposure" => "Higher-value requests deserve more direct visibility.",
                _ => group.Key
            })
            .ToList();

        return issueCodes;
    }
}
