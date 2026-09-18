using Microsoft.EntityFrameworkCore;
using RestaurantHub.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RestaurantHubDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RestaurantHubDB")));

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RestaurantHubDbContext>();
    Console.WriteLine($"Conexión a SQL Server: {db.Database.CanConnect()}");
}

app.Run();