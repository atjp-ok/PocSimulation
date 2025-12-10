using Microsoft.EntityFrameworkCore;
using Shared.SharedModels.SharedDbContext;
using UserProfileService.slices.UserProfile;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new() { Title = "UserProfile Service API", Version = "v1" });
});
builder.Services.AddScoped<UserProfileHandler>();
builder.Services.AddDbContext<SharedDbContext>(Options =>
{
    Options.UseInMemoryDatabase("SharedDbContext");
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
    // Seed data
    dbContext.UserProfiles.AddRange(
        new Shared.SharedModels.UserModels.UserProfileModel { UserId = 1, Name = "Søren" },
        new Shared.SharedModels.UserModels.UserProfileModel { UserId = 2, Name = "Anne" }
    );
    dbContext.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "UserProfile Service API v1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
