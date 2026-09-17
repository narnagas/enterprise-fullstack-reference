using EnterpriseFullStackReference.Api.Contracts.Customers;
using EnterpriseFullStackReference.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseFullStackReference.Api.Services;

public sealed class CustomerEditorService(ApplicationDbContext dbContext) : ICustomerEditorService
{
    public async Task<CustomerDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new CustomerDetailDto(
                x.Id,
                x.CustomerNumber,
                x.CompanyName,
                x.ContactName,
                x.Email,
                x.Phone,
                x.City,
                x.State,
                x.CreatedDate,
                x.IsActive))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<CustomerDetailDto?> UpdateAsync(int id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (customer is null) return null;

        customer.CompanyName = request.CompanyName.Trim();
        customer.ContactName = request.ContactName.Trim();
        customer.Email = request.Email.Trim();
        customer.Phone = request.Phone.Trim();
        customer.City = request.City.Trim();
        customer.State = request.State.Trim();
        customer.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CustomerDetailDto(
            customer.Id,
            customer.CustomerNumber,
            customer.CompanyName,
            customer.ContactName,
            customer.Email,
            customer.Phone,
            customer.City,
            customer.State,
            customer.CreatedDate,
            customer.IsActive);
    }
}
