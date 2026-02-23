using AA1.Repositories;
using AA1.Controllers;
using AA1.Services;

var builder = WebApplication.CreateBuilder(args);

// Forzamos a que la aplicación escuche en el puerto 3000 (importante para Docker)
builder.WebHost.UseUrls("http://*:3000");

var connectionString = builder.Configuration.GetConnectionString("AA1");

// Add services to the container.
builder.Services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IPistaRepository, PistaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IMantenimientoService, MantenimientoService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IPistaService, PistaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS: Vital para que el Frontend (puerto 8080/5173) pueda leer esta API (puerto 3000)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// MODIFICACIÓN: Sacamos Swagger del IF de Development para que funcione siempre en Docker
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AA1 API V1");
    c.RoutePrefix = "swagger"; // Esto hace que accedas en http://localhost:3000/swagger
});


app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();