using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Projects;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseFullStackReference.Api.Controllers;

[ApiController]
[Route("api/projects")]
public sealed class ProjectsController(
    IProjectSearchService projectSearchService,
    IProjectEditorService projectEditorService)
    : ControllerBase
{
    [HttpPost("search")]
    [ProducesResponseType(typeof(PagedResponse<ProjectSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ProjectSummaryDto>>> Search(
        [FromBody] ProjectSearchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await projectSearchService.SearchAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProjectDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var project = await projectEditorService.GetByIdAsync(id, cancellationToken);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProjectDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectDetailDto>> Update(
        int id,
        [FromBody] UpdateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var project = await projectEditorService.UpdateAsync(id, request, cancellationToken);
        return project is null ? NotFound() : Ok(project);
    }
}
