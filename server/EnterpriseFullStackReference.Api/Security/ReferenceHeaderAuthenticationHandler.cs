using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace EnterpriseFullStackReference.Api.Security;

public sealed class ReferenceHeaderAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "ReferenceHeader";
    public const string UserHeader = "X-Reference-User";
    public const string RoleHeader = "X-Reference-Role";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(UserHeader, out var user) || string.IsNullOrWhiteSpace(user))
            return Task.FromResult(AuthenticateResult.NoResult());

        if (!Request.Headers.TryGetValue(RoleHeader, out var role) || string.IsNullOrWhiteSpace(role))
            return Task.FromResult(AuthenticateResult.Fail("A reference role is required."));

        var roleValue = role.ToString();
        var allowedRoles = new[] { SecurityPolicies.ViewerRole, SecurityPolicies.EditorRole, SecurityPolicies.AdministratorRole };
        if (!allowedRoles.Contains(roleValue, StringComparer.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.Fail("The reference role is not recognized."));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.ToString()),
            new Claim(ClaimTypes.Role, roleValue)
        };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
