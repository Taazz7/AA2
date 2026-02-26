using Microsoft.Data.SqlClient;
using Models;
using AA1.Repositories;

namespace AA1.Repositories
{
    public class PistaRepository : IPistaRepository
    {
        private readonly string _connectionString;

        public PistaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AA1") ?? "Not found";
        }

        public async Task<List<Pista>> GetAllAsync()
        {
            var pistas = new List<Pista>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idPista, nombre, tipo, direccion, activa, precioHora FROM AA1.dbo.PISTAS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var pista = new Pista
                            {
                                IdPista = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Tipo = reader.GetString(2),
                                Direccion = reader.GetString(3),
                                Activa = reader.GetBoolean(4),
                                PrecioHora = reader.GetInt32(5)
                            }; 

                            pistas.Add(pista);
                        }
                    }
                }
            }
            return pistas;
        }

        public async Task<Pista> GetByIdAsync(int idPista)
        {
            Pista pista = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idPista, nombre, tipo, direccion, activa, precioHora  FROM PISTAS WHERE idPista = @IdPista";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPista", idPista);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            pista = new Pista
                            {
                                IdPista = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Tipo = reader.GetString(2),
                                Direccion = reader.GetString(3),
                                Activa = reader.GetBoolean(4),
                                PrecioHora = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }
            return pista;
        }

        public async Task AddAsync(Pista pista)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // QUITAMOS IdPista del INSERT
                string query = @"INSERT INTO PISTAS (Nombre, Tipo, Direccion, Activa, PrecioHora) 
                                OUTPUT INSERTED.idPista
                                VALUES (@Nombre, @Tipo, @Direccion, @Activa, @PrecioHora)";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", pista.Nombre);
                    command.Parameters.AddWithValue("@Tipo", pista.Tipo);
                    command.Parameters.AddWithValue("@Direccion", pista.Direccion);
                    command.Parameters.AddWithValue("@Activa", pista.Activa);
                    command.Parameters.AddWithValue("@PrecioHora", pista.PrecioHora);

                    var newId = await command.ExecuteScalarAsync();
                    pista.IdPista = Convert.ToInt32(newId);
                }
            }
        }



        public async Task UpdateAsync(Pista pista)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE PISTAS SET Nombre = @Nombre, Tipo = @Tipo, Direccion = @Direccion, Activa = @Activa, PrecioHora = @PrecioHora WHERE idPista = @IdPista";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", pista.Nombre);
                    command.Parameters.AddWithValue("@Tipo", pista.Tipo);
                    command.Parameters.AddWithValue("@Direccion", pista.Direccion);
                    command.Parameters.AddWithValue("@Activa", pista.Activa);
                    command.Parameters.AddWithValue("@PrecioHora", pista.PrecioHora);
                    command.Parameters.AddWithValue("@IdPista", pista.IdPista);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int idPista)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM PISTAS WHERE idPista = @IdPista";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPista", idPista);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Pista>> GetAllFilteredAsync(string? tipo, bool? activa, string? orderBy, bool ascending)
        {
            var pistas = new List<Pista>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idPista, nombre, tipo, direccion, activa, precioHora FROM PISTAS WHERE 1=1";
                var parameters = new List<SqlParameter>();

                // Filtros
                if (!string.IsNullOrWhiteSpace(tipo))
                {
                    query += " AND tipo = @Tipo";
                    parameters.Add(new SqlParameter("@Tipo", tipo));
                }

                if (activa.HasValue)
                {
                    query += " AND activa = @Activa";
                    parameters.Add(new SqlParameter("@Activa", activa.Value));
                }

                // Ordenación
                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    var validColumns = new[] { "nombre", "tipo", "preciohora", "direccion" };
                    var orderByLower = orderBy.ToLower();
                    
                    if (validColumns.Contains(orderByLower))
                    {
                        var direction = ascending ? "ASC" : "DESC";
                        query += $" ORDER BY {orderByLower} {direction}";
                    }else
                {
                    query += " ORDER BY nombre ASC";
                }
                }
                

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var pista = new Pista
                            {
                                IdPista = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Tipo = reader.GetString(2),
                                Direccion = reader.GetString(3),
                                Activa = reader.GetBoolean(4),
                                PrecioHora = reader.GetInt32(5)
                            };

                            pistas.Add(pista);
                        }
                    }
                }
            }
            return pistas;
        }


        public async Task InicializarDatosAsync()
{
    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();

        // QUITAMOS IdPista de la lista de columnas y de los valores
        var query = @"
            INSERT INTO PISTAS (Nombre, Tipo, Direccion, Activa, PrecioHora)
            VALUES 
            (@Nombre1, @Tipo1, @Direccion1, @Activa1, @PrecioHora1),
            (@Nombre2, @Tipo2, @Direccion2, @Activa2, @PrecioHora2)";

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Nombre1", "CDM Mudejar");
            command.Parameters.AddWithValue("@Tipo1", "baloncesto");
            command.Parameters.AddWithValue("@Direccion1", "c/NoTengoNiIdea");
            command.Parameters.AddWithValue("@Activa1", true);
            command.Parameters.AddWithValue("@PrecioHora1", 5);

            command.Parameters.AddWithValue("@Nombre2", "La Romareda");
            command.Parameters.AddWithValue("@Tipo2", "futbol");
            command.Parameters.AddWithValue("@Direccion2", "c/ParadaDelTranvia");
            command.Parameters.AddWithValue("@Activa2", false);
            command.Parameters.AddWithValue("@PrecioHora2", 7);

            await command.ExecuteNonQueryAsync();
        }
    }
}


    }

}