using System.ComponentModel.DataAnnotations;

namespace EnterpriseFullStackReference.Api.Contracts.Projects;

public sealed class UpdateProjectRequest
{
    [Required, StringLength(150)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(150)]
    public string CustomerName { get; init; } = string.Empty;

    [Required, StringLength(50)]
    public string Status { get; init; } = string.Empty;

    public DateTime? DueDate { get; init; }

    public bool IsActive { get; init; }
}
