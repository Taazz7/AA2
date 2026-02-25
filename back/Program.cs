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

// --- LÓGICA DE INICIALIZACIÓN DE BASE DE DATOS ---
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    // El archivo CreateDB.sql debe estar en la raíz de ejecución del contenedor
    string scriptPath = Path.Combine(AppContext.BaseDirectory, "CreateDB.sql");

    // Creamos una cadena de conexión temporal a 'master' para crear la base de datos AA1
    var masterConnectionString = (connectionString ?? "").Replace("Database=AA1", "Database=master");

    bool initialized = false;
    for (int i = 0; i < 15; i++) // 15 intentos (aprox 75 segundos)
    {
        try
        {
            using var connection = new SqlConnection(masterConnectionString);
            connection.Open();
            
            if (File.Exists(scriptPath))
            {
                string script = File.ReadAllText(scriptPath);
                using var command = new SqlCommand(script, connection);
                command.ExecuteNonQuery();
                logger.LogInformation(">>> SQL: Base de datos y tablas verificadas/creadas correctamente.");
                initialized = true;
            }
            else 
            {
                logger.LogError($">>> SQL: No se encuentra el archivo {scriptPath}");
            }
            break; 
        }
        catch (Exception ex)
        {
            logger.LogWarning($">>> SQL: Esperando a SQL Server... Intento {i + 1}/15. (Error: {ex.Message})");
            Thread.Sleep(5000); // Espera 5 segundos entre intentos
        }
    }
    if (!initialized) logger.LogCritical(">>> SQL: No se pudo inicializar la base de datos.");
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