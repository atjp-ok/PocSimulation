using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using TankService.slices.Tank.StartTank;
using TankService.slices.Tank.GetTankStatus;
using TankService.slices.CompleteTank;
using Shared.SharedModels.SharedDbContext;
using Microsoft.EntityFrameworkCore;
using TankService.Slices.Tank.GetCompleted;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new() { Title = "Tank Service API", Version = "v1" });
});
builder.Services.AddScoped<StartTankHandler>();
builder.Services.AddScoped<GetTankStatusHandler>();
builder.Services.AddScoped<CompleteTankHandler>();
builder.Services.AddScoped<GetTankCompleteTransactiondHandler>();
builder.Services.AddDbContext<SharedDbContext>(Options =>
{
    Options.UseInMemoryDatabase("SharedDbContext");
});
builder.Services.AddHttpClient();
builder.Logging.AddFilter("TankService", LogLevel.Information);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Tank Service API v1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
