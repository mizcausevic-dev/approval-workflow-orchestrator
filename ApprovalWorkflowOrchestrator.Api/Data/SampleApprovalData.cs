using ApprovalWorkflowOrchestrator.Api.Models;

namespace ApprovalWorkflowOrchestrator.Api.Data;

public static class SampleApprovalData
{
    public static IReadOnlyList<ApprovalRequest> Requests { get; } =
    [
        new ApprovalRequest(
            "apr-4018",
            "Enterprise annual renewal with custom security language",
            "revenue-operations",
            "high",
            "legal-review",
            285000m,
            6,
            2,
            true,
            true,
            ["legal", "security", "deal-desk"],
            [
                new AuditEvent(DateTimeOffset.Parse("2026-05-09T10:00:00Z"), "sales-ops", "submitted", "intake", "Renewal package entered workflow."),
                new AuditEvent(DateTimeOffset.Parse("2026-05-09T14:30:00Z"), "deal-desk", "approved", "commercial-review", "Discount posture approved."),
                new AuditEvent(DateTimeOffset.Parse("2026-05-10T12:15:00Z"), "legal", "in-review", "legal-review", "Custom security rider under review.")
            ]),
        new ApprovalRequest(
            "apr-4024",
            "Vendor onboarding with regional data-processing addendum",
            "security-governance",
            "medium",
            "security-review",
            98000m,
            18,
            1,
            true,
            false,
            ["security", "procurement"],
            [
                new AuditEvent(DateTimeOffset.Parse("2026-05-08T15:00:00Z"), "procurement", "submitted", "intake", "Vendor risk packet attached."),
                new AuditEvent(DateTimeOffset.Parse("2026-05-09T09:20:00Z"), "procurement", "advanced", "security-review", "Regional processing addendum escalated.")
            ]),
        new ApprovalRequest(
            "apr-4031",
            "Pricing rollout approval for new marketplace package",
            "growth-systems",
            "high",
            "finance-review",
            142000m,
            10,
            1,
            false,
            true,
            ["finance", "product-marketing"],
            [
                new AuditEvent(DateTimeOffset.Parse("2026-05-10T08:10:00Z"), "pricing-ops", "submitted", "intake", "Package ladder updated."),
                new AuditEvent(DateTimeOffset.Parse("2026-05-10T09:45:00Z"), "product-marketing", "approved", "go-to-market-review", "Launch copy aligned.")
            ]),
        new ApprovalRequest(
            "apr-4037",
            "Internal AI prompt release with elevated data-retention exception",
            "ai-operations",
            "critical",
            "governance-review",
            0m,
            4,
            3,
            true,
            true,
            ["ai-governance", "security", "privacy"],
            [
                new AuditEvent(DateTimeOffset.Parse("2026-05-10T06:40:00Z"), "ai-platform", "submitted", "intake", "Model release candidate entered."),
                new AuditEvent(DateTimeOffset.Parse("2026-05-10T07:05:00Z"), "security", "requested-change", "governance-review", "Retention exception requires privacy signoff.")
            ])
    ];
}
