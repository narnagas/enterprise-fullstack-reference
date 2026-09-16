namespace EnterpriseFullStackReference.Api.Contracts.Projects;

public sealed class ProjectSearchRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 25;
    public string? Search { get; init; }
    public string? Customer { get; init; }
    public string? Status { get; init; }
    public bool? IsActive { get; init; }
    public string SortField { get; init; } = "createdDate";
    public string SortDirection { get; init; } = "desc";
}
