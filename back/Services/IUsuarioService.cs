using Models;

namespace AA1.Services
{
    public interface IUsuarioService
    {
        // Obtiene la lista completa de usuarios con sus nuevos roles y datos
        Task<List<Usuario>> GetAllAsync();

        // Busca un usuario por su IdUsuario (PK)
        Task<Usuario?> GetByIdAsync(int id);

        // Registra un nuevo usuario con UsuarioNombre, Email, Telefono, Contraseña y Rol
        Task AddAsync(Usuario usuario);

        // Actualiza la información del usuario en la base de datos
        Task UpdateAsync(Usuario usuario);

        // Elimina el registro del usuario
        Task DeleteAsync(int id);

        // Lógica para asegurar que la base de datos tenga datos de prueba (admin/user)
        Task InicializarDatosAsync();
    }
}