using BFF.NewBFF.Slices.Appservice.CompleteParking;
using BFF.NewBFF.Slices.Appservice.GetParkingStatus;
using BFF.NewBFF.Slices.Appservice.StartParking;
using BFF.NewBFF.Slices.Tank.CompleteTank;
using BFF.NewBFF.Slices.Tank.GetTankStatus;
using BFF.NewBFF.Slices.Tank.StartTank;
using BFF.NewBFF.Slices.Vask.CompleteVask;
using BFF.NewBFF.Slices.Vask.GetVaskStatus;
using BFF.NewBFF.Slices.Vask.StartVask;
using NewBFF.Slices.Expenses;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new() { Title = "NewBFF API", Version = "v1" });
});
builder.Services.AddScoped<StartParkingHandler>();
builder.Services.AddScoped<GetParkingStatusHandler>();
builder.Services.AddScoped<CompleteParkingHandler>();

builder.Services.AddScoped<StartVaskHandler>();
builder.Services.AddScoped<GetStatusVaskHandler>();
builder.Services.AddScoped<CompleteVaskHandler>();

builder.Services.AddScoped<StartTankHandler>();
builder.Services.AddScoped<GetTankStatusHandler>();
builder.Services.AddScoped<CompleteTankHandler>();
builder.Services.AddScoped<ExpenseHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "NewBFF API v1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
