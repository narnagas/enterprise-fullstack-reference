using EnterpriseFullStackReference.Api.Contracts.Projects;

namespace EnterpriseFullStackReference.Api.Services;

public interface IProjectEditorService
{
    Task<ProjectDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProjectDetailDto?> UpdateAsync(int id, UpdateProjectRequest request, CancellationToken cancellationToken = default);
}
