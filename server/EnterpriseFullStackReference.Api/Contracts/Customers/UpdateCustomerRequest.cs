using System.ComponentModel.DataAnnotations;

namespace EnterpriseFullStackReference.Api.Contracts.Customers;

public sealed class UpdateCustomerRequest
{
    [Required, StringLength(150)]
    public string CompanyName { get; init; } = string.Empty;

    [StringLength(150)]
    public string ContactName { get; init; } = string.Empty;

    [EmailAddress, StringLength(200)]
    public string Email { get; init; } = string.Empty;

    [StringLength(50)]
    public string Phone { get; init; } = string.Empty;

    [StringLength(100)]
    public string City { get; init; } = string.Empty;

    [StringLength(50)]
    public string State { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
