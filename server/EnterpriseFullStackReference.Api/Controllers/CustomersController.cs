using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Customers;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseFullStackReference.Api.Controllers;

[ApiController]
[Route("api/customers")]
public sealed class CustomersController(
    ICustomerSearchService customerSearchService,
    ICustomerEditorService customerEditorService) : ControllerBase
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

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CustomerDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var customer = await customerEditorService.GetByIdAsync(id, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CustomerDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDetailDto>> Update(
        int id,
        [FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await customerEditorService.UpdateAsync(id, request, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }
}
