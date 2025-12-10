using Microsoft.EntityFrameworkCore;
using Shared.SharedModels.SharedDbContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new() { Title = "App Service API", Version = "v1" });
});
builder.Services.AddScoped<AppService.Services.IAppService, AppService.Services.AppService>();
builder.Services.AddDbContext<SharedDbContext>(Options =>
{
    Options.UseInMemoryDatabase("SharedDbContext");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "App Service API v1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
