using AA1.Repositories;
using Models;

namespace AA1.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero.");

            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Usuario usuario)
        {
            // CAMBIO: Validamos UsuarioNombre (antes Nombre)
            if (string.IsNullOrWhiteSpace(usuario.UsuarioNombre))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");

            // CAMBIO: Validamos Email (antes Apellido)
            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new ArgumentException("El email del usuario no puede estar vacío.");

            await _usuarioRepository.AddAsync(usuario);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            if (usuario.IdUsuario <= 0)
                throw new ArgumentException("El ID no es válido para actualización.");

            // CAMBIO: Validamos UsuarioNombre
            if (string.IsNullOrWhiteSpace(usuario.UsuarioNombre))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");

            await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido para eliminación.");

            await _usuarioRepository.DeleteAsync(id);
        }

        public async Task InicializarDatosAsync() {
            await _usuarioRepository.InicializarDatosAsync();
        }
    }
}