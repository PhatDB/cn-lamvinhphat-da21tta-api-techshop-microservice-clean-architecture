using ApiGateway.ReverseProxy.DependencyInjections.Extensions;
using ApiGateway.ReverseProxy.DependencyInjections.Options;
using Asp.Versioning;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddCors(options => { options.AddPolicy("CorsPolicy", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        List<SwaggerService>? swaggerServices = builder.Configuration.GetSection("SwaggerServices").Get<List<SwaggerService>>();

        foreach (SwaggerService service in swaggerServices) options.SwaggerEndpoint(service.Url, service.Name);
    });
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();