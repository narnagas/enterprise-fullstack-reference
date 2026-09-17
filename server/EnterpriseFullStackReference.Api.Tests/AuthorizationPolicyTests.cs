using System.Security.Claims;
using EnterpriseFullStackReference.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EnterpriseFullStackReference.Api.Tests;

public sealed class AuthorizationPolicyTests
{
    private static IAuthorizationService CreateAuthorizationService()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(SecurityPolicies.CanRead, policy =>
                policy.RequireRole(SecurityPolicies.ViewerRole, SecurityPolicies.EditorRole, SecurityPolicies.AdministratorRole));
            options.AddPolicy(SecurityPolicies.CanEdit, policy =>
                policy.RequireRole(SecurityPolicies.EditorRole, SecurityPolicies.AdministratorRole));
        });
        return services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();
    }

    private static ClaimsPrincipal User(string role) => new(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.Name, "reference.user"),
        new Claim(ClaimTypes.Role, role)
    }, "test"));

    [Theory]
    [InlineData(SecurityPolicies.ViewerRole)]
    [InlineData(SecurityPolicies.EditorRole)]
    [InlineData(SecurityPolicies.AdministratorRole)]
    public async Task CanRead_AllowsKnownRoles(string role)
        => Assert.True((await CreateAuthorizationService().AuthorizeAsync(User(role), null, SecurityPolicies.CanRead)).Succeeded);

    [Fact]
    public async Task CanEdit_RejectsViewer()
        => Assert.False((await CreateAuthorizationService().AuthorizeAsync(User(SecurityPolicies.ViewerRole), null, SecurityPolicies.CanEdit)).Succeeded);

    [Theory]
    [InlineData(SecurityPolicies.EditorRole)]
    [InlineData(SecurityPolicies.AdministratorRole)]
    public async Task CanEdit_AllowsEditorAndAdministrator(string role)
        => Assert.True((await CreateAuthorizationService().AuthorizeAsync(User(role), null, SecurityPolicies.CanEdit)).Succeeded);
}
