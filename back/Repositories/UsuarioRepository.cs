using Microsoft.Data.SqlClient;
using Models;

namespace AA1.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
             _connectionString = configuration.GetConnectionString("AA1") ?? "Not found";
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // CAMBIO: Nombres de columnas según tu nueva tabla
                string query = "SELECT idUsuario, usuario, email, telefono, contraseña, rol FROM USUARIOS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                UsuarioNombre = reader.GetString(1), // Mapea 'usuario'
                                Email = reader.GetString(2),         // Mapea 'email'
                                Telefono = reader.GetInt32(3),
                                Contraseña = reader.GetString(4),    // Mapea 'contraseña'
                                Rol = reader.GetString(5)            // Mapea 'rol'
                            }; 

                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            return usuarios;
        }

        public async Task<Usuario?> GetByIdAsync(int idUsuario)
        {
            Usuario? usuario = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idUsuario, usuario, email, telefono, contraseña, rol FROM USUARIOS WHERE idUsuario = @IdUsuario";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                UsuarioNombre = reader.GetString(1),
                                Email = reader.GetString(2),
                                Telefono = reader.GetInt32(3),
                                Contraseña = reader.GetString(4),
                                Rol = reader.GetString(5)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public async Task AddAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // CAMBIO: Query adaptada a los campos de la nueva tabla
                string query = @"INSERT INTO USUARIOS (usuario, email, telefono, contraseña, rol) 
                                OUTPUT INSERTED.idUsuario
                                VALUES (@usuario, @email, @telefono, @contraseña, @rol)";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usuario", usuario.UsuarioNombre);
                    command.Parameters.AddWithValue("@email", usuario.Email);
                    command.Parameters.AddWithValue("@telefono", usuario.Telefono);
                    command.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                    command.Parameters.AddWithValue("@rol", usuario.Rol);

                    var newId = await command.ExecuteScalarAsync();
                    usuario.IdUsuario = Convert.ToInt32(newId);
                }
            }
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // CAMBIO: No se debe intentar actualizar el IdUsuario en el SET. 
                // Se actualizan los campos nuevos.
                string query = @"UPDATE USUARIOS 
                                 SET usuario = @usuario, 
                                     email = @email, 
                                     telefono = @telefono, 
                                     contraseña = @contraseña, 
                                     rol = @rol 
                                 WHERE idUsuario = @IdUsuario";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    command.Parameters.AddWithValue("@usuario", usuario.UsuarioNombre);
                    command.Parameters.AddWithValue("@email", usuario.Email);
                    command.Parameters.AddWithValue("@telefono", usuario.Telefono);
                    command.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                    command.Parameters.AddWithValue("@rol", usuario.Rol);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int idUsuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM USUARIOS WHERE idUsuario = @IdUsuario";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task InicializarDatosAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // CAMBIO: Datos iniciales adaptados a la lógica de Auth (admin/user)
                var query = @"
                    IF (SELECT COUNT(*) FROM USUARIOS) = 0
                    BEGIN
                        INSERT INTO USUARIOS (usuario, email, telefono, contraseña, rol)
                        VALUES 
                        (@usuario1, @email1, @telefono1, @pass1, @rol1),
                        (@usuario2, @email2, @telefono2, @pass2, @rol2)
                    END";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usuario1", "admin");
                    command.Parameters.AddWithValue("@email1", "admin@pistas.com");
                    command.Parameters.AddWithValue("@telefono1", 600000001);
                    command.Parameters.AddWithValue("@pass1", "admin123");
                    command.Parameters.AddWithValue("@rol1", "admin");

                    command.Parameters.AddWithValue("@usuario2", "user");
                    command.Parameters.AddWithValue("@email2", "user@pistas.com");
                    command.Parameters.AddWithValue("@telefono2", 600000002);
                    command.Parameters.AddWithValue("@pass2", "user123");
                    command.Parameters.AddWithValue("@rol2", "user");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}