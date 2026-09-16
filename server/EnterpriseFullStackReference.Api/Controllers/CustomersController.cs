using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Customers;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseFullStackReference.Api.Controllers;

[ApiController]
[Route("api/customers")]
public sealed class CustomersController(ICustomerSearchService customerSearchService) : ControllerBase
{
    [HttpPost("search")]
    [ProducesResponseType(typeof(PagedResponse<CustomerSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<CustomerSummaryDto>>> Search(
        [FromBody] CustomerSearchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await customerSearchService.SearchAsync(request, cancellationToken);
        return Ok(result);
    }
}
