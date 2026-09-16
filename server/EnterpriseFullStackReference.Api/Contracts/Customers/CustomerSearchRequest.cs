namespace EnterpriseFullStackReference.Api.Contracts.Customers;

public sealed class CustomerSearchRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 25;
    public string? Search { get; init; }
    public string? State { get; init; }
    public bool? IsActive { get; init; }
    public string SortField { get; init; } = "companyName";
    public string SortDirection { get; init; } = "asc";
}
