namespace ApprovalWorkflowOrchestrator.Api.Models;

public sealed record ApprovalRequest(
    string Id,
    string Title,
    string BusinessDomain,
    string Priority,
    string CurrentStage,
    decimal RequestAmount,
    int SlaHoursRemaining,
    int RequiredApprovalsPending,
    bool HasPolicyException,
    bool LaunchCritical,
    IReadOnlyList<string> Owners,
    IReadOnlyList<AuditEvent> AuditTrail);

public sealed record AuditEvent(
    DateTimeOffset Timestamp,
    string Actor,
    string Action,
    string Stage,
    string Notes);

public sealed record WorkflowIssue(
    string Code,
    string Severity,
    string Summary);

public sealed record WorkflowReport(
    string RequestId,
    string Status,
    int Score,
    IReadOnlyList<WorkflowIssue> Issues,
    IReadOnlyList<string> PassedChecks,
    string RecommendedNextAction,
    IReadOnlyList<string> OwnershipLanes);

public sealed record DashboardSummary(
    int RequestsInFlight,
    int RequestsAtRisk,
    int RequestsWithExceptions,
    int LaunchCriticalRequests,
    IReadOnlyList<string> CurrentPressureThemes);
