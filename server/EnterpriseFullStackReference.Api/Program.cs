using EnterpriseFullStackReference.Api.Data;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string LocalClientPolicy = "LocalAngularClient";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy(LocalClientPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProjectSearchService, ProjectSearchService>();
builder.Services.AddScoped<IProjectEditorService, ProjectEditorService>();
builder.Services.AddScoped<ICustomerSearchService, CustomerSearchService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(LocalClientPolicy);
app.MapControllers();

app.Run();
