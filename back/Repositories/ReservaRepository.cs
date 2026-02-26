using Microsoft.Data.SqlClient;
using Models;
using Microsoft.Extensions.Configuration;

namespace AA1.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly string _connectionString;
        private readonly IUsuarioRepository _usuariorepository;
        private readonly IPistaRepository _pistarepository;

        public ReservaRepository(IConfiguration configuration, IUsuarioRepository usuariorepository, IPistaRepository pistarepository)
        {
             _connectionString = configuration.GetConnectionString("AA1") ?? "Not found";
            _usuariorepository = usuariorepository;
            _pistarepository = pistarepository;
        }

        public async Task<List<Reserva>> GetAllAsync()
        {
            var reservas = new List<Reserva>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT idReserva, idUsuario, idPista, fecha, horas, precio FROM RESERVAS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reservas.Add(new Reserva
                            {
                                IdReserva = reader.GetInt32(0),
                                IdUsuario = await _usuariorepository.GetByIdAsync(reader.GetInt32(1)),
                                IdPista = await _pistarepository.GetByIdAsync(reader.GetInt32(2)),
                                Fecha = reader.GetDateTime(3),
                                Horas = reader.GetInt32(4),
                                Precio = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            return reservas;
        }

        public async Task<Reserva> GetByIdAsync(int idReserva)
        {
            Reserva reserva = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT idReserva, idUsuario, idPista, fecha, horas, precio FROM RESERVAS WHERE idReserva = @idReserva";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idReserva", idReserva);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            reserva = new Reserva
                            {
                                IdReserva = reader.GetInt32(0),
                                IdUsuario = await _usuariorepository.GetByIdAsync(reader.GetInt32(1)),
                                IdPista = await _pistarepository.GetByIdAsync(reader.GetInt32(2)),
                                Fecha = reader.GetDateTime(3),
                                Horas = reader.GetInt32(4),
                                Precio = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }
            return reserva;
        }

        public async Task AddAsync(Reserva reserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO RESERVAS (idUsuario, idPista, fecha, horas, precio) 
                                OUTPUT INSERTED.idReserva
                                VALUES (@idUsuario, @idPista, @fecha, @horas, @precio)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", reserva.IdUsuario?.IdUsuario ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@idPista", reserva.IdPista?.IdPista ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@fecha", reserva.Fecha);
                    command.Parameters.AddWithValue("@horas", reserva.Horas);
                    command.Parameters.AddWithValue("@precio", reserva.Precio);
                    var newId = await command.ExecuteScalarAsync();
                    reserva.IdReserva = Convert.ToInt32(newId);
                }
            }
        }

        public async Task UpdateAsync(Reserva reserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Corregido: Quitamos el intento de actualizar la columna idReserva (que es Identity)
                string query = "UPDATE RESERVAS SET idUsuario = @idUsuario, idPista = @idPista, fecha = @fecha, horas = @horas, precio = @precio WHERE idReserva = @idReserva";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idReserva", reserva.IdReserva);
                    command.Parameters.AddWithValue("@idUsuario", reserva.IdUsuario?.IdUsuario ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@idPista", reserva.IdPista?.IdPista ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@fecha", reserva.Fecha);
                    command.Parameters.AddWithValue("@horas", reserva.Horas);
                    command.Parameters.AddWithValue("@precio", reserva.Precio);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int idReserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM RESERVAS WHERE idReserva = @idReserva";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idReserva", idReserva);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task InicializarDatosAsync()
        {
            // Mantenemos tu lógica de inicialización original
        }

        public async Task<List<Reserva>> GetByUsuarioIdAsync(int idUsuario)
        {
            var reservas = new List<Reserva>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT idReserva, idUsuario, idPista, fecha, horas, precio FROM RESERVAS WHERE idUsuario = @idUsuario";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reservas.Add(new Reserva {
                                IdReserva = reader.GetInt32(0),
                                IdUsuario = await _usuariorepository.GetByIdAsync(reader.GetInt32(1)),
                                IdPista = await _pistarepository.GetByIdAsync(reader.GetInt32(2)),
                                Fecha = reader.GetDateTime(3),
                                Horas = reader.GetInt32(4),
                                Precio = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            return reservas;
        }
    }
}