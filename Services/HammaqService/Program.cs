using HammaqService.slices.GetHammaqStatus;
using HammaqService.slices.StartHammaq;
using HammaqService.slices.Hammaq.CompleteHammaq;
using Shared.SharedModels.SharedDbContext;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new() { Title = "Hammq Service API", Version = "v1" });
});
builder.Services.AddScoped<GetStatusHandler>();
builder.Services.AddScoped<StartHammaqHandler>();
builder.Services.AddScoped<CompleteHammaqHandler>();
builder.Services.AddDbContext<SharedDbContext>(Options =>
{
    Options.UseInMemoryDatabase("SharedDbContext");
});
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Hammq Service API v1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
