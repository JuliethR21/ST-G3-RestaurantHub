using Microsoft.EntityFrameworkCore;
using RestaurantHub.API.Data;
using RestaurantHub.API.Exceptions;
using RestaurantHub.API.Repositories;
using RestaurantHub.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RestaurantHubDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RestaurantHubDB")));

// Capa de acceso a datos
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<ProductoRepository>();
builder.Services.AddScoped<IngredienteRepository>();
builder.Services.AddScoped<PedidoRepository>();
builder.Services.AddScoped<PersonaRepository>();

// Capa de negocio
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<IngredienteService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<PersonaService>();

// Capa de presentación (API)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Permite que la interfaz web (index.html) consuma la API desde otro origen
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Convierte las excepciones de negocio en respuestas HTTP con un mensaje claro
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (NoEncontradoException ex)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (ReglaNegocioException ex)
    {
        context.Response.StatusCode = StatusCodes.Status409Conflict;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // En desarrollo se deja HTTP para que el frontend local pueda llamar a la API sin redirecciones
    app.UseHttpsRedirection();
}

app.UseCors();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RestaurantHubDbContext>();
    Console.WriteLine($"Conexión a SQL Server: {db.Database.CanConnect()}");
}

app.Run();
