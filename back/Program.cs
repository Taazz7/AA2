using AA1.Repositories;
using AA1.Controllers;
using AA1.Services;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE RED Y PUERTOS ---
builder.WebHost.UseUrls("http://*:3000");

var connectionString = builder.Configuration.GetConnectionString("AA1");

// --- INYECCIÓN DE DEPENDENCIAS ---
// Repositorios
builder.Services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IPistaRepository, PistaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Servicios
builder.Services.AddScoped<IMantenimientoService, MantenimientoService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IPistaService, PistaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CONFIGURACIÓN DE CORS ---
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
    string scriptPath = Path.Combine(AppContext.BaseDirectory, "CreateDB.sql");

    var builderDb = new SqlConnectionStringBuilder(connectionString);
    builderDb.InitialCatalog = "master"; 
    string masterConn = builderDb.ConnectionString;

    for (int i = 0; i < 15; i++) 
    {
        try
        {
            using var connection = new SqlConnection(masterConn);
            connection.Open();
            
            if (File.Exists(scriptPath))
            {
                string script = File.ReadAllText(scriptPath);
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
            }

            // --- INSERCIÓN DE EMERGENCIA (Pistas y Usuarios) ---
            using (var checkConn = new SqlConnection(connectionString)) 
            {
                checkConn.Open();
                
                // Inserción de Usuarios
                string checkUsersQuery = @"
                    IF (SELECT COUNT(*) FROM USUARIOS) = 0 
                    BEGIN
                        INSERT INTO USUARIOS (usuario, email, telefono, contraseña, rol) 
                        VALUES ('admin', 'admin@pistas.com', 600000000, 'admin123', 'admin');
                        
                        INSERT INTO USUARIOS (usuario, email, telefono, contraseña, rol) 
                        VALUES ('user', 'user@pistas.com', 611223344, 'user123', 'user');
                    END";
                using var cmdUser = new SqlCommand(checkUsersQuery, checkConn);
                cmdUser.ExecuteNonQuery();

                // Inserción de Pistas
                string checkPistasQuery = @"
                    IF (SELECT COUNT(*) FROM PISTAS) = 0 
                    BEGIN
                        INSERT INTO PISTAS (nombre, tipo, direccion, activa, precioHora) 
                        VALUES ('Pista Central', 'Tenis', 'Calle Principal 1', 1, 25),
                               ('Pista Norte', 'Padel', 'Calle Norte 2', 1, 20);
                    END";
                using var cmdPista = new SqlCommand(checkPistasQuery, checkConn);
                cmdPista.ExecuteNonQuery();
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

// --- CONFIGURACIÓN DE MIDDLEWARE (EL ORDEN IMPORTA) ---
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