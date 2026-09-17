namespace EnterpriseFullStackReference.Api.Contracts.Customers;

public sealed record CustomerDetailDto(
    int Id,
    string CustomerNumber,
    string CompanyName,
    string ContactName,
    string Email,
    string Phone,
    string City,
    string State,
    DateTime CreatedDate,
    bool IsActive);
