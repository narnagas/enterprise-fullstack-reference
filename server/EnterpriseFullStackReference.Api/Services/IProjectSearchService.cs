using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Projects;

namespace EnterpriseFullStackReference.Api.Services;

public interface IProjectSearchService
{
    Task<PagedResponse<ProjectSummaryDto>> SearchAsync(
        ProjectSearchRequest request,
        CancellationToken cancellationToken = default);
}
