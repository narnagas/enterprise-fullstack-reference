using EnterpriseFullStackReference.Api.Data;
using EnterpriseFullStackReference.Api.Security;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string LocalClientPolicy = "LocalAngularClient";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy(LocalClientPolicy, policy =>
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());
});

builder.Services
    .AddAuthentication(ReferenceHeaderAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, ReferenceHeaderAuthenticationHandler>(
        ReferenceHeaderAuthenticationHandler.SchemeName, _ => { });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(SecurityPolicies.CanRead, policy =>
        policy.RequireRole(SecurityPolicies.ViewerRole, SecurityPolicies.EditorRole, SecurityPolicies.AdministratorRole));
    options.AddPolicy(SecurityPolicies.CanEdit, policy =>
        policy.RequireRole(SecurityPolicies.EditorRole, SecurityPolicies.AdministratorRole));
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProjectSearchService, ProjectSearchService>();
builder.Services.AddScoped<IProjectEditorService, ProjectEditorService>();
builder.Services.AddScoped<ICustomerSearchService, CustomerSearchService>();
builder.Services.AddScoped<ICustomerEditorService, CustomerEditorService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors(LocalClientPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
