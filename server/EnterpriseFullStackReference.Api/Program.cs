using EnterpriseFullStackReference.Api.Data;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProjectSearchService, ProjectSearchService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
