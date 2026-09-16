using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Customers;

namespace EnterpriseFullStackReference.Api.Services;

public interface ICustomerSearchService
{
    Task<PagedResponse<CustomerSummaryDto>> SearchAsync(
        CustomerSearchRequest request,
        CancellationToken cancellationToken = default);
}
