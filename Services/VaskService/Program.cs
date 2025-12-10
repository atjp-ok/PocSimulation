using VaskService.Slices.GetStatusService;
using VaskService.Slices.Vask.StartVask;
using VaskService.Slices.CompleteVask;
using Shared.SharedModels.SharedDbContext;
using Microsoft.EntityFrameworkCore;
using VaskService.Slices.Vask.GetCompleted;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new() { Title = "Vask Service API", Version = "v1" });
    s.SupportNonNullableReferenceTypes();
}
);
builder.Services.AddScoped<StartVaskHandler>();
builder.Services.AddScoped<GetStatusVaskHandler>();
builder.Services.AddScoped<CompleteVaskHandler>();
builder.Services.AddScoped<GetCompletedTransactionHandler>();

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
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Vask Service API v1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
