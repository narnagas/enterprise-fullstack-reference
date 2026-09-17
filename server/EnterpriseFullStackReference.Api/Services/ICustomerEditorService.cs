using EnterpriseFullStackReference.Api.Contracts.Customers;

namespace EnterpriseFullStackReference.Api.Services;

public interface ICustomerEditorService
{
    Task<CustomerDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CustomerDetailDto?> UpdateAsync(int id, UpdateCustomerRequest request, CancellationToken cancellationToken = default);
}
