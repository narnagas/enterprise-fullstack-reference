using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Projects;
using EnterpriseFullStackReference.Api.Security;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseFullStackReference.Api.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize(Policy = SecurityPolicies.CanRead)]
public sealed class ProjectsController(IProjectSearchService projectSearchService, IProjectEditorService projectEditorService) : ControllerBase
{
    [HttpPost("search")]
    public async Task<ActionResult<PagedResponse<ProjectSummaryDto>>> Search([FromBody] ProjectSearchRequest request, CancellationToken cancellationToken)
        => Ok(await projectSearchService.SearchAsync(request, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var project = await projectEditorService.GetByIdAsync(id, cancellationToken);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = SecurityPolicies.CanEdit)]
    public async Task<ActionResult<ProjectDetailDto>> Update(int id, [FromBody] UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await projectEditorService.UpdateAsync(id, request, cancellationToken);
        return project is null ? NotFound() : Ok(project);
    }
}
