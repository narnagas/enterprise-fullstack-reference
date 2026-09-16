namespace EnterpriseFullStackReference.Api.Contracts.Projects;

public sealed record ProjectDetailDto(
    int Id,
    string ProjectNumber,
    string Name,
    string CustomerName,
    string Status,
    DateTime CreatedDate,
    DateTime? DueDate,
    bool IsActive);
