using AA1.Repositories;
using AA1.Controllers;
using AA1.Services;
using Microsoft.Data.SqlClient;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Forzamos a que la aplicación escuche en el puerto 3000 (importante para Docker)
builder.WebHost.UseUrls("http://*:3000");

// Obtenemos la cadena de conexión de appsettings.json
var connectionString = builder.Configuration.GetConnectionString("AA1");

// Inyección de dependencias - Repositorios
builder.Services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IPistaRepository, PistaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Inyección de dependencias - Servicios
builder.Services.AddScoped<IMantenimientoService, MantenimientoService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IPistaService, PistaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS: Permite que el Frontend (8080) acceda al Backend (3000) [cite: 75]
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();


// --- BLOQUE DE DEPURACIÓN E INICIALIZACIÓN ---
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    string scriptPath = Path.Combine(AppContext.BaseDirectory, "CreateDB.sql");

    // Construimos una cadena de conexión manual solo para la inicialización
    // Esto evita que el driver busque 'AA1' antes de tiempo
    var builderDb = new SqlConnectionStringBuilder(connectionString);
    builderDb.InitialCatalog = "master"; // Forzamos conexión a master
    string masterConn = builderDb.ConnectionString;

    bool initialized = false;
    for (int i = 0; i < 15; i++) 
    {
        try
        {
            using var connection = new SqlConnection(masterConn);
            connection.Open();
            
            if (File.Exists(scriptPath))
            {
                string script = File.ReadAllText(scriptPath);
                // Dividimos por GO para evitar errores de sintaxis
                var parts = script.Split(new[] { "GO", "go", "Go", "gO" }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var part in parts)
                {
                    if (!string.IsNullOrWhiteSpace(part))
                    {
                        using var command = new SqlCommand(part, connection);
                        command.ExecuteNonQuery();
                    }
                }
                logger.LogInformation(">>> SQL: ¡ÉXITO! Base de datos AA1 y tablas creadas.");
                initialized = true;
            }
            break; 
        }
        catch (Exception ex)
        {
            logger.LogWarning($">>> SQL: Intento {i + 1}/15. Esperando a que el contenedor DB esté listo...");
            Thread.Sleep(5000); 
        }
    }
    if (!initialized) logger.LogCritical(">>> SQL: No se pudo crear la base de datos.");
}

// Configuración de Swagger para que sea accesible en Docker 
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AA1 API V1");
    c.RoutePrefix = "swagger"; 
});

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();