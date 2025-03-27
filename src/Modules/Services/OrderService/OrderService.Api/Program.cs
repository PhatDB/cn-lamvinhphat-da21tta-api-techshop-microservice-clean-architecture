using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.Builder;
using BuildingBlocks;
using BuildingBlocks.Extensions;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Persistence.DependencyInjections;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBuildingBlocks().AddApplication().AddPersistence(builder.Configuration).AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();
ApiVersionSet apiVersionSet = app.NewApiVersionSet().HasApiVersion(new ApiVersion(1)).ReportApiVersions().Build();
RouteGroupBuilder versionedGroup = app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.Run();