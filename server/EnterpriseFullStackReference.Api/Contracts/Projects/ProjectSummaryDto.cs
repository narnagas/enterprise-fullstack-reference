namespace EnterpriseFullStackReference.Api.Contracts.Projects;

public sealed record ProjectSummaryDto(
    int Id,
    string ProjectNumber,
    string Name,
    string CustomerName,
    string Status,
    DateTime CreatedDate,
    DateTime? DueDate,
    bool IsActive);
