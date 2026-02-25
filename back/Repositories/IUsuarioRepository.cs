using Models;

namespace AA1.Repositories
{
    public interface IUsuarioRepository
    {
        // Obtiene todos los usuarios (ahora con los campos usuario, email, rol, etc.)
        Task<List<Usuario>> GetAllAsync();
        
        // Busca por la nueva PK idUsuario
        Task<Usuario?> GetByIdAsync(int id);
        
        // Recibe el modelo con UsuarioNombre, Email, Telefono, Contraseña y Rol
        Task AddAsync(Usuario usuario);
        
        // Actualiza los nuevos campos en la DB
        Task UpdateAsync(Usuario usuario);
        
        // Elimina por ID
        Task DeleteAsync(int idUsuario);

        // Mantiene la lógica de creación de datos de emergencia si la tabla está vacía
        Task InicializarDatosAsync();
    }
}