using AA1.Repositories;
using AA1.Controllers;
using AA1.Services;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions; // IMPORTANTE: Nueva librería para el Regex

var builder = WebApplication.CreateBuilder(args);

// Configuración de red para Docker
builder.WebHost.UseUrls("http://*:3000");

var connectionString = builder.Configuration.GetConnectionString("AA1");

// --- INYECCIÓN DE DEPENDENCIAS ---
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

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// --- LÓGICA DE INICIALIZACIÓN DE BASE DE DATOS ---
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    string scriptPath = Path.Combine(AppContext.BaseDirectory, "CreateDB.sql");

    var builderDb = new SqlConnectionStringBuilder(connectionString);
    builderDb.InitialCatalog = "master"; 
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
                
                // CAMBIO CLAVE: Regex para que solo corte en "GO" aislados y no en palabras como "Goya"
                var parts = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                
                foreach (var part in parts)
                {
                    var cleanPart = part.Trim().Trim('\uFEFF', '\u200B');
                    if (!string.IsNullOrWhiteSpace(cleanPart))
                    {
                        using var command = new SqlCommand(cleanPart, connection);
                        command.ExecuteNonQuery();
                    }
                }
                logger.LogInformation(">>> SQL: Script CreateDB ejecutado correctamente.");
                initialized = true;
            }

            // --- INSERCIÓN DE EMERGENCIA ---
            using (var checkConn = new SqlConnection(connectionString)) 
            {
                checkConn.Open();
                string checkQuery = "IF (SELECT COUNT(*) FROM PISTAS) = 0 " +
                                   "INSERT INTO PISTAS (nombre, tipo, direccion, activa, precioHora) " +
                                   "VALUES ('Pista Manual Program', 'Multiuso', 'Creada desde C#', 1, 15)";
                using var cmd = new SqlCommand(checkQuery, checkConn);
                cmd.ExecuteNonQuery();
            }
            
            break; 
        }
        catch (Exception ex)
        {
            logger.LogWarning($">>> SQL: Intento {i + 1}/15. Error: {ex.Message}");
            Thread.Sleep(5000); 
        }
    }
}

// --- CONFIGURACIÓN DE MIDDLEWARE ---
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AA1 API V1");
    c.RoutePrefix = "swagger"; 
});

app.UseRouting();

app.UseCors("AllowAll"); 

//app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();