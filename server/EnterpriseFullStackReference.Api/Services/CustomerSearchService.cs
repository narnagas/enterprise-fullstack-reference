using EnterpriseFullStackReference.Api.Contracts.Common;
using EnterpriseFullStackReference.Api.Contracts.Customers;
using EnterpriseFullStackReference.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseFullStackReference.Api.Services;

public sealed class CustomerSearchService(ApplicationDbContext dbContext) : ICustomerSearchService
{
    public async Task<PagedResponse<CustomerSummaryDto>> SearchAsync(
        CustomerSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(request.PageNumber, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var query = dbContext.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x =>
                x.CustomerNumber.ToLower().Contains(search) ||
                x.CompanyName.ToLower().Contains(search) ||
                x.ContactName.ToLower().Contains(search) ||
                x.Email.ToLower().Contains(search) ||
                x.City.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(request.State))
        {
            var state = request.State.Trim().ToLower();
            query = query.Where(x => x.State.ToLower() == state);
        }

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        query = ApplySorting(query, request.SortField, request.SortDirection);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new CustomerSummaryDto(
                x.Id, x.CustomerNumber, x.CompanyName, x.ContactName,
                x.Email, x.Phone, x.City, x.State, x.CreatedDate, x.IsActive))
            .ToListAsync(cancellationToken);

        return new PagedResponse<CustomerSummaryDto>(items, pageNumber, pageSize, totalCount);
    }

    private static IQueryable<Models.Customer> ApplySorting(
        IQueryable<Models.Customer> query,
        string? sortField,
        string? sortDirection)
    {
        var descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
        return sortField?.Trim().ToLowerInvariant() switch
        {
            "customernumber" => descending ? query.OrderByDescending(x => x.CustomerNumber) : query.OrderBy(x => x.CustomerNumber),
            "contactname" => descending ? query.OrderByDescending(x => x.ContactName) : query.OrderBy(x => x.ContactName),
            "city" => descending ? query.OrderByDescending(x => x.City) : query.OrderBy(x => x.City),
            "state" => descending ? query.OrderByDescending(x => x.State) : query.OrderBy(x => x.State),
            "createddate" => descending ? query.OrderByDescending(x => x.CreatedDate) : query.OrderBy(x => x.CreatedDate),
            _ => descending ? query.OrderByDescending(x => x.CompanyName) : query.OrderBy(x => x.CompanyName)
        };
    }
}
