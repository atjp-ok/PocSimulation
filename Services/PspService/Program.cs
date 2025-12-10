using Microsoft.EntityFrameworkCore;
using PspService.slices.Capture;
using PspService.slices.Reserve;
using Shared.SharedModels.SharedDbContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new() { Title = "Psp Service API", Version = "v1" });
});
builder.Services.AddScoped<CaptureHandler>();
builder.Services.AddScoped<ReserveHandler>();
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
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Psp Service API v1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
