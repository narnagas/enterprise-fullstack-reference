using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Customers;
using EnterpriseFullStackReference.Api.Security;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseFullStackReference.Api.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize(Policy = SecurityPolicies.CanRead)]
public sealed class CustomersController(ICustomerSearchService customerSearchService, ICustomerEditorService customerEditorService) : ControllerBase
{
    [HttpPost("search")]
    public async Task<ActionResult<PagedResponse<CustomerSummaryDto>>> Search([FromBody] CustomerSearchRequest request, CancellationToken cancellationToken)
        => Ok(await customerSearchService.SearchAsync(request, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var customer = await customerEditorService.GetByIdAsync(id, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = SecurityPolicies.CanEdit)]
    public async Task<ActionResult<CustomerDetailDto>> Update(int id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerEditorService.UpdateAsync(id, request, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }
}
