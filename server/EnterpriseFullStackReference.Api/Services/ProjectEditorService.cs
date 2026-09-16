using EnterpriseFullStackReference.Api.Contracts.Projects;
using EnterpriseFullStackReference.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseFullStackReference.Api.Services;

public sealed class ProjectEditorService(ApplicationDbContext dbContext) : IProjectEditorService
{
    public async Task<ProjectDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => ToDetail(x.Id, x.ProjectNumber, x.Name, x.CustomerName, x.Status, x.CreatedDate, x.DueDate, x.IsActive))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ProjectDetailDto?> UpdateAsync(int id, UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = await dbContext.Projects.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (project is null) return null;

        project.Name = request.Name.Trim();
        project.CustomerName = request.CustomerName.Trim();
        project.Status = request.Status.Trim();
        project.DueDate = request.DueDate;
        project.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return ToDetail(project.Id, project.ProjectNumber, project.Name, project.CustomerName, project.Status, project.CreatedDate, project.DueDate, project.IsActive);
    }

    private static ProjectDetailDto ToDetail(
        int id,
        string projectNumber,
        string name,
        string customerName,
        string status,
        DateTime createdDate,
        DateTime? dueDate,
        bool isActive) =>
        new(id, projectNumber, name, customerName, status, createdDate, dueDate, isActive);
}
