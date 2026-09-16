using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Projects;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseFullStackReference.Api.Controllers;

[ApiController]
[Route("api/projects")]
public sealed class ProjectsController(IProjectSearchService projectSearchService)
    : ControllerBase
{
    [HttpPost("search")]
    [ProducesResponseType(typeof(PagedResponse<ProjectSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ProjectSummaryDto>>> Search(
        [FromBody] ProjectSearchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await projectSearchService.SearchAsync(
            request,
            cancellationToken);

        return Ok(result);
    }
}
