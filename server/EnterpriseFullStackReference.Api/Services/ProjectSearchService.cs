using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Projects;
using EnterpriseFullStackReference.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseFullStackReference.Api.Services;

public sealed class ProjectSearchService(ApplicationDbContext dbContext)
    : IProjectSearchService
{
    public async Task<PagedResponse<ProjectSummaryDto>> SearchAsync(
        ProjectSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(request.PageNumber, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var query = dbContext.Projects
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.ProjectNumber.Contains(search) ||
                x.Name.Contains(search) ||
                x.CustomerName.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(request.Customer))
        {
            var customer = request.Customer.Trim();
            query = query.Where(x => x.CustomerName.Contains(customer));
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = request.Status.Trim();
            query = query.Where(x => x.Status == status);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, request.SortField, request.SortDirection);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProjectSummaryDto(
                x.Id,
                x.ProjectNumber,
                x.Name,
                x.CustomerName,
                x.Status,
                x.CreatedDate,
                x.DueDate,
                x.IsActive))
            .ToListAsync(cancellationToken);

        return new PagedResponse<ProjectSummaryDto>(
            items,
            pageNumber,
            pageSize,
            totalCount);
    }

    private static IQueryable<Models.Project> ApplySorting(
        IQueryable<Models.Project> query,
        string? sortField,
        string? sortDirection)
    {
        var descending = string.Equals(
            sortDirection,
            "desc",
            StringComparison.OrdinalIgnoreCase);

        return sortField?.Trim().ToLowerInvariant() switch
        {
            "projectnumber" => descending
                ? query.OrderByDescending(x => x.ProjectNumber)
                : query.OrderBy(x => x.ProjectNumber),
            "name" => descending
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name),
            "customername" => descending
                ? query.OrderByDescending(x => x.CustomerName)
                : query.OrderBy(x => x.CustomerName),
            "status" => descending
                ? query.OrderByDescending(x => x.Status)
                : query.OrderBy(x => x.Status),
            "duedate" => descending
                ? query.OrderByDescending(x => x.DueDate)
                : query.OrderBy(x => x.DueDate),
            _ => descending
                ? query.OrderByDescending(x => x.CreatedDate)
                : query.OrderBy(x => x.CreatedDate)
        };
    }
}
